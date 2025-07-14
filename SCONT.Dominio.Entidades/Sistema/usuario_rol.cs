using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades
{
	[DataContract]
	public class usuario_rol
	{
	    [DataMember]
	    public int ID_USUARIO_ROL{ get; set; }
	    [DataMember]
	    public int ID_USUARIO{ get; set; }
	    [DataMember]
	    public int ID_ROL{ get; set; }
        [DataMember]
        public string NOMBRE_ROL { get; set; }
	    [DataMember]
	    public string DOCUMENTO_IDENTIDAD{ get; set; }
	    [DataMember]
	    public string NOMBRE{ get; set; }
	    [DataMember]
	    public int ESTADO{ get; set; }
	    [DataMember]
	    public string USUARIO_CREACION{ get; set; }
	    [DataMember]
	    public Nullable<System.DateTime> FECHA_CREACION{ get; set; }
	    [DataMember]
	    public string USUARIO_MODIFICACION{ get; set; }
	    [DataMember]
	    public Nullable<System.DateTime> FECHA_MODIFICACION{ get; set; }

        [DataMember]
        public List<string> LISTA_ZONA { get; set; }
        [DataMember]
        public List<string> LISTA_EJECUTORA { get; set; }
        [DataMember]
        public List<string> LISTA_DESTINO { get; set; }
        [DataMember]
        public List<string> LISTA_DIRECCION { get; set; }
        [DataMember]
        public string SEC_EJEC { get; set; }
        [DataMember]
        public int ID_DIRECCION { get; set; }
        [DataMember]
        public int ID_ZONA { get; set; }
        [DataMember]
        public string NOMBRE_ZONA { get; set; }
        [DataMember]
        public string NOMBRE_REGION { get; set; }
        [DataMember]
        public string NOMBRE_EJECUTORA { get; set; }

        [DataMember]
        public int CUENTA_DIRECCION { get; set; }
        [DataMember]
        public int CUENTA_EJECUTORA { get; set; }
        [DataMember]
        public int CUENTA_ZONA { get; set; }
	}
}
