using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCONT.Presentacion.Web.Models.ViewModel
{
    public class RecoveryViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un valor en el campo USUARIO")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Debe ingresar un valor en el campo CORREO ELECTRÓNICO")]
        [EmailAddress]
        public string Correo { get; set; }
    }
}