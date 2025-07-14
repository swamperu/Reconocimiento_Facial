using System.Collections.Generic;
using System.ServiceModel;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{
	[ServiceContract]
	public interface Iusuario_rolService
	{
	    [OperationContract]
	    DatoRetorno<JQGrid> SelectPaginated_usuario_rol(Parametro dato);
	    [OperationContract]
	    DatoRetorno<usuario_rol> Select_usuario_rol(usuario_rol dato);
        [OperationContract]
        DatoRetorno<usuario_rol> Select_usuario_rol_by_documento(usuario_rol dato);
	    [OperationContract]
	    DatoRetorno<bool> Insert_usuario_rol(usuario_rol dato);
	    [OperationContract]
	    DatoRetorno<bool> Update_usuario_rol(usuario_rol dato);
	    [OperationContract]
	    DatoRetorno<bool> Delete_usuario_rol(usuario_rol dato);
        [OperationContract]
        DatoRetorno<usuario_rol> SelectAll_rol();

        [OperationContract]
        DatoRetorno<JQGrid> SelectPaginated_usuario_rol_direccion(Parametro dato);
        [OperationContract]
        DatoRetorno<JQGrid> SelectPaginated_usuario_rol_ejecutora(Parametro dato);
        [OperationContract]
        DatoRetorno<JQGrid> SelectPaginated_usuario_rol_zona(Parametro dato);
        [OperationContract]
        DatoRetorno<usuario_rol> SelectAll_zona_by_usuario(usuario_rol dato);
        [OperationContract]
        DatoRetorno<usuario_rol> SelectAll_zona_by_usuario_ejecutora(usuario_rol dato);
        [OperationContract]
        DatoRetorno<usuario_rol> SelectAll_ejecutora_by_usuario_zona_region(usuario_rol dato);

	}
}
