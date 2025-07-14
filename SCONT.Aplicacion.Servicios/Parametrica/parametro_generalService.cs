using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Dominio.Entidades.Parametrica;
using SCONT.Dominio.Contratos;
using SCONT.Aplicacion.Contratos;
using SCONT.Infraestructura.Dao;
using SCONT.Infraestructura.Transversal;

namespace SCONT.Aplicacion.Servicios
{
    public class parametro_generalService: Iparametro_generalService
    {
        private  Iparametro_generalRepository _parametro_generalRepository;
        public parametro_generalService() {
            _parametro_generalRepository = new parametro_generalRepository();
        }

        public DatoRetorno<parametro_general> Insert_parametro_general(parametro_general dato)
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Dato = (_parametro_generalRepository.Insert_parametro_general(dato) ? dato : null);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
            return datRetorno;
        }

        public DatoRetorno<parametro_general> Update_parametro_general(parametro_general dato)
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Dato = (_parametro_generalRepository.Update_parametro_general(dato) ? dato : null);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            return datRetorno;
        }

        public DatoRetorno<parametro_general> Delete_parametro_general(parametro_general dato)
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Dato = (_parametro_generalRepository.Delete_parametro_general(dato) ? dato : null);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
            return datRetorno;
        }

        public DatoRetorno<parametro_general> Select_parametro_general(parametro_general dato)
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Dato = _parametro_generalRepository.Select_parametro_general(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<parametro_general> SelectAll_parametro_general()
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Lista = _parametro_generalRepository.SelectAll_parametro_general();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<JQGrid>  SelectPaginated_parametro_general(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _parametro_generalRepository.SelectPaginated_parametro_general(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<parametro_general> SelAll_ejecutora()
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Lista = _parametro_generalRepository.SelAll_ejecutora();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<parametro_general> SelAll_ejecutora_usuario_centro_costo( Parametro dato)
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Lista = _parametro_generalRepository.SelAll_ejecutora_usuario_centro_costo(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<parametro_general> SelAll_tipo_proceso()
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Lista = _parametro_generalRepository.SelAll_tipo_proceso();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<parametro_general> SelAll_tipo_proceso_ps()
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Lista = _parametro_generalRepository.SelAll_tipo_proceso_ps();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<parametro_general> SelAll_modalidad_compra_asp()
        {
            DatoRetorno<parametro_general> datRetorno = new DatoRetorno<parametro_general>();
            datRetorno.Lista = _parametro_generalRepository.SelAll_modalidad_compra_asp();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

    }
}
