using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades.Sistema
{

    /// <summary>
    /// Nombre       : recurso
    /// Descripción  : Métodos de trabajo para recurso
    /// </summary>
    /// <remarks>
    /// Creacion     : 11/08/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract]
    public class recurso : BaseEntitity
{
        [DataMember]
        public int ID_RECURSO { get; set; }
        [DataMember]
        public string CODIGO { get; set; }
        [DataMember]
        public string ID_ROL { get; set; }
        [DataMember]
        public string NOMBRE { get; set; }
        [DataMember]
        public string TIPO { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public string RELACION { get; set; }
        [DataMember]
        public string TABLA { get; set; }
        [DataMember]
        public string IMAGEN { get; set; }
        [DataMember]
        public string DESCRIPCION { get; set; }
        [DataMember]
        public string GRUPO { get; set; }
        [DataMember]
        public string GRUPO_NOMBRE { get; set; }      
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
        public int ORDEN { get; set; }
    }
}
