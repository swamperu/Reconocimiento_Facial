using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Transactions;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Contratos;
using SCONT.Aplicacion.Contratos;
using SCONT.Infraestructura.Dao;
using SCONT.Infraestructura.Transversal;
using SCONT.Dominio.Entidades.Custom;
using System.Data;

namespace SCONT.Aplicacion.Servicios
{
    public class marcacionService : ImarcacionService
    {
        private ImarcacionRepository _marcacionRepository;
        public marcacionService()
        {
            _marcacionRepository = new marcacionRepository();
        }

        public DatoRetorno<JQGrid> SelectPaginated_marcacion(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _marcacionRepository.SelectPaginated_marcacion(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<bool> Insert_marcacion(marcacion dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _marcacionRepository.Insert_marcacion(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
            return datRetorno;
        }

        public DatoRetorno<bool> Delete_marcacion(marcacion dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _marcacionRepository.Delete_marcacion(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
            return datRetorno;
        }

        public DatoRetorno<DataTable> SelectAll_reporte_horas_trabajadas_toExport(Parametro dato)
        {
            DatoRetorno<DataTable> datRetorno = new DatoRetorno<DataTable>();
            datRetorno.Dato = _marcacionRepository.SelectAll_reporte_horas_trabajadas_toExport(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

    }
}
