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
using System.Diagnostics;

namespace SCONT.Presentacion.Web.Controllers.Mantenimientos
{
    public class trabajadorController : MEDBaseController
    {
        private Iparametro_generalService _parametro_GeneralService;
        private Itabla_generalService _tabla_generalService;
        private ItrabajadorService _trabajadorService;
        private IubigeoService _ubigeoService;

        public trabajadorController(Iparametro_generalService parametro_GeneralService, Itabla_generalService tabla_generalService, ItrabajadorService trabajadorService, IubigeoService ubigeoService)
        {
            _parametro_GeneralService = parametro_GeneralService;
            _tabla_generalService = tabla_generalService;
            _trabajadorService = trabajadorService;
            _ubigeoService = ubigeoService;
        }

        [Route("trabajador/index")]
        public ActionResult Index()
        {
            ViewBag.Anio_Actual = sisFuncion.GetFechaServidor().Value.Year;
            ViewBag.lisGENERO = (SelectList)HelperConvert.SelectListAddItem(_tabla_generalService.SelectAll_tabla_general_by_grupo(sisConstante.Tabla_General_Genero).Lista, "CODIGO", "NOMBRE", "", "(SELECCIONAR)");
            ViewBag.lisDEPARTAMENTO = (SelectList)HelperConvert.SelectListAddItem(_ubigeoService.SelectAll_departamento().Lista, "DEP_CODIGO", "DEP_NOMBRE", "", "(SELECCIONAR)");
            ViewBag.lisHORARIO = (SelectList)HelperConvert.SelectListAddItem(_tabla_generalService.SelectAll_tabla_general_by_grupo(sisConstante.Tabla_General_Horario_Laboral).Lista, "CODIGO", "NOMBRE", "", "(SELECCIONAR)");
            ViewBag.lisVacio = (SelectList)HelperConvert.SelectListAddItem(new List<JQCombo>(), "Id", "Valor", "", "(SELECCIONAR)", "");

            return View("~/Views/Mantenimientos/Trabajador.cshtml");
        }

        [HttpPost]
        [Route("trabajador/GetBandeja")]
        public JsonResult GetBandeja(Parametro dato)
        {
            return Json(_trabajadorService.SelectPaginated_trabajador(dato));
        }

        [HttpPost]
        [Route("trabajador/GetProvincias")]
        public JsonResult GetProvincias(ubigeo dato)
        {
            DatoRetorno<JQCombo> datRetorno = new DatoRetorno<JQCombo> { Lista = sisFuncion.ListToJQCombo("PRV_CODIGO", "PRV_NOMBRE", _ubigeoService.SelectAll_provincia(dato).Lista) };
            return Json(datRetorno);
        }

        [HttpPost]
        [Route("trabajador/GetDistritos")]
        public JsonResult GetDistritos(ubigeo dato)
        {
            DatoRetorno<JQCombo> datRetorno = new DatoRetorno<JQCombo> { Lista = sisFuncion.ListToJQCombo("DIS_CODIGO", "DIS_NOMBRE", _ubigeoService.SelectAll_distrito(dato).Lista) };
            return Json(datRetorno);
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("trabajador/InsRegistro")]
        public JsonResult InsRegistro(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            dato.USUARIO_CREACION = sisSesion.CodUsuario;
            if(dato.FECHA_NACIMIENTO_TEXTO != "") { dato.FECHA_NACIMIENTO = Convert.ToDateTime(dato.FECHA_NACIMIENTO_TEXTO); }
            if (dato.FECHA_INGRESO_TEXTO != "") { dato.FECHA_INGRESO = Convert.ToDateTime(dato.FECHA_INGRESO_TEXTO); }
            datRetorno = _trabajadorService.Insert_trabajador(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("trabajador/UpdRegistro")]
        public JsonResult UpdRegistro(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            if (dato.FECHA_NACIMIENTO_TEXTO != "") { dato.FECHA_NACIMIENTO = Convert.ToDateTime(dato.FECHA_NACIMIENTO_TEXTO); }
            if (dato.FECHA_INGRESO_TEXTO != "") { dato.FECHA_INGRESO = Convert.ToDateTime(dato.FECHA_INGRESO_TEXTO); }
            datRetorno = _trabajadorService.Update_trabajador(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [Route("trabajador/GetRegistro")]
        public JsonResult GetRegistro(trabajador dato)
        {
            DatoRetorno<trabajador> datRetorno = _trabajadorService.Select_trabajador(dato);
            return Json(datRetorno);
        }


        [HttpPost]
        [Route("trabajador/GetLog")]
        public JsonResult GetLog(tabla_general dato)
        {
            DatoRetorno<tabla_general> datRetorno = _tabla_generalService.SelectAll_seguimiento_log(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [HelperTokenRequired]
        [Route("trabajador/DelRegistro")]
        public JsonResult DelRegistro(trabajador dato)
        {
            dato.USUARIO_MODIFICACION = sisSesion.CodUsuario;
            DatoRetorno<bool> datRetorno = _trabajadorService.Delete_trabajador(dato);
            return Json(datRetorno);
        }

        [HttpPost]
        [Route("trabajador/IniCaptura")]
        public JsonResult IniCaptura(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>(); ;
            try
            {
                string nombre_exe = "";
                datRetorno = _trabajadorService.Update_trabajador_captura(dato);
                if (datRetorno.Dato)
                {
                    parametro_general datParametro = _parametro_GeneralService.Select_parametro_general(new parametro_general { CODIGO = "RUTA_NOMBRE_EXE_CAPTURA" }).Dato;
                    if (datParametro != null)
                    {
                        nombre_exe = datParametro.VALOR;
                    }
                    // Ruta al archivo .exe
                    string rutaExe = @nombre_exe;

                    //// Crear un nuevo proceso para ejecutar el .exe
                    //ProcessStartInfo startInfo = new ProcessStartInfo();
                    //startInfo.FileName = rutaExe;
                    //startInfo.UseShellExecute = true;

                    Process process = new Process();
                    process.StartInfo.FileName = rutaExe;
                    process.Start();

                    //Process proceso = Process.Start(startInfo);
                    datRetorno.Dato = true;
                }


            }
            catch (Exception ex)
            {
                datRetorno.Msg = "Error al ejecutar el archivo .exe: " + ex.Message;
                datRetorno.Dato = false;
            }


            return Json(datRetorno);
        }

      

    }
}