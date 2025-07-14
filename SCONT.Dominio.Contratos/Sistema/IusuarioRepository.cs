using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{
	public interface IusuarioRepository
	{
	    JQGrid SelectPaginated_usuario(Parametro objdato);
        usuario Select_usuario(usuario objdato);
        usuario Select_usuario_by_usuario(usuario objdato);
        usuario Select_usuario_by_correo(usuario objdato);
        usuario Select_usuario_by_dni(usuario objdato);
        usuario Select_usuario_by_datos(usuario objdato);
        usuario Select_usuario_login(usuario objdato);
        bool Insert_usuario(usuario objdato);
	    bool Update_usuario(usuario objdato);
        bool Update_usuario_ingreso(usuario objdato);
        bool Delete_usuario(usuario objdato);
        bool Update_usuario_password(usuario objdato);
        List<usuario> SelectAll_usuario_by_rol(string id_rol);
        List<usuario> SelectAll_usuario();
        bool Update_usuario_encriptado(usuario objdato);
        usuario Select_usuario_recuperacion(usuario objdato);
        usuario Select_usuario_by_token(usuario objdato);
        bool Update_usuario_recuperacion(usuario objdato);
        DataTable SelectAll_reporte_usuario_logistica_toExport();
        DataTable SelectAll_reporte_usuario_oficina_usuaria_toExport();
    }
}
