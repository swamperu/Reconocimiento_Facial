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
	public class ubigeoService: IubigeoService
	{
	    private IubigeoRepository _ubigeoRepository;
	    public ubigeoService(){
	        _ubigeoRepository = new ubigeoRepository();
	    }

	    public DatoRetorno<JQGrid>  SelectPaginated_ubigeo(Parametro dato)
	    {
	        DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
	        datRetorno.Dato = _ubigeoRepository.SelectPaginated_ubigeo(dato);
	        datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
	        return datRetorno;
	    }

	    public DatoRetorno<ubigeo> Select_ubigeo(ubigeo dato)
	    {
	        DatoRetorno<ubigeo> datRetorno = new DatoRetorno<ubigeo>();
	        datRetorno.Dato = _ubigeoRepository.Select_ubigeo(dato);
	        datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
	        return datRetorno;
	    }

        public DatoRetorno<ubigeo> SelectAll_departamento()
        {
            DatoRetorno<ubigeo> datRetorno = new DatoRetorno<ubigeo>();
            datRetorno.Lista = _ubigeoRepository.SelectAll_departamento();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<ubigeo> SelectAll_provincia(ubigeo dato)
        {
            DatoRetorno<ubigeo> datRetorno = new DatoRetorno<ubigeo>();
            datRetorno.Lista = _ubigeoRepository.SelectAll_provincia(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<ubigeo> SelectAll_distrito(ubigeo dato)
        {
            DatoRetorno<ubigeo> datRetorno = new DatoRetorno<ubigeo>();
            datRetorno.Lista = _ubigeoRepository.SelectAll_distrito(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<bool> Insert_ubigeo(ubigeo dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _ubigeoRepository.Insert_ubigeo(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
	        return datRetorno;
	    }

	    public DatoRetorno<bool> Update_ubigeo(ubigeo dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _ubigeoRepository.Update_ubigeo(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
	        return datRetorno;
	    }

	    public DatoRetorno<bool> Delete_ubigeo(ubigeo dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _ubigeoRepository.Delete_ubigeo(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
	        return datRetorno;
	    }
	}
}
