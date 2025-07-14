using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{
    public interface ItrabajadorRepository
    {
        JQGrid SelectPaginated_trabajador(Parametro objdato);
        trabajador Select_trabajador(trabajador objdato);
        trabajador Select_trabajador_by_dni(trabajador objdato);
        bool Insert_trabajador(trabajador objdato);
        bool Update_trabajador(trabajador objdato);
        bool Update_trabajador_captura(trabajador objdato);
        bool Delete_trabajador(trabajador objdato);
    }
}
