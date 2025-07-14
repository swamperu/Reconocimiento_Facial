using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCONT.Dominio.Entidades;
using SCONT.Infraestructura.Dao;
using SCONT.Infraestructura.Transversal;
using SCONT.Presentacion.Web.Controllers;


namespace SCONT.Presentacion.Web.Helpers
{
    public class HelperActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //1.Verifica si esta en modo encriptacion
            var isEncrypt = sisVariable.IsEncrypt;
            var actionExceptions = new List<string>()
            {
                "ACCOUNT/PRESENTACION",
                "ACCOUNT/LOGINREGION",
                "ACCOUNT/AUTENTICADO",
                "ACCOUNT/LOGIN",
                "ACCOUNT/LOGOFF",
                "ERROR/ACCESODENEGADO",
                "ERROR/SESIONEXPIRADA",
                "ACCESS/INICIARRECUPERACION",
                 "ACCESS/RECUPERACION",
                "ACCESS/RECUPERACIONPASS",
                "ACCESS/KEYTOKEN",

            };
            var action = string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper(), filterContext.ActionDescriptor.ActionName.ToUpper());
            //if ((isEncrypt) && !((filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper().Equals("ACCOUNT")) && (filterContext.ActionDescriptor.ActionName.ToUpper().Equals("AUTENTICADO") || filterContext.ActionDescriptor.ActionName.ToUpper().Equals("LOGIN"))))
            if ((isEncrypt) && !actionExceptions.Contains(action))
            {
                //Valida si contraseña logeada no es la actual               
                if (sisSesion.Usuario != null)
                {
                    usuarioRepository _usuarioRepository = new usuarioRepository();
                    usuario datUsuario = _usuarioRepository.Select_usuario(new usuario { ID_USUARIO = int.Parse(sisSesion.CodUsuario) });
                    if (datUsuario != null)
                    {
                        usuario datSesion = (usuario)sisSesion.Usuario;
                        if (datUsuario.CONTRASENIA != datSesion.CONTRASENIA)
                        {
                            var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                            var response = filterContext.HttpContext.Response;
                            response.Redirect(url.Action("SesionExpirada", "Error"));
                            filterContext.Result = new EmptyResult();
                            base.OnActionExecuting(filterContext);
                        }
                    }
                }

                if (sisSesion.Usuario == null)
                {
                    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                    //    {
                    //        {"controller", "Error"},
                    //        {"action", "SesionExpirada"}
                    //    });
                    var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                    var response = filterContext.HttpContext.Response;
                    response.Redirect(url.Action("SesionExpirada", "Error"));
                    filterContext.Result = new EmptyResult();
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    //Se valida si requiere TOKEN de acceso


                    var tokeRequired = filterContext.ActionDescriptor.IsDefined(typeof(HelperTokenRequiredAttribute), true);
                    if (tokeRequired)
                    {
                        #region  VALIDA TOKEN
                        var request = filterContext.HttpContext.Request;
                        var response = filterContext.HttpContext.Response;
                        //var session = filterContext.HttpContext.Session;

                        var tokenValid = false;
                        var tokenHeader = request.Headers["Authorization"];

                        if (tokenHeader != null)
                        {
                            //SE VALIDA EL TOKEN
                            var tokenEncrypt = tokenHeader.Substring(7, tokenHeader.Length - 7);
                            var tokenDecryptJson = HelperAES.DecryptStringAES(tokenEncrypt, sisVariable.IsEncrypt_Clave, true);

                            var tokenCached = MemoryCache.Default;
                            var tokenValue = tokenCached[tokenDecryptJson];

                            if (tokenValue != null)
                            {
                                tokenValid = true;
                                MemoryCache.Default.Remove(tokenDecryptJson);

                            }

                        }

                        //SI EL TOKEN NO ES VÁLIDO RETORNAMOS UN ERROR
                        if (!tokenValid)
                        {
                            
                            
                            var url = new UrlHelper(request.RequestContext);
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            response.Redirect(url.Action("NoAutorizado", "Error", new { area = "" }));
                            filterContext.Result = new EmptyResult();
                            
                            base.OnActionExecuting(filterContext);
                        }
                        #endregion
                    }


                    //2. Si la accion tiene parametros
                    #region PARAMETROS

                    if (filterContext.ActionParameters.Count > 0)
                    {
                        //3.Se obtiene el valor del parametro encriptado
                        string valueParameter;

                        var parametros = filterContext.Controller.ValueProvider.GetValue("filter");
                        if (parametros == null)
                        {
                            var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                            var response = filterContext.HttpContext.Response;
                            response.Redirect(url.Action("NoAutorizado", "Error"));
                            filterContext.Result = new EmptyResult();
                            base.OnActionExecuting(filterContext);

                        }
                        else
                        {
                            if (filterContext.Controller.ValueProvider.GetValue("filter").RawValue.GetType().Name.ToUpper().Equals("STRING[]"))
                                valueParameter = ((string[])filterContext.Controller.ValueProvider.GetValue("filter").RawValue)[0];
                            else
                                valueParameter = filterContext.Controller.ValueProvider.GetValue("filter").RawValue.ToString();

                            if (!string.IsNullOrWhiteSpace(valueParameter))
                            {
                                //4.Se desencripta el valor
                                string valueDecrypt = HelperAES.DecryptStringAES(valueParameter, sisVariable.IsEncrypt_Clave, true);

                                //5.Se verifica si tiene el formato JSON
                                if (ValidateJson(valueDecrypt))
                                {

                                    //6.Se convierte a JSON
                                    var oJson = JObject.Parse(valueDecrypt);

                                    //7. Se obtiene el 1er parametro del Action
                                    var actionParameters = filterContext.ActionDescriptor.GetParameters();
                                    foreach (var args in actionParameters)
                                    {
                                        //8.Se verifica si existe el parametro de la action en el objeto JSON
                                        var tag = oJson[args.ParameterName];

                                        if (tag != null)
                                        {

                                            //9. Verifica si el parametro es un objeto
                                            if (tag.HasValues)
                                            {
                                                //9.1 Se convierten al tipo del parametro de la action
                                                var settingsJson = new JsonSerializerSettings()
                                                {
                                                    NullValueHandling = NullValueHandling.Ignore,
                                                    DateFormatString = "dd/MM/yyyy"
                                                };
                                                var argValue = JsonConvert.DeserializeObject(tag.ToString(), args.ParameterType, settingsJson);
                                                filterContext.ActionParameters[args.ParameterName] = argValue;

                                            }
                                            else
                                            {
                                                //9.2 Se convierten al tipo del parametro de la action
                                                if (args.ParameterType != null)
                                                {
                                                    var argValue = Convert.ChangeType(tag.ToString(), args.ParameterType);
                                                    filterContext.ActionParameters[args.ParameterName] = argValue;
                                                }
                                                else
                                                {
                                                    //9.3 Se convierten al tipo del parametro de la action
                                                    var argValue = tag.Value<string>();
                                                    filterContext.ActionParameters[args.ParameterName] = argValue;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //Intenta deserializar el obj JSON
                                            var argValue = JsonConvert.DeserializeObject(oJson.ToString(), args.ParameterType);
                                            filterContext.ActionParameters[args.ParameterName] = argValue;

                                        }
                                    }


                                }

                            }
                        }

                        
                    }
                    #endregion
                    //FIN parametros
                }


            }





        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var isEncrypt = sisVariable.IsEncrypt;
            if (isEncrypt)
            {
                if (filterContext.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    var returnType = filterContext.Result.GetType();
                    if (returnType == typeof(JsonResult))
                    {
                        var result = filterContext.Result as JsonResult;
                        var dataJson = JsonConvert.SerializeObject(result.Data);
                        var dataEncrypt = HelperAES.EncryptStringAES(dataJson, sisVariable.IsEncrypt_Clave, true);
                        result.Data = dataEncrypt;
                    }

                }
            }
        }
        public static bool ValidateJson(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                var msjError = ex.Message;
                return false;
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
}