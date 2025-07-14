using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCONT.Presentacion.Web.Models
{
    public class UsuarioViewModel
    {
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string CaptchaValido { get; set; }
        public string Captcha { get; set; }
        public string credenciales { get; set; }
    }
}