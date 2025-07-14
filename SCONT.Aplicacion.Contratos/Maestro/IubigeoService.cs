using System.Collections.Generic;
using System.ServiceModel;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{
	[ServiceContract]
	public interface IubigeoService
	{
	    [OperationContract]
	    DatoRetorno<JQGrid> SelectPaginated_ubigeo(Parametro dato);
	    [OperationContract]
	    DatoRetorno<ubigeo> Select_ubigeo(ubigeo dato);
        [OperationContract]
        DatoRetorno<ubigeo> SelectAll_departamento();
        [OperationContract]
        DatoRetorno<ubigeo> SelectAll_provincia(ubigeo dato);
        [OperationContract]
        DatoRetorno<ubigeo> SelectAll_distrito(ubigeo dato);
        [OperationContract]
	    DatoRetorno<bool> Insert_ubigeo(ubigeo dato);
	    [OperationContract]
	    DatoRetorno<bool> Update_ubigeo(ubigeo dato);
	    [OperationContract]
	    DatoRetorno<bool> Delete_ubigeo(ubigeo dato);
	}
}
