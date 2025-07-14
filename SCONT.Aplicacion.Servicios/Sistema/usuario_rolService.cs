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
using SCONT.Dominio.Entidades .Custom ;

namespace SCONT.Aplicacion.Servicios
{
	public class usuario_rolService: Iusuario_rolService
	{
	    private Iusuario_rolRepository _usuario_rolRepository;
	    public usuario_rolService(){
	        _usuario_rolRepository = new usuario_rolRepository();
	    }

	    public DatoRetorno<JQGrid>  SelectPaginated_usuario_rol(Parametro dato)
	    {
	        DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
	        datRetorno.Dato = _usuario_rolRepository.SelectPaginated_usuario_rol(dato);
	        datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
	        return datRetorno;
	    }

	    public DatoRetorno<usuario_rol> Select_usuario_rol(usuario_rol dato)
	    {
	        DatoRetorno<usuario_rol> datRetorno = new DatoRetorno<usuario_rol>();
	        datRetorno.Dato = _usuario_rolRepository.Select_usuario_rol(dato);
	        datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
	        return datRetorno;
	    }

        public DatoRetorno<usuario_rol> Select_usuario_rol_by_documento(usuario_rol dato)
        {
            DatoRetorno<usuario_rol> datRetorno = new DatoRetorno<usuario_rol>();
            datRetorno.Dato = _usuario_rolRepository.Select_usuario_rol_by_documento(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

	    public DatoRetorno<bool> Insert_usuario_rol(usuario_rol dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _usuario_rolRepository.Insert_usuario_rol(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
	        return datRetorno;
	    }

	    public DatoRetorno<bool> Update_usuario_rol(usuario_rol dato)
	    {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = true;
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            using (TransactionScope tran = new TransactionScope())
            {
                if (dato.LISTA_EJECUTORA.Count > 0)
                {
                    _usuario_rolRepository.Delete_usuario_rol_ejecutora(dato);
                    foreach (string registro in dato.LISTA_EJECUTORA)
                    {
                        dato.SEC_EJEC = registro;
                        datRetorno.Dato = _usuario_rolRepository.Insert_usuario_rol_ejecutora(dato);
                        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
                    }
                }
                if (dato.LISTA_DIRECCION.Count > 0)
                {
                    _usuario_rolRepository.Delete_usuario_rol_direccion(dato);
                    foreach (string registro in dato.LISTA_DIRECCION)
                    {
                        dato.ID_DIRECCION = int.Parse(registro);
                        datRetorno.Dato = _usuario_rolRepository.Insert_usuario_rol_direccion(dato);
                        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
                    }
                }
                if (dato.LISTA_ZONA.Count > 0)
                {
                    _usuario_rolRepository.Delete_usuario_rol_zona(dato);
                    foreach (string registro in dato.LISTA_ZONA)
                    {
                        dato.ID_ZONA = int.Parse(registro);
                        datRetorno.Dato = _usuario_rolRepository.Insert_usuario_rol_zona(dato);
                        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
                    }
                }
                tran.Complete();
            }

            return datRetorno;


	    }

	    public DatoRetorno<bool> Delete_usuario_rol(usuario_rol dato)
	    {
	        DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
	        datRetorno.Dato = _usuario_rolRepository.Delete_usuario_rol(dato);
	        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
	        return datRetorno;
	    }

        public DatoRetorno<usuario_rol> SelectAll_rol()
        {
            DatoRetorno<usuario_rol> datRetorno = new DatoRetorno<usuario_rol>();
            datRetorno.Lista = _usuario_rolRepository.SelectAll_rol();
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<JQGrid> SelectPaginated_usuario_rol_direccion(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _usuario_rolRepository.SelectPaginated_usuario_rol_direccion(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<JQGrid> SelectPaginated_usuario_rol_ejecutora(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _usuario_rolRepository.SelectPaginated_usuario_rol_ejecutora(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<JQGrid> SelectPaginated_usuario_rol_zona(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _usuario_rolRepository.SelectPaginated_usuario_rol_zona(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario_rol> SelectAll_zona_by_usuario(usuario_rol dato)
        {
            DatoRetorno<usuario_rol> datRetorno = new DatoRetorno<usuario_rol>();
            datRetorno.Lista = _usuario_rolRepository.SelectAll_zona_by_usuario(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario_rol> SelectAll_zona_by_usuario_ejecutora(usuario_rol dato)
        {
            DatoRetorno<usuario_rol> datRetorno = new DatoRetorno<usuario_rol>();
            datRetorno.Lista = _usuario_rolRepository.SelectAll_zona_by_usuario_ejecutora(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario_rol> SelectAll_ejecutora_by_usuario_zona_region(usuario_rol dato)
        {
            DatoRetorno<usuario_rol> datRetorno = new DatoRetorno<usuario_rol>();
            datRetorno.Lista = _usuario_rolRepository.SelectAll_ejecutora_by_usuario_zona_region(dato);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;
        }
	}
}
