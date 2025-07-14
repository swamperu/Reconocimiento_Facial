using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{
	public interface Itabla_generalRepository
	{
	    JQGrid SelectPaginated_tabla_general(Parametro objdato);
	    tabla_general Select_tabla_general(tabla_general objdato);
        List<tabla_general> SelectAll_tabla_general_by_grupo(int grupo);
        bool Insert_tabla_general(tabla_general objdato);
	    bool Update_tabla_general(tabla_general objdato);
	    bool Delete_tabla_general(tabla_general objdato);
		List<tabla_general> SelectAll_tabla_general_by_grupo_and_rol(int grupo, int rol);
        bool Delete_seguimiento_log(tabla_general dato);

        List<tabla_general> SelectAll_tabla_general_by_nombre_grupo(tabla_general grupo);
        List<tabla_general> SelectAll_seguimiento_log(tabla_general dato);

    }
}
