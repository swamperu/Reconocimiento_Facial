using MED.Security.API.Authorization;
using SCONT.Aplicacion.Contratos;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Infraestructura.Transversal;
using SCONT.Presentacion.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCONT.Presentacion.Web.Controllers
{
    public class HomeController : MEDBaseController
    {
        private readonly IAuthorization _authorization;

        public HomeController(IAuthorization authorization)
        {
            _authorization = authorization;
        }

        public ActionResult Index()
        {
            HttpBrowserCapabilitiesBase browser = Request.Browser;
            string type = browser.Type;
            string name = browser.Browser;
            string version = browser.Version;
            if ((name == "InternetExplorer" || name == "IE") && double.Parse(version) < 10)
            {
                return View("~/Views/Home/VistaCompatibilidad.cshtml");
            }
            ViewBag.TAB = sisSesion.Tab_Seleccionado;

            usuario datUsuario = (usuario)sisSesion.Usuario;
            DateTime? fechaActual = sisFuncion.GetFechaServidor();
            //fechaActual = Convert.ToDateTime("03/03/2023");
            ViewBag.FechaActual = fechaActual.Value.ToString("dd/MM/yyyy");
            ViewBag.Rol = datUsuario.ID_ROL;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult GetFecha()
        {
            //DateTime? hoy = sisFuncion.GetFechaServidor();
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = true;
            datRetorno.Msg = "";
            return Json(datRetorno);
        }

        [HttpPost]
        [Route("Home/SetTabSeleccionado")]
        public JsonResult SetTabSeleccionado(string dato)
        {
            DatoRetorno<string> datRetorno = new DatoRetorno<string>();
            sisSesion.Tab_Seleccionado = dato;
            datRetorno.Dato = dato;
            datRetorno.Msg = "";
            return Json(datRetorno);
        }

    }
}