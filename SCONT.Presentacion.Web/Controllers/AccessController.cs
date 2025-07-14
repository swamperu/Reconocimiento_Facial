using SCONT.Aplicacion.Contratos;
using SCONT.Dominio.Entidades;
using SCONT.Presentacion.Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SCONT.Servicios.Externos;
using SCONT.Infraestructura.Transversal;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Aplicacion.Servicios;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Caching;

namespace SCONT.Presentacion.Web.Controllers
{
    public class AccessController : Controller
    {
        private IrecursoService _recursoService;
        private IusuarioService _usuarioService;

        public AccessController(IrecursoService recursoService,
                                 IusuarioService usuarioService)
        {
            _recursoService = recursoService;
            _usuarioService = usuarioService;
        }
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IniciarRecuperacion()
        {
            ViewBag.EncryptToken = System.Configuration.ConfigurationManager.AppSettings["SCONT.Encrypt.Clave"];
            RecoveryViewModel model = new RecoveryViewModel();
            return View(model);
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
        public ActionResult IniciarRecuperacion(string parametros) //(RecoveryViewModel model)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            string token = System.Configuration.ConfigurationManager.AppSettings["SCONT.Encrypt.Clave"];
            string valueParameter = parametros;

            if (!string.IsNullOrWhiteSpace(valueParameter))
            {
                //4.Se desencripta el valor
                string valueDecrypt = HelperAES.DecryptStringAES(valueParameter, token, true);

                //5.Se verifica si tiene el formato JSON
                if (ValidateJson(valueDecrypt))
                {

                    //6.Se convierte a JSON
                    var oJson = JObject.Parse(valueDecrypt);
                    var usuario = JsonConvert.DeserializeObject<RecoveryViewModel>(oJson.ToString());

                    if (string.IsNullOrWhiteSpace(usuario.Usuario))
                    {
                        datRetorno.Msg = "Debe ingresar el USUARIO.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }

                    if (string.IsNullOrWhiteSpace(usuario.Correo))
                    {
                        datRetorno.Msg = "Debe ingresar el CORREO ELECTRÓNICO.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }

                    string token_rec = GetSHA256(Guid.NewGuid().ToString());
                    usuario datUsuario = _usuarioService.Select_usuario_recuperacion(new usuario { USUARIO = usuario.Usuario, CORREO = usuario.Correo }).Dato;
                    if (datUsuario == null)
                    {
                        datRetorno.Msg = "El USUARIO y CORREO ingresado no existen";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }
                    bool ind = _usuarioService.Update_usuario_recuperacion(new usuario { ID_USUARIO = datUsuario.ID_USUARIO, TOKEN_RECUPERACION = token_rec }).Dato;
                    if (ind)
                    {
                        string dominioPath = HttpContext.Request.Url.ToString().ToUpper();
                        string dominio = dominioPath.Replace("ACCESS/INICIARRECUPERACION", "");

                        //var url = sisVariable.Siscont_Dominio_Url + "Access/Recuperacion?token=" + token_rec;
                        var url = dominio + (dominio.Last().ToString() == "/" ? "" : "/") + "Access/Recuperacion?token=" + token_rec;

                        //CorreoServiceClient srvCorreo = new CorreoServiceClient();
                        //var send = srvCorreo.EnviarCorreoHtml(datUsuario.CORREO, "", "", "", "", "Recuperación de contraseña", "<p>Estimado usuario<p>Para continuar con el proceso de Recuperación de Contraseña deberá ingresar al siguiente link:<br><a href='" + url + "'>" + url + "</a>", "html");
                        var send = sisFuncion.Enviar_Correo_Html(datUsuario.CORREO, "Recuperación de contraseña", "<p>Estimado usuario<p>Para continuar con el proceso de Recuperación de Contraseña deberá ingresar al siguiente link:<br><a href='" + url + "'>" + url + "</a>", "");
                        if (!send)
                        {
                            datRetorno.Msg = "Hubo un error al enviar el correo de recuperación";
                            datRetorno.Dato = false;
                            return Json(datRetorno);
                        }
                        else
                        {
                            datRetorno.Msg = "Para completar la recuperación, ingrese al link enviado al correo electrónico brindado";
                            datRetorno.Dato = true;
                            return Json(datRetorno);
                        }
                    }
                    else
                    {
                        datRetorno.Msg = "Hubo un error al generar el TOKEN de recuperación";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }
                }

            }

            return Json(datRetorno);

        }

        [HttpGet]
        public ActionResult Recuperacion(string token)
        {
            ViewBag.EncryptToken = System.Configuration.ConfigurationManager.AppSettings["SCONT.Encrypt.Clave"];
            RecoveryPasswordViewModel model = new RecoveryPasswordViewModel();
            ViewBag.TokenPass = token;
            if (token == null || token.Trim().Equals(""))
            {
                ViewBag.Error = "El TOKEN de recuperación no existe";
                return View();
            }
            usuario datUsuario = _usuarioService.Select_usuario_by_token(new usuario { TOKEN_RECUPERACION = token }).Dato;
            if (datUsuario == null)
            {
                ViewBag.Error = "El TOKEN de recuperación no existe o ha expirado";
                return View();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult RecuperacionPass(string parametros) //(RecoveryViewModel model)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            string token = System.Configuration.ConfigurationManager.AppSettings["SCONT.Encrypt.Clave"];
            string valueParameter = parametros;

            if (!string.IsNullOrWhiteSpace(valueParameter))
            {
                //4.Se desencripta el valor
                string valueDecrypt = HelperAES.DecryptStringAES(valueParameter, token, true);

                //5.Se verifica si tiene el formato JSON
                if (ValidateJson(valueDecrypt))
                {

                    //6.Se convierte a JSON
                    var oJson = JObject.Parse(valueDecrypt);
                    var usuario = JsonConvert.DeserializeObject<RecoveryPasswordViewModel>(oJson.ToString());

                    if (string.IsNullOrWhiteSpace(usuario.Password))
                    {
                        datRetorno.Msg = "Debe ingresar la NUEVA CONTRASEÑA.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }

                    if (string.IsNullOrWhiteSpace(usuario.Password))
                    {
                        datRetorno.Msg = "Vuelva a ingresar la NUEVA CONTRASEÑA.";
                        datRetorno.Dato = false;
                        return Json(datRetorno);
                    }

                    DatoRetorno<bool> datRerno = _usuarioService.Update_usuario_password_recuperacion(new usuario { TOKEN_RECUPERACION = usuario.token, NUEVA_CONTRASENIA = usuario.Password, CONFIRMAR_NUEVA_CONTRASENIA = usuario.Password2 });

                    if (datRerno.Dato == false)
                    {
                        datRetorno = datRerno;
                        return Json(datRetorno);
                    }
                    else
                    {
                        datRetorno.Msg = "Contraseña modificada con éxito, click en Iniciar Sesión para continuar";
                        datRetorno.Dato = true;
                        return Json(datRetorno);
                    }
                }

            }

            return Json(datRetorno);

        }

        [HttpPost]
        public JsonResult KeyToken()
        {
            var retorno = new DatoRetorno<string>() { Success = true };
            var token = Guid.NewGuid().ToString("N");
            var captchaCached = MemoryCache.Default;
            captchaCached.Add(token, true, DateTime.Now.AddSeconds(sisVariable.Token_time));

            retorno.Dato = token;

            return Json(retorno);
        }

        #region HELPERS
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        #endregion
    }
}