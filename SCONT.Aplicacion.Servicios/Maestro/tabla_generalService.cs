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
	public class tabla_generalService: Itabla_generalService
	{
	    private Itabla_generalRepository _tabla_generalRepository;
	    public tabla_generalService(){
	        _tabla_generalRepository = new tabla_generalRepository();
	    }

	    public DatoRetorno<JQGrid>  SelectPaginated_tabla_general(Parametro dato)
	    {
	        DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
	        datRetorno.Dato = _tabla_generalRepository.SelectPaginated_tabla_general(dato);
	        datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
	        return datRetorno;
	    }

	    public DatoRetorno<tabla_general> Select_tabla_general(tabla_general dato)
	    {
	        DatoRetorno<tabla_general> datRetorno = new DatoRetorno<tabla_general>();
	        datRetorno.Dato = _tabla_generalRepository.Select_tabla_general(dato);
	        datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
	        return datRetorno;
	    }

        public DatoRetorno<tabla_general> SelectAll_seguimiento_log(tabla_general dato)
        {
            DatoRetorno<tabla_general> datRetorno = new DatoRetorno<tabla_general>();
            datRetorno.Lista = _tabla_generalRepository.SelectAll_seguimiento_log(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<tabla_general> SelectAll_tabla_general_by_grupo(int grupo)
        {
            DatoRetorno<tabla_general> datRetorno = new DatoRetorno<tabla_general>();
            datRetorno.Lista = _tabla_generalRepository.SelectAll_tabla_general_by_grupo(grupo);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<bool> Insert_tabla_general(tabla_general dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _tabla_generalRepository.Insert_tabla_general(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
	        return datRetorno;
	    }

	    public DatoRetorno<bool> Update_tabla_general(tabla_general dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _tabla_generalRepository.Update_tabla_general(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
	        return datRetorno;
	    }

	    public DatoRetorno<bool> Delete_tabla_general(tabla_general dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _tabla_generalRepository.Delete_tabla_general(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
	        return datRetorno;
	    }

        public DatoRetorno<bool> Delete_seguimiento_log(tabla_general dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _tabla_generalRepository.Delete_seguimiento_log(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
            return datRetorno;
        }

        public DatoRetorno<tabla_general> SelectAll_tabla_general_by_grupo_and_rol(int grupo, int rol)
		{
			DatoRetorno<tabla_general> datRetorno = new DatoRetorno<tabla_general>();
			datRetorno.Lista = _tabla_generalRepository.SelectAll_tabla_general_by_grupo_and_rol(grupo,rol);
			datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
			return datRetorno;
		}

		public DatoRetorno<tabla_general> SelectAll_tabla_general_by_nombre_grupo(tabla_general grupo)
		{
			DatoRetorno<tabla_general> datRetorno = new DatoRetorno<tabla_general>();
			datRetorno.Lista = _tabla_generalRepository.SelectAll_tabla_general_by_nombre_grupo(grupo);
			datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
			return datRetorno;
		}

	}
}
