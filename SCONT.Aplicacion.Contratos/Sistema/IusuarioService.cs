using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Aplicacion.Contratos
{
	[ServiceContract]
	public interface IusuarioService
	{
	    [OperationContract]
	    DatoRetorno<JQGrid> SelectPaginated_usuario(Parametro dato);
        [OperationContract]
	    DatoRetorno<usuario> Select_usuario(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_by_usuario(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_by_correo(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_by_dni(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_by_datos(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_login(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_recuperacion(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> Select_usuario_by_token(usuario dato);
        [OperationContract]
	    DatoRetorno<bool> Insert_usuario(usuario dato);
	    [OperationContract]
	    DatoRetorno<bool> Update_usuario(usuario dato);
        [OperationContract]
        DatoRetorno<bool> Update_usuario_ingreso(usuario dato);
        [OperationContract]
	    DatoRetorno<bool> Delete_usuario(usuario dato);
        [OperationContract]
        DatoRetorno<bool> Update_usuario_password(usuario dato);
        [OperationContract]
        DatoRetorno<usuario> SelectAll_usuario_by_rol(string id_rol);
        [OperationContract]
        DatoRetorno<bool> Update_usuario_encriptado();
        [OperationContract]
        DatoRetorno<bool> Update_usuario_recuperacion(usuario dato);
        [OperationContract]
        DatoRetorno<bool> Update_usuario_password_recuperacion(usuario dato);
        [OperationContract]
        DatoRetorno<bool> ContrasenaSegura(string contraseñaSinVerificar, bool es_cambiar_contrasenia);
        [OperationContract]
        DatoRetorno<DataTable> SelectAll_reporte_usuario_logistica_toExport();
        [OperationContract]
        DatoRetorno<DataTable> SelectAll_reporte_usuario_oficina_usuaria_toExport();
    }
}
