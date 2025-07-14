using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{
    public interface ImarcacionRepository
    {
        JQGrid SelectPaginated_marcacion(Parametro objdato);
        bool Insert_marcacion(marcacion objdato);
        bool Delete_marcacion(marcacion objdato);

        DataTable SelectAll_reporte_horas_trabajadas_toExport(Parametro objdato);
    }
}
