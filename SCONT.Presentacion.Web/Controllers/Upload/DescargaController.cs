using MED.Security.API.Authorization;
using SCONT.Aplicacion.Contratos;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Infraestructura.Transversal;
using SCONT.Presentacion.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SCONT.Presentacion.Web.Controllers.Upload
{
    public class DescargaController : MEDBaseController
    {
        //[Route("descarga/descarga_incidencia")]
        //public ActionResult descarga_incidencia()
        //{
        //    asp_orden_incidencia datIncidencia = (asp_orden_incidencia)sisSesion.Reporte_Parametro;
        //    string carpeta = sisVariable.Ruta_Carga_Incidencia;
        //    string archivo = datIncidencia.ARCHIVO_FISICO;
        //    string app = "";
        //    app = "application/pdf";
        //    return File(carpeta + archivo, app);
        //}

    }
}