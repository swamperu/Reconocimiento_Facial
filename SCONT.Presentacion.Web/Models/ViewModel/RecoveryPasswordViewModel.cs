using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCONT.Presentacion.Web.Models.ViewModel
{
    public class RecoveryPasswordViewModel
    {
        public string token { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Password2 { get; set; }
    }
}