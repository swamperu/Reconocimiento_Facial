using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    /// <summary>
    /// Nombre       : Sistema
    /// Descripción  : Métodos de trabajo para Sistema
    /// </summary>
    /// <remarks>
    /// Creacion     : 09/05/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract()]
    public class Sistema
    {
        [DataMember()]
        public string CODIGO_SISTEMA { get; set; }
        [DataMember()]
        public string IDs { get; set; }
        [DataMember()]
        public string AutenticarUsuarioIDs { get; set; }
        [DataMember()]
        public string RegistrarUsuarioIDs { get; set; }
        [DataMember()]
        public string CambiarContraseniaUsuarioIDs { get; set; }
    }
}
