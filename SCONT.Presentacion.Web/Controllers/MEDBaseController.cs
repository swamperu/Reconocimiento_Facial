using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web.Mvc;
using SCONT.Infraestructura.Transversal;
using MED.Security.API.AuthorizationService;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Dominio.Entidades;
using SCONT.Servicios.Externos;
using System.Collections.Generic;
using SCONT.Aplicacion.Servicios;

namespace SCONT.Presentacion.Web.Controllers
{
    public class MEDBaseController : Controller
    {
        public MEDBaseController()
        {
            if (sisSesion.Usuario != null)
            {
                RolResponse rolPrincipal = (RolResponse)sisSesion.RolPrincipal;
                usuario datUsuario = (usuario)sisSesion.Usuario;
                string ultimoInicioSesion = "";
                if (datUsuario.ULTIMO_INGRESO.HasValue) ultimoInicioSesion = datUsuario.ULTIMO_INGRESO.Value.ToShortDateString();


                ViewBag.MED_ROL_PRINCIPAL =datUsuario.NOMBRE_ROL;
                ViewBag.MED_NOMBRE_USUARIO = datUsuario.NOMBRES + " " + datUsuario.APELLIDO_PATERNO + " " + datUsuario.APELLIDO_MATERNO;
                ViewBag.MED_USUARIO = datUsuario.USUARIO;
                ViewBag.MED_LISTADO_ROLES = sisSesion.Roles;
                ViewBag.MED_LISTADO_MENUS = sisSesion.Menus;
                ViewBag.MED_ENCRYPT = (sisVariable.IsEncrypt ? "S" : "N");
                ViewBag.MED_ENCRYPT_CLAVE = sisVariable.IsEncrypt_Clave;
                ViewBag.MED_ULTIMA_SESION = ultimoInicioSesion;
                ViewBag.MED_Siscont_VERSION = sisVariable.Siscont_Version;
                ViewBag.MED_VERSION_JS = sisFuncion.GetFechaServidor().Value.ToString("yyyyMMddHHmmss");
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext context)
        {
            if (sisSesion.CodUsuario != null && sisSesion.CodUsuario != "")
            {
                if (context.HttpContext.Request.CurrentExecutionFilePath.ToUpper().Contains("/HOME/INDEX") == false) {
                    if (sisSesion.FormularioValido(context.HttpContext.Request.CurrentExecutionFilePath) == false)
                    {
                        sisSesion.RedirectToAccesoNoAutorizado();
                        return;
                    }
                }
            }
            else
            {
                RedirectToAction("Presentacion", "Account");
            }
            base.OnActionExecuted(context);
        }


        public string GetRecurso(string url)
        {
            List<MenuItemResponse> listaOpciones = (List<MenuItemResponse>)sisSesion.Menus;
            List<MenuItemResponse> listaRecursos = (from x in listaOpciones where x.NivelMenu.Equals(2) && url.ToUpper().Contains(x.UrlMenu.ToUpper()) select x).ToList();
            if(listaRecursos.Count > 0){
                return listaRecursos[0].Codigo;
            }
            else
            {
                return "";
            }
            
        }

        [HttpPost]
        public async Task<JsonResult> FileUpload()
        {
            var retorno = new DatoRetorno<string>() { Success = true };
            try
            {
                if (Request.Files.Count > 0)
                {
                    foreach (string fileName in Request.Files)
                    {
                        var fileContent = Request.Files[fileName];

                        //Se formatea el nombre a guardar
                        if (fileName.IndexOf('@') >= 0)
                            retorno.Dato = string.Format("anonimo_{0}{1}", DateTime.Now.ToString("ddMMyyyy_HHmmss"), Path.GetExtension(fileContent.FileName));
                        else
                        {
                            retorno.Dato = string.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("ddMMyyyy_HHmmss"), Path.GetExtension(fileContent.FileName));
                        }

                        if (fileContent != null && fileContent.ContentLength > 0)
                        {

                            var urlFileSystemTemp = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SCONT.UrlFileSystemTemp"])
                                       ? Server.MapPath("~/Temp")
                                       : ConfigurationManager.AppSettings["SCONT.UrlFileSystemTemp"];

                            if (!Directory.Exists(urlFileSystemTemp))
                                Directory.CreateDirectory(urlFileSystemTemp);

                            FileUploadSecurityPath(urlFileSystemTemp);

                            var path = Path.Combine(urlFileSystemTemp, retorno.Dato);//Temporal
                            fileContent.SaveAs(path);
                        }
                    }
                }
                else
                {
                    retorno.Success = false;
                }

            }
            catch (Exception ex)
            {
                var msjError = ex.Message;
                retorno.Success = false;
            }

            return Json(retorno);
        }

        [HttpPost]
        public void FileUploadTempRemove(Adjunto dato)
        {
            //Se elimina del repositorio temporal
            if (!string.IsNullOrEmpty(dato.NOMBRE_ARCHIVO_ALMACENADO))
            {

                var urlFileSystemTemp = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SCONT.UrlFileSystemTemp"])
                                        ? Server.MapPath("~/Temp")
                                        : ConfigurationManager.AppSettings["SCONT.UrlFileSystemTemp"];

                FileUploadSecurityPath(urlFileSystemTemp);
                var pathFileTemp = Path.Combine(urlFileSystemTemp, dato.NOMBRE_ARCHIVO_ALMACENADO);
                if (System.IO.File.Exists(pathFileTemp))
                {
                    System.IO.File.Delete(pathFileTemp);
                }
            }


        }
        [HttpPost]
        public DatoRetorno<string> FileUploadSave(Adjunto dato)
        {
            var result = new DatoRetorno<string>() { Success = false };
            try
            {
                var urlFileSystemTemp = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SCONT.UrlFileSystemTemp"])
                                        ? Server.MapPath("~/Temp")
                                        : ConfigurationManager.AppSettings["SCONT.UrlFileSystemTemp"];
                if (Directory.Exists(urlFileSystemTemp))
                {

                    var sourceFile = Path.Combine(urlFileSystemTemp, dato.NOMBRE_ARCHIVO_ALMACENADO);

                    var urlFileSystem = ConfigurationManager.AppSettings["SCONT.UrlFileSystem"];
                    if (!Directory.Exists(urlFileSystem))
                        Directory.CreateDirectory(urlFileSystem);

                    var destinyFile = string.Empty;
                    var directory = string.Empty;
                    var arrFile = dato.NOMBRE_ARCHIVO_ALMACENADO.Split('_');

                    //Verfica si tiene caperta definida
                    directory = Path.Combine(urlFileSystem, arrFile[0] == "anonimo" ? "Anonimos" : arrFile[0]);

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    //Habilitar acceso ruta final
                    FileUploadSecurityPath(directory);
                    destinyFile = Path.Combine(directory, dato.NOMBRE_ARCHIVO_ALMACENADO);
                    System.IO.File.Copy(sourceFile, destinyFile, true);

                    result.Success = true;
                }

            }
            catch (Exception ex)
            {
                var msjError = ex.Message;
                result.Success = false;
            }
            return result;

        }
        public void FileUploadSecurityPath(string fileName)
        {
            var enableSecurity = false;
            var usuario = ConfigurationManager.AppSettings["SCONT.UrlFileSystem.Credentials"];
            var dInfo = new DirectoryInfo(fileName);
            var security = dInfo.GetAccessControl();

            if (security != null)
            {

                var rules = security.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));

                foreach (FileSystemAccessRule rule in rules)
                {
                    if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                        continue;

                    if (rule.AccessControlType == AccessControlType.Allow)
                        enableSecurity = false;
                    else if (rule.AccessControlType == AccessControlType.Deny)
                        enableSecurity = true;
                }

            }


            if (enableSecurity)
            {
                security.AddAccessRule(new FileSystemAccessRule(usuario, FileSystemRights.FullControl, AccessControlType.Allow));
                dInfo.SetAccessControl(security);
            }


        }

        //public respuestaReniec BuscarPersonaReniecPorDNI(string DNI)
        //{
        //    try
        //    {
        //        string NAME_USER = ConfigurationManager.AppSettings["EP_Usuario"];
        //        string PASSWORD = ConfigurationManager.AppSettings["EP_Clave"];

        //        return new ReniecServiceClient().buscarDNICascada(NAME_USER, PASSWORD, Request.ServerVariables["REMOTE_ADDR"], DNI);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //}

        public List<MenuItemResponse> GetRecursosByCodigo(string codigo)
        {
            List<MenuItemResponse> listaOpciones = null;
            List<MenuItemResponse> listaRecursos = (List<MenuItemResponse>)sisSesion.Menus;
            MenuItemResponse itemRecurso = (from x in listaRecursos where x.NivelMenu.Equals(2) && x.Codigo.Equals(codigo) select x).SingleOrDefault();
            
            if (itemRecurso != null){
                listaOpciones = (from x in listaRecursos where x.NivelMenu.Equals(3) && x.IdMenuPadre.Equals(itemRecurso.IdMenu) orderby x.Codigo select x).ToList();
            }
            
            return listaOpciones;
        }

    }
}