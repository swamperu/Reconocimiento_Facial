using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades.Parametrica
{

    /// <summary>
    /// Nombre       : parametro_general
    /// Descripción  : Métodos de trabajo para parametro_general
    /// </summary>
    /// <remarks>
    /// Creacion     : 25/10/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract]
    public class parametro_general : BaseEntitity
{
        [DataMember]
        public int ID_PARAMETRO_GENERAL { get; set; }
        [DataMember]
        public string CODIGO { get; set; }
        [DataMember]
        public string NOMBRE { get; set; }
        [DataMember]
        public string CODIGO_NOMBRE { get; set; }
        [DataMember]
        public string VALOR { get; set; }
        [DataMember]
        public int ESTADO { get; set; }
        [DataMember]
        public string USUARIO_CREACION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> FECHA_CREACION { get; set; }
        [DataMember]
        public string USUARIO_MODIFICACION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        [DataMember]
        public string EJECUTORA { get; set; }
    }
}
