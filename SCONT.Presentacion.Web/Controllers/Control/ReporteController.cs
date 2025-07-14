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
using System.Data;

namespace SCONT.Presentacion.Web.Controllers.Control
{
    public class ReporteController : MEDBaseController
    {
        private Iparametro_generalService _parametro_GeneralService;
        private Itabla_generalService _tabla_generalService;
        private ImarcacionService _marcacionService;

        public ReporteController(Iparametro_generalService parametro_GeneralService, Itabla_generalService tabla_generalService, ImarcacionService marcacionService)
        {
            _parametro_GeneralService = parametro_GeneralService;
            _tabla_generalService = tabla_generalService;
            _marcacionService = marcacionService;
        }

        [Route("reporte/index")]
        public ActionResult Index()
        {
            usuario datUsuario = (usuario)sisSesion.Usuario;
            ViewBag.Rol = datUsuario.ID_ROL;
            ViewBag.Anio_Actual = sisFuncion.GetFechaServidor().Value.Year;
            ViewBag.Fecha_Actual = sisFuncion.GetFechaServidor().Value.ToString("dd/MM/yyyy");
            ViewBag.Fecha_Inicio = "01/" + sisFuncion.GetFechaServidor().Value.ToString("MM/yyyy");
            List<tabla_general> lisReporte = new List<tabla_general>();
            lisReporte.Add(new tabla_general { CODIGO = "1", NOMBRE = "REPORTE DE HORAS TRABAJADAS" });

            ViewBag.lisTIPO_REPORTE = (SelectList)HelperConvert.SelectListAddItem(lisReporte, "CODIGO", "NOMBRE", "", "(SELECCIONAR)");
          
            return View("~/Views/Control/Reporte.cshtml");
        }

        [HttpPost]
        [Route("reporte/GenerarReporte")]
        public JsonResult GenerarReporte(Parametro dato)
        {
            DatoRetorno<string> datRetorno = new DatoRetorno<string>();
            DataTable lisData = new DataTable();
            switch (dato.PARAMETRO1)
            {
                case "1":
                    lisData = _marcacionService.SelectAll_reporte_horas_trabajadas_toExport(dato).Dato;
                    if (lisData != null)
                    {
                        sisSesion.Reporte_Parametro = dato;
                        sisSesion.Lista_Reporte = lisData;
                        datRetorno.Dato = "reporte_horas_trabajadas";
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
            if (lisData.Rows.Count <= 0)
            {
                datRetorno.Dato = "";
                datRetorno.Msg = "No existen registros para generar el reporte";
            }

            return Json(datRetorno);
        }

    }
}