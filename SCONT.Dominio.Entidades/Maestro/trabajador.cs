using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades
{
    [DataContract]
    public class trabajador
    {
        [DataMember]
        public int ID_TRABAJADOR { get; set; }
        [DataMember]
        public string NUMERO_DNI { get; set; }
        [DataMember]
        public string NOMBRES { get; set; }
        [DataMember]
        public string APELLIDO_PATERNO { get; set; }
        [DataMember]
        public string APELLIDO_MATERNO { get; set; }
        [DataMember]
        public string ID_GENERO { get; set; }
        [DataMember]
        public DateTime? FECHA_NACIMIENTO { get; set; }
        [DataMember]
        public string FECHA_NACIMIENTO_TEXTO { get; set; }
        [DataMember]
        public string CORREO_ELECTRONICO { get; set; }
        [DataMember]
        public string TELEFONO { get; set; }
        [DataMember]
        public DateTime? FECHA_INGRESO { get; set; }
        [DataMember]
        public string FECHA_INGRESO_TEXTO { get; set; }
        [DataMember]
        public string DEP_CODIGO { get; set; }
        [DataMember]
        public string PRV_CODIGO { get; set; }
        [DataMember]
        public string DIS_CODIGO { get; set; }
        [DataMember]
        public string DIRECCION { get; set; }
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
        [DataMember]
        public string ID_HORARIO_LABORAL { get; set; }
    }
}
