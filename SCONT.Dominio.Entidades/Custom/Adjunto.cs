using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    [DataContract]
    public class Adjunto : BaseEntitity
    {

        [DataMember]
        public int ID_ADJUNTO { get; set; }
        [DataMember]
        public string FORMATO_MIME { get; set; }
        [DataMember]
        public string NOMBRE_ARCHIVO_ORIGEN { get; set; }
        [DataMember]
        public string EXTENSION_ARCHIVO_ORIGEN { get; set; }
        [DataMember]
        public int? TAMANO_ARCHIVO_ORIGEN { get; set; }
        [DataMember]
        public string NOMBRE_ARCHIVO_ALMACENADO { get; set; }
        [DataMember]
        public bool? ACTIVO { get; set; }
        [DataMember]
        public string USUARIO_CREACION { get; set; }
        [DataMember]
        public string USUARIO_MODIFICACION { get; set; }

    }
}
