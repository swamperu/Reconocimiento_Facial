using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    /// <summary>
    /// Nombre       : JQCombo
    /// Descripción  : Métodos de trabajo para JQCombo
    /// </summary>
    /// <remarks>
    /// Creacion     : 09/05/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract()]
    public class JQCombo
    {
        [DataMember()]
        public string Id { get; set; }

        [DataMember()]
        public string Valor { get; set; }
    }
}
