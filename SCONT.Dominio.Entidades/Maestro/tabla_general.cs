using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades
{
	[DataContract]
	public class tabla_general
	{
	    [DataMember]
	    public int ID_TABLA_GENERAL{ get; set; }
	    [DataMember]
	    public int ID_GRUPO{ get; set; }
	    [DataMember]
	    public string NOMBRE_GRUPO{ get; set; }
	    [DataMember]
	    public string CODIGO{ get; set; }
	    [DataMember]
	    public string NOMBRE{ get; set; }
	    [DataMember]
	    public string DATO1{ get; set; }
	    [DataMember]
	    public string DATO2{ get; set; }
	    [DataMember]
	    public string DATO3{ get; set; }
	    [DataMember]
	    public int ESTADO{ get; set; }
	    [DataMember]
	    public int USUARIO_CREACION{ get; set; }
	    [DataMember]
	    public Nullable<System.DateTime> FECHA_CREACION{ get; set; }
	    [DataMember]
	    public int USUARIO_MODIFICACION{ get; set; }
	    [DataMember]
	    public Nullable<System.DateTime> FECHA_MODIFICACION{ get; set; }

        [DataMember]
        public string LOG_DNI { get; set; }
        [DataMember]
        public string LOG_COMENTARIO { get; set; }
        [DataMember]
        public string LOG_FECHA_HORA { get; set; }
    }
}
