using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web.Security;
using SCONT.Presentacion.Web.Models;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Infraestructura.Transversal;
using SCONT.Aplicacion.Contratos;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Sistema;
using SCONT.Presentacion.Web.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MED.Security.API.Authentication;
using MED.Security.API.Authentication.Models;

namespace SCONT.Presentacion.Web.Controllers
{
    public class AccountController : Controller
    {
        private IrecursoService _recursoService;
        private IusuarioService _usuarioService;

        public AccountController(IrecursoService recursoService,IusuarioService usuarioService)
        {
            _recursoService = recursoService;
            _usuarioService = usuarioService;
        }


        public ActionResult Presentacion()
        {
            HttpBrowserCapabilitiesBase browser = Request.Browser;
            string type = browser.Type;
            string name = browser.Browser;
            string version = browser.Version;
            if ((name == "InternetExplorer" || name == "IE") && double.Parse(version) < 10)
            {
                return View("~/Views/Home/VistaCompatibilidad.cshtml");
            }
            var token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16).ToUpper();
            Session["__token"] = token;
            //ViewBag.EncryptToken = token;
            ViewBag.EncryptToken = System.Configuration.ConfigurationManager.AppSettings["SCONT.Encrypt.Clave"];
            if (Session["conteo"] == null)
            {
                Session["conteo"] = 0;
            }
            ViewBag.MED_VERSION_JS = sisFuncion.GetFechaServidor().Value.ToString("yyyyMMddHHmmss");
            string dominioPath = HttpContext.Request.Url.ToString().ToUpper();
            ViewBag.Dominio = dominioPath.Replace("ACCOUNT/PRESENTACION", "");
            return View("~/Views/Account/Presentacion.cshtml");
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string credenciales)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            string token = System.Configuration.ConfigurationManager.AppSettings["SCONT.Encrypt.Clave"];
            string valueParameter = credenciales;
            if (Session["conteo"] == null) Session["conteo"] = 0;
            Session["conteo"] = (int)Session["conteo"] + 1;
            if (!string.IsNullOrWhiteSpace(valueParameter))
            {
                //4.Se desencripta el valor
                string valueDecrypt = HelperAES.DecryptStringAES(valueParameter, token, true);

                //5.Se verifica si tiene el formato JSON
                if (ValidateJson(valueDecrypt))
                {

                    //6.Se convierte a JSON
                    var oJson = JObject.Parse(valueDecrypt);
                    var usuario = JsonConvert.DeserializeObject<UsuarioViewModel>(oJson.ToString());

                    //UsuarioViewModel usuario = new UsuarioViewModel();
                    if (usuario.NombreUsuario == null || usuario.NombreUsuario == "")
                    {
                        datRetorno.Msg = "Debe ingresar el usuario y la contraseña, verifique.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }

                    if (usuario.Contrasenia == null || usuario.Contrasenia == "")
                    {
                        datRetorno.Msg = "Debe ingresar el usuario y la contraseña, verifique.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }

                    if ((int)Session["conteo"] >= 4)
                    {
                        if (usuario.Captcha == null || usuario.Captcha == "")
                        {
                            datRetorno.Msg = "Debe ingresar el código captcha, verifique.";
                            datRetorno.Dato = false;
                            return Json(datRetorno);
                        }
                        if (usuario.Captcha != usuario.CaptchaValido)
                        {
                            datRetorno.Msg = " El código de imágen no concuerda. Ingrese captcha válido";
                            datRetorno.Dato = false;
                            return Json(datRetorno);
                        }
                    }
                    usuario datUsuario = new usuario();
                    datUsuario = _usuarioService.Select_usuario_login(new usuario { USUARIO = HelperAES.EncryptStringAES(usuario.NombreUsuario.ToUpper()), CONTRASENIA = HelperAES.EncryptStringAES(usuario.Contrasenia) }).Dato;

                    if (datUsuario == null)
                    {
                        datRetorno.Msg = "El usuario o contraseña son incorrectos, verifique.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }
                    if (datUsuario != null)
                    {
                        if (datUsuario.ESTADO != 0)
                        {
                            datRetorno.Msg = "El usuario no se encuentra ACTIVO, verifique.";
                            datRetorno.Dato = false;
                            return Json(datRetorno);
                        }
                    }

                    FormsAuthentication.SetAuthCookie(datUsuario.USUARIO, false);
                    var authTicket = new FormsAuthenticationTicket(1, datUsuario.USUARIO, DateTime.Now, DateTime.Now.AddMinutes(30), false, JsonConvert.SerializeObject(datUsuario));
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    authCookie.HttpOnly = true;
                    //HttpContext.Response.Cookies.Remove("ASP.NET_SessionId");
                    HttpContext.Response.Cookies.Add(authCookie);

                    sisSesion.Menus = _recursoService.SelectAll_recurso(new recurso { ID_ROL = datUsuario.ID_ROL }).Lista;
                    sisSesion.Usuario = datUsuario;
                    _usuarioService.Update_usuario_ingreso(datUsuario);
                    datRetorno.Msg = "";
                    datRetorno.Dato = true;
                    Session["conteo"] = 0;
                    return Json(datRetorno);

                }

            }

            return Json(datRetorno);

        }

        [HttpPost]
        public ActionResult LogOff()
        {
            var authentication = DependencyResolver.Current.GetService<IAuthentication>();
            FormsAuthentication.SignOut();
            var userSession = (usuario)sisSesion.Usuario;
            ViewBag.MED_VERSION_JS = sisFuncion.GetFechaServidor().Value.ToString("yyyyMMddHHmmss");
            string dominioPath = HttpContext.Request.Url.ToString().ToUpper();
            ViewBag.Dominio = dominioPath.Replace("ACCOUNT/PRESENTACION", "");
            if (userSession == null)
            {
                var count_ = Request.Cookies.Count;
                for (int i = 0; i < count_; i++)
                {
                    var cookie = new HttpCookie(Request.Cookies[i].Name);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.Value = string.Empty;
                    Response.Cookies.Add(cookie);
                }
                Session.Clear();
                Session.Abandon();
                return RedirectToAction("Presentacion", "Account");
            }
            LogOnModel model = new LogOnModel();
            model.CodigoSistema = sisVariable.Siscont_Codigo;
            model.UserName = userSession.USUARIO;

            //if (userSession != null) authentication.Logout(model);
            var count = Request.Cookies.Count;
            for (int i = 0; i < count; i++)
            {
                var cookie = new HttpCookie(Request.Cookies[i].Name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = string.Empty;
                Response.Cookies.Add(cookie);
            }
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Presentacion", "Account");
        }


        [HttpPost]
        public JsonResult UpdPassword(usuario dato)
        {
            dato.ID_USUARIO = int.Parse(sisSesion.CodUsuario);
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datRetorno = _usuarioService.Update_usuario_password(dato);
            return Json(datRetorno);
        }

    }
}