using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades
{
    [DataContract]
    public class marcacion
    {
        [DataMember]
        public int ID_MARCACION { get; set; }
        [DataMember]
        public string NUMERO_DNI { get; set; }
        [DataMember]
        public DateTime? FECHA_HORA { get; set; }
        [DataMember]
        public string FECHA_TEXTO { get; set; }
        [DataMember]
        public string HORA_TEXTO { get; set; }
        [DataMember]
        public string INGRESO_SALIDA { get; set; }

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
        public string INICIA_CAPTURA { get; set; }
    }
}
