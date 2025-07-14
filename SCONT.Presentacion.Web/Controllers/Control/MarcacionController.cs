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
    public class marcacionController : MEDBaseController
    {
        private Iparametro_generalService _parametro_GeneralService;
        private Itabla_generalService _tabla_generalService;
        private ImarcacionService _marcacionService;
        private IubigeoService _ubigeoService;

        public marcacionController(Iparametro_generalService parametro_GeneralService, Itabla_generalService tabla_generalService, ImarcacionService marcacionService, IubigeoService ubigeoService)
        {
            _parametro_GeneralService = parametro_GeneralService;
            _tabla_generalService = tabla_generalService;
            _marcacionService = marcacionService;
            _ubigeoService = ubigeoService;
        }

        [Route("marcacion/index")]
        public ActionResult Index()
        {
            ViewBag.Anio_Actual = sisFuncion.GetFechaServidor().Value.Year;
            ViewBag.lisVacio = (SelectList)HelperConvert.SelectListAddItem(new List<JQCombo>(), "Id", "Valor", "", "(SELECCIONAR)", "");
            ViewBag.Fecha_Actual = sisFuncion.GetFechaServidor().Value.ToString("dd/MM/yyyy");
            ViewBag.Fecha_Inicio = "01" + sisFuncion.GetFechaServidor().Value.ToString("/MM/yyyy");
            ViewBag.lisTIPO = (SelectList)HelperConvert.SelectListAddItem(_tabla_generalService.SelectAll_tabla_general_by_grupo(sisConstante.Tabla_General_Tipo_Marcacion).Lista, "CODIGO", "NOMBRE", "", "(SELECCIONAR)");
            return View("~/Views/Control/Marcacion.cshtml");
        }

        [HttpPost]
        [Route("marcacion/GetBandeja")]
        public JsonResult GetBandeja(Parametro dato)
        {
            return Json(_marcacionService.SelectPaginated_marcacion(dato));
        }

        //[HttpPost]
        //[Route("marcacion/GetProvincias")]
        //public JsonResult GetProvincias(ubigeo dato)
        //{
        //    DatoRetorno<JQCombo> datRetorno = new DatoRetorno<JQCombo> { Lista = sisFuncion.ListToJQCombo("PRV_CODIGO", "PRV_NOMBRE", _ubigeoService.SelectAll_provincia(dato).Lista) };
        //    return Json(datRetorno);
        //}

        //[HttpPost]
        //[Route("marcacion/GetDistritos")]
        //public JsonResult GetDistritos(ubigeo dato)
        //{
        //    DatoRetorno<JQCombo> datRetorno = new DatoRetorno<JQCombo> { Lista = sisFuncion.ListToJQCombo("DIS_CODIGO", "DIS_NOMBRE", _ubigeoService.SelectAll_distrito(dato).Lista) };
        //    return Json(datRetorno);
        //}

        [HttpPost]
        [HelperTokenRequired]
        [Route("marcacion/InsRegistro")]
        public JsonResult InsRegistro(marcacion dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            dato.USUARIO_CREACION = sisSesion.CodUsuario;
            if (dato.FECHA_TEXTO != "") { dato.FECHA_HORA = Convert.ToDateTime(dato.FECHA_TEXTO + " " + dato.HORA_TEXTO); }
            datRetorno = _marcacionService.Insert_marcacion(dato);
            return Json(datRetorno);
        }

        //[HttpPost]
        //[HelperTokenRequired]
        //[Route("marcacion/UpdRegistro")]
        //public JsonResult UpdRegistro(marcacion dato)
        //{
        //    DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
        //    dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
        //    if (dato.FECHA_NACIMIENTO_TEXTO != "") { dato.FECHA_NACIMIENTO = Convert.ToDateTime(dato.FECHA_NACIMIENTO_TEXTO); }
        //    if (dato.FECHA_INGRESO_TEXTO != "") { dato.FECHA_INGRESO = Convert.ToDateTime(dato.FECHA_INGRESO_TEXTO); }
        //    datRetorno = _marcacionService.Update_marcacion(dato);
        //    return Json(datRetorno);
        //}

        //[HttpPost]
        //[Route("marcacion/GetRegistro")]
        //public JsonResult GetRegistro(marcacion dato)
        //{
        //    DatoRetorno<marcacion> datRetorno = _marcacionService.Select_marcacion(dato);
        //    return Json(datRetorno);
        //}

        [HttpPost]
        [HelperTokenRequired]
        [Route("marcacion/DelRegistro")]
        public JsonResult DelRegistro(marcacion dato)
        {
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datRetorno = _marcacionService.Delete_marcacion(dato);
            return Json(datRetorno);
        }
    }
}