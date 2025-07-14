using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Infraestructura.Transversal
{
    public static class sisEnum
    {
        public enum RolSistema
        {
            Administrador = 79,
            UsuarioControlPlazas = 2,
            UsuarioConsultaControlPlazas = 3,
            UsuarioLegajos = 4,
            UsuarioConsultaLegajos = 5,
            UsuarioSUP = 6,
            UsuarioACM = 7,
            UsuarioTecnicoPlanillas = 8,
            UsuarioJefeEscalafon = 9,
            MasterLocal = 10
        }
        public enum RetornoAutorizado
        {
            NoAutorizado = 0,
            Autorizado = 1
        }
        public enum EstadoRegistro
        {
            Activo= 0,
            Inactivo = 1,
            Eliminado = 2
        }

        public enum EtapaProceso
        { 
            Programacion = 1,
            Seguimiento = 2
        }
    }
}
