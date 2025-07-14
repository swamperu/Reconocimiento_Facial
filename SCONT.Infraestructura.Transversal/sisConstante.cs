using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Infraestructura.Transversal
{
    public static class sisConstante
    {
        public const string Session_Token = "MED_Session_Token";
        public const string Session_Usuario = "MED_Session_Usuario";
        public const string Session_Entidad = "MED_Session_Entidad";
        public const string Session_Menus = "MED_Session_Menus";
        public const string Session_Forms = "MED_Session_Forms";
        public const string Session_Methods = "MED_Session_Metodos";
        public const string Session_Resources = "MED_Session_Recursos";
        public const string Session_Roles = "MED_Session_Rol";
        public const string Session_RolPrincipal = "MED_Session_Rol_Principal";

        public const string Session_Rol_Sistema = "RHASI00001";
        public const string Session_Reporte_Parametro = "MED_Session_Reporte_Prametro";
        public const string Session_Lista_Reporte = "MED_Session_Lista_Reporte";
        public const string Session_Lista_Data = "MED_Session_Lista_Data";
        public const string Session_Lista_Adelanto = "MED_Session_Lista_Adelanto";
        public const string Session_Lista_Incidencia_Armada_Item = "MED_Session_Lista_Incidencia_Armada_Item";
        public const string Session_Lista_Detalle = "MED_Session_Lista_Detalle";
        public const string Session_Lista_Detalle2 = "MED_Session_Lista_Detalle2";
        public const string Session_Codigo = "MED_Session_Codigo";
        public const string Session_Codigo_2 = "MED_Session_Codigo_2";
        public const string Sesion_Tab_Seleccionado = "MED_Sesion_Tab_Seleccionado";

        public const string Config_Dominio_Url = "SCONT.Host.Url";
        public const string Config_IsEncrypt = "SCONT.Encrypt";
        public const string Config_IsEncrypt_Clave = "SCONT.Encrypt.Clave";
        public const string Config_Siscont_Version = "SCONT.Version";
        public const string Config_Sistema_Codigo = "CodigoSistema";

        public const string Url_Acceso_Denegado = "~/Error/AccesoDenegado";
       
       
        public const string Token_time = "TokenTime";

        public const int Estado_Activo= 0;
        public const int Estado_Inactivo = 1;
        public const int Estado_Eliminado = 2;

        
        public const int Id_Pliego_MINEDU = 1;
        public const string SIAF_EJECUTORA_ALMACEN = "120";

        public const string SIAF_SEC_EJEC = "1476";


        public const int Tabla_General_Rol_Usuario = 1;
        public const int Tabla_General_Genero = 2;
        public const int Tabla_General_Tipo_Marcacion = 3;
        public const int Tabla_General_Horario_Laboral = 4;


        public static class Rol_Usuario
        {
            public const string Administrador = "1";
        }

        public static class Parametro_General
        {
            public const string Imprimir_Orden = "IMP_ORDEN";
            public const string Nombre_Decenio = "NOM_DECENIO";
            public const string Nombre_Anio = "NOM_ANIO";
            public const string Nombre_Anio_Bicentenario = "NOM_ANIO_BICENTENARIO";
            public const string Nombre_Coordinador_EC = "NOM_COORDINADOR_EC";
            public const string Nombre_Coordinador_AM = "NOM_COORDINADOR_AM";
        }

    }
}
