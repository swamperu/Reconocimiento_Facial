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

namespace SCONT.Aplicacion.Servicios
{
    public class trabajadorService : ItrabajadorService
    {
        private ItrabajadorRepository _trabajadorRepository;
        private Itabla_generalRepository _tabla_generalRepository;

        public trabajadorService()
        {
            _trabajadorRepository = new trabajadorRepository();
            _tabla_generalRepository = new tabla_generalRepository();
        }

        public DatoRetorno<JQGrid> SelectPaginated_trabajador(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _trabajadorRepository.SelectPaginated_trabajador(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<trabajador> Select_trabajador(trabajador dato)
        {
            DatoRetorno<trabajador> datRetorno = new DatoRetorno<trabajador>();
            datRetorno.Dato = _trabajadorRepository.Select_trabajador(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<trabajador> Select_trabajador_by_dni(trabajador dato)
        {
            DatoRetorno<trabajador> datRetorno = new DatoRetorno<trabajador>();
            datRetorno.Dato = _trabajadorRepository.Select_trabajador_by_dni(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<bool> Insert_trabajador(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            trabajador datExiste = _trabajadorRepository.Select_trabajador_by_dni(dato);
            if(datExiste != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "El N° DNI ingresado ya existe, verifique";
                return datRetorno;
            }

            datRetorno.Dato = _trabajadorRepository.Insert_trabajador(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
            return datRetorno;
        }

        public DatoRetorno<bool> Update_trabajador(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            trabajador datExiste = _trabajadorRepository.Select_trabajador_by_dni(dato);
            if (datExiste != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "El N° DNI ingresado ya existe, verifique";
                return datRetorno;
            }
            datRetorno.Dato = _trabajadorRepository.Update_trabajador(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            return datRetorno;
        }

        public DatoRetorno<bool> Update_trabajador_captura(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            trabajador datExiste = _trabajadorRepository.Select_trabajador(dato);
            if (datExiste != null)
            {
                if(datExiste.INICIA_CAPTURA == "1")
                {
                    datRetorno.Dato = false;
                    datRetorno.Msg = "El proceso de captura de imágenes ya se encuentra iniciada";
                    return datRetorno;
                }
            }
            datRetorno.Dato = _trabajadorRepository.Update_trabajador_captura(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);

            if (datRetorno.Dato)
            {
                _tabla_generalRepository.Delete_seguimiento_log(new tabla_general { LOG_DNI = datExiste.NUMERO_DNI });
            }
            return datRetorno;
        }

        public DatoRetorno<bool> Delete_trabajador(trabajador dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _trabajadorRepository.Delete_trabajador(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
            return datRetorno;
        }

    }
}
