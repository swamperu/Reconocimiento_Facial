using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{
	public interface Iusuario_rolRepository
	{
	    JQGrid SelectPaginated_usuario_rol(Parametro objdato);
	    usuario_rol Select_usuario_rol(usuario_rol objdato);
        usuario_rol Select_usuario_rol_by_documento(usuario_rol objdato);
	    bool Insert_usuario_rol(usuario_rol objdato);
	    bool Update_usuario_rol(usuario_rol objdato);
	    bool Delete_usuario_rol(usuario_rol objdato);
        List<usuario_rol> SelectAll_rol();

        JQGrid SelectPaginated_usuario_rol_direccion(Parametro objdato);
        JQGrid SelectPaginated_usuario_rol_ejecutora(Parametro objdato);
        JQGrid SelectPaginated_usuario_rol_zona(Parametro objdato);
        bool Delete_usuario_rol_ejecutora(usuario_rol objdato);
        bool Insert_usuario_rol_ejecutora(usuario_rol objdato);
        bool Delete_usuario_rol_direccion(usuario_rol objdato);
        bool Insert_usuario_rol_direccion(usuario_rol objdato);
        bool Delete_usuario_rol_zona(usuario_rol objdato);
        bool Insert_usuario_rol_zona(usuario_rol objdato);
        List<usuario_rol> SelectAll_zona_by_usuario(usuario_rol objdato);
        List<usuario_rol> SelectAll_zona_by_usuario_ejecutora(usuario_rol objdato);
        List<usuario_rol> SelectAll_ejecutora_by_usuario_zona_region(usuario_rol objdato);
	}
}
