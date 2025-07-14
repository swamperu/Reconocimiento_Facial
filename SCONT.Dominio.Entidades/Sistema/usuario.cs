using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades
{
	[DataContract]
	public class usuario
	{
	    [DataMember]
	    public int ID_USUARIO{ get; set; }
	    [DataMember]
	    public string USUARIO{ get; set; }
	    [DataMember]
	    public string CONTRASENIA{ get; set; }
        [DataMember]
        public string NUEVA_CONTRASENIA { get; set; }
        [DataMember]
        public string CONFIRMAR_NUEVA_CONTRASENIA{ get; set; }
	    [DataMember]
	    public string DNI{ get; set; }
	    [DataMember]
	    public string NOMBRES{ get; set; }
	    [DataMember]
	    public string APELLIDO_PATERNO{ get; set; }
	    [DataMember]
	    public string APELLIDO_MATERNO{ get; set; }
	    [DataMember]
	    public string ID_ROL{ get; set; }
	    [DataMember]
	    public Nullable<System.DateTime> ULTIMO_INGRESO{ get; set; }
	    [DataMember]
	    public string CORREO{ get; set; }
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
        public string NOMBRE_ROL { get; set; }
		[DataMember]
		public string NOMBRE_COMPLETO { get; set; }
        [DataMember]
        public string TOKEN_RECUPERACION { get; set; }
    }
}
