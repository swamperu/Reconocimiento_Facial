using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using SCONT.Presentacion.Web.Helpers;
using MED.Security.API.Authorization;
using SCONT.Aplicacion.Contratos;
using SCONT.Aplicacion.Servicios;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Infraestructura.Transversal;
using SCONT.Dominio.Entidades.Parametrica;
using SCONT.Dominio.Entidades;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;

namespace SCONT.Presentacion.Web.Controllers.Mantenimientos
{
    public class usuarioController : MEDBaseController
    {
        private Iparametro_generalService _parametro_GeneralService;
        private Itabla_generalService _tabla_generalService;
        private IusuarioService _usuarioService;
        
        public usuarioController(Iparametro_generalService parametro_GeneralService, Itabla_generalService tabla_generalService, IusuarioService usuarioService)
        {
            _parametro_GeneralService = parametro_GeneralService;
            _tabla_generalService = tabla_generalService;
            _usuarioService = usuarioService;
        }

        [Route("usuario/index")]
        public ActionResult Index()
        {
            ViewBag.Anio_Actual = sisFuncion.GetFechaServidor().Value.Year;
            ViewBag.lisFilROL_USUARIO = (SelectList)HelperConvert.SelectListAddItem(_tabla_generalService.SelectAll_tabla_general_by_grupo(sisConstante.Tabla_General_Rol_Usuario).Lista, "CODIGO", "NOMBRE", "", "(TODOS)");
            ViewBag.lisROL_USUARIO = (SelectList)HelperConvert.SelectListAddItem(_tabla_generalService.SelectAll_tabla_general_by_grupo(sisConstante.Tabla_General_Rol_Usuario).Lista, "CODIGO", "NOMBRE", "", "(SELECCIONAR)");
            return View("~/Views/Mantenimientos/usuario.cshtml");
        }

        [HttpPost]
        [Route("usuario/GetBandeja")]
        public JsonResult GetBandeja(Parametro dato)
        {
            return Json(_usuarioService.SelectPaginated_usuario(dato));
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("usuario/Insusuario")]
        public JsonResult Insusuario(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            dato.USUARIO_CREACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datValidacion = new DatoRetorno<bool>();
            datValidacion = ValidacionesUsuario(dato);
            if (datValidacion.Dato == false)
            {
                datRetorno = datValidacion;
                return Json(datRetorno);
            }
           datRetorno = _usuarioService.Insert_usuario(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("usuario/Updusuario")]
        public JsonResult Updusuario(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datValidacion = new DatoRetorno<bool>();
            datValidacion = ValidacionesUsuario(dato);
            if (datValidacion.Dato == false)
            {
                datRetorno = datValidacion;
                return Json(datRetorno);
            }
            datRetorno = _usuarioService.Update_usuario(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [Route("usuario/Getusuario")]
        public JsonResult Getusuario(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = _usuarioService.Select_usuario(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("usuario/Delusuario")]
        public JsonResult Delusuario(usuario dato)
        {
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datRetorno = _usuarioService.Delete_usuario(dato);
            return Json(datRetorno);
        }
        
        [HttpPost]
        [Route("usuario/GenerarReporte")]
        public JsonResult GenerarReporte(Parametro dato)
        {
            DatoRetorno<string> datRetorno = new DatoRetorno<string>();
            DataTable lisData = new DataTable();
            dato.FECHA_SERVIDOR = sisFuncion.GetFechaServidor();
            switch (dato.PARAMETRO1)
            {
                case "1":
                    lisData = (DataTable)_usuarioService.SelectAll_reporte_usuario_logistica_toExport().Dato;
                    if (lisData != null)
                    {
                        sisSesion.Reporte_Parametro = dato;
                        sisSesion.Lista_Reporte = lisData;
                        datRetorno.Dato = "reporte_usuario_logistica";
                        datRetorno.Msg = "Generando Reporte";
                    }
                    break;
                case "2":
                    lisData = (DataTable)_usuarioService.SelectAll_reporte_usuario_oficina_usuaria_toExport().Dato;
                    if (lisData != null)
                    {
                        sisSesion.Reporte_Parametro = dato;
                        sisSesion.Lista_Reporte = lisData;
                        datRetorno.Dato = "reporte_usuario_oficina_usuaria";
                        datRetorno.Msg = "Generando Reporte";
                    }
                    break;
                default:
                    datRetorno.Dato = "";
                    datRetorno.Msg = "No existe el reporte seleccionado";
                    break;
            }

            if (lisData == null)
            {
                datRetorno.Dato = "";
                datRetorno.Msg = "No existen registros para generar el reporte";
            }

            return Json(datRetorno);
        }

        //Validaciones
        public DatoRetorno<bool> ValidacionesUsuario(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = true;
            datRetorno.Msg = "";
            //if (string.IsNullOrWhiteSpace(dato.DNI))
            //{
            //    datRetorno.Dato = false;
            //    datRetorno.Msg = "Debe ingresar un valor en el campo DNI";
            //    return datRetorno;
            //}
            //if (dato.DNI.Length != 8)
            //{
            //    datRetorno.Dato = false;
            //    datRetorno.Msg = "El campo DNI debe tener como mínimo 8 dígitos";
            //    return datRetorno;
            //}
            Regex numeros = new Regex(@"^[\d]+$");
            //if (!numeros.IsMatch(dato.DNI))
            //{
            //    datRetorno.Dato = false;
            //    datRetorno.Msg = "El campo DNI debe tener solo valores numéricos";
            //    return datRetorno;
            //}
            if (string.IsNullOrWhiteSpace(dato.NOMBRES))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe ingresar un valor en el campo NOMBRES";
                return datRetorno;
            }
            if (string.IsNullOrWhiteSpace(dato.APELLIDO_PATERNO) && string.IsNullOrWhiteSpace(dato.APELLIDO_MATERNO))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe ingresar un valor en el campo APELLIDO PATERNO o APELLIDO MATERNO";
                return datRetorno;
            }
            if (string.IsNullOrWhiteSpace(dato.ID_ROL))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe seleccionar un valor en el campo ROL";
                return datRetorno;
            }
            if (string.IsNullOrWhiteSpace(dato.USUARIO))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe ingresar un valor en el campo USUARIO";
                return datRetorno;
            }
            if (string.IsNullOrWhiteSpace(dato.USUARIO))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe ingresar un valor en el campo CONTRASEÑA";
                return datRetorno;
            }
            if(dato.CORREO != "")
            {
                if (sisFuncion.IsEmail(dato.CORREO) == false)
                {
                    datRetorno.Dato = false;
                    datRetorno.Msg = "Debe ingresar un valor correcto en el campo CORREO ELECTRÓNICO";
                    return datRetorno;
                }
            }
            
            return datRetorno;
        }

    }
}