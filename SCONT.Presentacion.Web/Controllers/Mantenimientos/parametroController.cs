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

namespace SCONT.Presentacion.Web.Controllers.Mantenimientos
{
    public class parametroController : MEDBaseController
    {
        private Iparametro_generalService _parametro_GeneralService;
        private Itabla_generalService _tabla_generalService;

        public parametroController(Iparametro_generalService parametro_GeneralService, Itabla_generalService tabla_generalService)
        {
            _parametro_GeneralService = parametro_GeneralService;
            _tabla_generalService = tabla_generalService;
        }

        [Route("parametro/index")]
        public ActionResult Index()
        {
            ViewBag.Anio_Actual = sisFuncion.GetFechaServidor().Value.Year;
            return View("~/Views/Mantenimientos/Parametro.cshtml");
        }

        [HttpPost]
        [Route("parametro/GetBandeja")]
        public JsonResult GetBandeja(Parametro dato)
        {
            return Json(_parametro_GeneralService.SelectPaginated_parametro_general(dato));
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("parametro/UpdParametro")]
        public JsonResult UpdParametro(parametro_general dato)
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datValidaciones = Validaciones(dato);
            if (datValidaciones.Dato == false)
            {
                datRetorno.Dato = null;
                datRetorno.Msg = datValidaciones.Msg;
                return Json(datRetorno);
            }
            datRetorno = _parametro_GeneralService.Update_parametro_general(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [Route("parametro/GetParametro")]
        public JsonResult GetParametro(parametro_general dato)
        {
            DatoRetorno<parametro_general> datRetorno = _parametro_GeneralService.Select_parametro_general(dato);
            return Json(datRetorno);
        }

        //Validaciones
        public DatoRetorno<bool> Validaciones(parametro_general dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = true;
            datRetorno.Msg = "";

            if (string.IsNullOrWhiteSpace(dato.NOMBRE))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe ingresar un valor en el campo NOMBRE";
                return datRetorno;
            }
            if (string.IsNullOrWhiteSpace(dato.VALOR))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Debe ingresar un valor en el campo VALOR";
                return datRetorno;
            }
            return datRetorno;
        }

    }
}