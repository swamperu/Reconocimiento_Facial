using System.Collections.Generic;
using System.ServiceModel;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{
	[ServiceContract]
	public interface Itabla_generalService
	{
	    [OperationContract]
	    DatoRetorno<JQGrid> SelectPaginated_tabla_general(Parametro dato);
	    [OperationContract]
	    DatoRetorno<tabla_general> Select_tabla_general(tabla_general dato);
        [OperationContract]
        DatoRetorno<tabla_general> SelectAll_tabla_general_by_grupo(int grupo);
        [OperationContract]
        DatoRetorno<tabla_general> SelectAll_seguimiento_log(tabla_general dato);
        [OperationContract]
	    DatoRetorno<bool> Insert_tabla_general(tabla_general dato);
	    [OperationContract]
	    DatoRetorno<bool> Update_tabla_general(tabla_general dato);
	    [OperationContract]
	    DatoRetorno<bool> Delete_tabla_general(tabla_general dato);        
		[OperationContract]
		DatoRetorno<tabla_general> SelectAll_tabla_general_by_grupo_and_rol(int grupo, int rol);
        [OperationContract]
        DatoRetorno<bool> Delete_seguimiento_log(tabla_general dato);

        [OperationContract]
		DatoRetorno<tabla_general> SelectAll_tabla_general_by_nombre_grupo(tabla_general grupo);

	}
}
