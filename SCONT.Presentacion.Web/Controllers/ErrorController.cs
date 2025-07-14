using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SDP.Presentacion.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult AccesoDenegado()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { state = false, statusCode = HttpStatusCode.MethodNotAllowed }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }
        public ActionResult SesionExpirada()
        {
            HttpBrowserCapabilitiesBase browser = Request.Browser;
            string type = browser.Type;
            string name = browser.Browser;
            string version = browser.Version;
            if ((name == "InternetExplorer" || name == "IE") && double.Parse(version) < 10)
            {
                return View("~/Views/Home/VistaCompatibilidad.cshtml");
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { state = false, statusCode = 999 }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult PaginaNoEncontrada()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(new { state = false, statusCode = HttpStatusCode.NotFound }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult SolicitudIncorrecta()
        {
            return View("~/Views/Error/SolicitudIncorrecta.cshtml");
        }
        public ActionResult NoAutorizado()
        {
            if (Request.IsAjaxRequest())
            {
                //HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Json(new { state = false, statusCode = HttpStatusCode.Unauthorized }, JsonRequestBehavior.AllowGet);
            }
            return View("~/Views/Error/AccesoDenegado.cshtml");
        }
    }
}