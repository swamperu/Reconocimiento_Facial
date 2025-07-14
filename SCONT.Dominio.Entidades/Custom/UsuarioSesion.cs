using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    /// <summary>
    /// Nombre       : UsuarioSesion
    /// Descripción  : Métodos de trabajo para UsuarioSesion
    /// </summary>
    /// <remarks>
    /// Creacion     : 09/05/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract()]
    public class UsuarioSesion
    {
        //[DataMember()]
        //public long IdUsuario { get; set; }
        //[DataMember()]
        //public Guid IdInicioSesion { get; set; }
        //[DataMember()]
        //public bool EsValido { get; set; }
        //[DataMember()]
        //public string UserName { get; set; }
        //[DataMember()]
        //public string CodigoSistema { get; set; }
        //[DataMember()]
        //public string Correo { get; set; }
        //[DataMember()]
        //public string Nombres { get; set; }
        //[DataMember()]
        //public string Apellidos { get; set; }
        //[DataMember()]
        //public string Dni { get; set; }
        //[DataMember()]
        //public int IdTipoDocumento { get; set; }
        //[DataMember()]
        //public string TipoDocumento { get; set; }
        //[DataMember()]
        //public KeyValuePair<string, string> Mensaje { get; set; }
        //[DataMember()]
        //public string CodigoModularUsuario { get; set; }
        [DataMember()]
        public string APELLIDO_MATERNO { get; set; }
        [DataMember()]
        public string APELLIDO_PATERNO { get; set; }
        [DataMember()]
        public string CodigoSistema { get; set; }
        [DataMember()]
        public string CORREO_USUARIO { get; set; }
        [DataMember()]
        public bool DebeCambiarClave { get; set; }
        [DataMember()]
        public int DNI_VALIDO_RENIEC { get; set; }
        [DataMember()]
        public bool EsSuperUsuario { get; set; }
        [DataMember()]
        public bool EstaBloqueado { get; set; }
        [DataMember()]
        public bool EsValido { get; set; }
        [DataMember()]
        public int? ID_ROL { get; set; }
        [DataMember()]
        public int ID_SISTEMA { get; set; }
        [DataMember()]
        public int? ID_TIPO_DOCUMENTO { get; set; }
        [DataMember()]
        public long ID_USUARIO { get; set; }
        [DataMember()]
        public Guid IdInicioSesion { get; set; }
        [DataMember()]
        public KeyValuePair<string, string> Mensaje { get; set; }
        [DataMember()]
        public string NOMBRE_COMPLETO { get; set; }
        [DataMember()]
        public string NOMBRES_USUARIO { get; set; }
        [DataMember()]
        public string NUMERO_DOCUMENTO { get; set; }
        [DataMember()]
        public string TIPO_DOCUMENTO { get; set; }
        [DataMember()]
        public DateTime? UltimoInicioSesion { get; set; }
        [DataMember()]
        public string URL_SISTEMA { get; set; }
        [DataMember()]
        public string UserName { get; set; }

        [DataMember()]
        public int CUENTA_DIR { get; set; }
        [DataMember()]
        public int CUENTA_EJE { get; set; }
        [DataMember()]
        public int CUENTA_ZON { get; set; }
    }
}
