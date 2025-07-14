using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;

namespace SCONT.Dominio.Contratos
{
	public interface IubigeoRepository
	{
	    JQGrid SelectPaginated_ubigeo(Parametro objdato);
	    ubigeo Select_ubigeo(ubigeo objdato);
        List<ubigeo> SelectAll_departamento();
        List<ubigeo> SelectAll_provincia(ubigeo objdato);
        List<ubigeo> SelectAll_distrito(ubigeo objdato);
        bool Insert_ubigeo(ubigeo objdato);
	    bool Update_ubigeo(ubigeo objdato);
	    bool Delete_ubigeo(ubigeo objdato);
	}
}
