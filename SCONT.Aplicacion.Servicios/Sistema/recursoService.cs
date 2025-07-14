using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Dominio.Entidades.Sistema;
using SCONT.Dominio.Contratos;
using SCONT.Aplicacion.Contratos;
using SCONT.Infraestructura.Dao;
using SCONT.Infraestructura.Transversal;

namespace SCONT.Aplicacion.Servicios
{
    public class recursoService: IrecursoService
    {
        private  IrecursoRepository _recursoRepository;
        public recursoService() {
            _recursoRepository = new recursoRepository();
        }

        public DatoRetorno<recurso> SelectAll_recurso(recurso objdato)
        {
            DatoRetorno<recurso> datRetorno = new DatoRetorno<recurso>();
            datRetorno.Lista = _recursoRepository.SelectAll_recurso(objdato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

    }
}
