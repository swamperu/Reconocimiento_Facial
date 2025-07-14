using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Custom;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Infraestructura.Transversal
{
    public static class sisVariable
    {
        public static bool IsEncrypt
        {
            get { return ConfigurationManager.AppSettings[sisConstante.Config_IsEncrypt] == "true"; }
        }
        public static string IsEncrypt_Clave
        {
            get { return ConfigurationManager.AppSettings[sisConstante.Config_IsEncrypt_Clave]; }
        }
        public static string Siscont_Version
        {
            get { return ConfigurationManager.AppSettings[sisConstante.Config_Siscont_Version]; }
        }
        public static string Siscont_Codigo
        {
            get { return ConfigurationManager.AppSettings[sisConstante.Config_Sistema_Codigo]; }
        }
        public static string Si
        {
            get { return ConfigurationManager.AppSettings[sisConstante.Config_Sistema_Codigo]; }
        }
        
        public static double Token_time
        {
            get
            {
                double result = 30;
                var data = ConfigurationManager.AppSettings[sisConstante.Token_time];

                if (!string.IsNullOrEmpty(data))
                    result = Convert.ToDouble(data);

                return result;
            }
        }

        public static string SCONT_NOMBRE_CARPETA_PREVISION { get; set; }
        public static string SCONT_NOMBRE_ARCHIVO_PREVISION { get; set; }
        public static string SCONT_ARCHIVO_MAPA_SITUACIONAL { get; set; }
        public static int SCONT_ID_CUADRO_DISTRIBUCION { get; set; }
        public static int SCONT_ID_MODALIDAD { get; set; }
        public static int SCONT_ID_NIVEL_EDUCATIVO { get; set; }
        public static int SCONT_ID_GRADO { get; set; }
        public static int SCONT_ID_AREA_CURRICULAR { get; set; }
        public static int SCONT_ID_BENEFICIARIO { get; set; }
        public static int SCONT_ID_TIPO_ELEMENTO { get; set; }
        public static int SCONT_ID_TIPO_MATERIAL { get; set; }
        public static int SCONT_ID_PERIODO_VIDA { get; set; }
        public static int SCONT_ID_UNIDAD_MEDIDA { get; set; }
        public static int SCONT_ID_LENGUA { get; set; }
        public static int SCONT_ID_DIRECCION { get; set; }
        public static int SCONT_ID_DOTACION { get; set; }
        public static int SCONT_ID_GRUPO { get; set; }
        public static int SCONT_ID_PRODUCTO { get; set; }
        public static int SCONT_ESTADO_PROCESO { get; set; }
        public static int SCONT_ID_UNIDAD_GESTION_EDUCATIVA { get; set; }
        public static string SCONT_ESTADO_CDD { get; set; }
        public static string SCONT_SEMANA { get; set; }
        public static bool SCONT_REPORTE_PECOSA_CENTRO_COSTO_DETALLE { get; set; }
        public static Nullable<System.DateTime> SCONT_FECHA_INICIO { get; set; }
        public static Nullable<System.DateTime> SCONT_FECHA_FIN { get; set; }

        public static object Parametro_LegajoPersonal { get; set; }
        public static DataTable Lista_Pecosas { get; set; }
        public static DataTable Lista_Pecosas_Centro_Costo { get; set; }
        public static DataTable Lista_Ingresos_Programados { get; set; }
        public static DataTable Lista_Programar_Distribucion { get; set; }
    }
}
