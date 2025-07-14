using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades
{
	[DataContract]
	public class ubigeo
	{
	    [DataMember]
	    public string ID_UBIGEO{ get; set; }
	    [DataMember]
	    public string DEP_CODIGO{ get; set; }
	    [DataMember]
	    public string DEP_NOMBRE{ get; set; }
	    [DataMember]
	    public string PRV_CODIGO{ get; set; }
	    [DataMember]
	    public string PRV_NOMBRE{ get; set; }
	    [DataMember]
	    public string DIS_CODIGO{ get; set; }
	    [DataMember]
	    public string DIS_NOMBRE{ get; set; }
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
	}
}
