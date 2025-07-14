using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    /// <summary>
    /// Nombre       : PARAMETRO
    /// Descripción  : Métodos de trabajo para PARAMETRO
    /// </summary>
    /// <remarks>
    /// Creacion     : 09/05/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract()]
    public class Parametro : BaseEntitity
    {
        [DataMember()]
        public string PARAMETRO1 { get; set; }
        [DataMember()]
        public string PARAMETRO2 { get; set; }
        [DataMember()]
        public string PARAMETRO3 { get; set; }
        [DataMember()]
        public string PARAMETRO4 { get; set; }
        [DataMember()]
        public string PARAMETRO5 { get; set; }
        [DataMember()]
        public string PARAMETRO6 { get; set; }
        [DataMember()]
        public string PARAMETRO7 { get; set; }
        [DataMember()]
        public string PARAMETRO8 { get; set; }
        [DataMember()]
        public string PARAMETRO9 { get; set; }
        [DataMember()]
        public string PARAMETRO10 { get; set; }
        [DataMember()]
        public string PARAMETRO11 { get; set; }
        [DataMember()]
        public string PARAMETRO12 { get; set; }
        [DataMember()]
        public string PARAMETRO13 { get; set; }
        [DataMember()]
        public string PARAMETRO14 { get; set; }
        [DataMember()]
        public string PARAMETRO15 { get; set; }
        [DataMember()]
        public string NOMBRE_UNIDAD_EJECUTORA { get; set; }

        [DataMember()]
        public string PALABRA_CLAVE { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_1 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_2 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_3 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_4 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_5 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_6 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_7 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_8 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_9 { get; set; }
        [DataMember()]
        public string PALABRA_CLAVE_10 { get; set; }

        private string m_FECHA_DESDE;
        /// <summary>
        /// Rango de Fecha inicio (ingresar en formato dd/mm/yyyy)
        /// </summary>
        [DataMember()]
        public string FECHA_DESDE
        {
            get { return m_FECHA_DESDE; }
            set { m_FECHA_DESDE = value; }
        }
        private string m_FECHA_HASTA;
        /// <summary>
        /// Rango de Fecha fin (ingresa en formato dd/mm/yyyy)
        /// </summary>
        [DataMember()]
        public string FECHA_HASTA
        {
            get { return m_FECHA_HASTA; }
            set { m_FECHA_HASTA = value; }
        }
        /// <summary>
        /// Devuelve Rango de Fecha inicio en formato YYYYMMDD
        /// </summary>
        [DataMember()]
        public string FECHA_DESDE_YYYYMMDD
        {
            get { return String.Format("{0:yyyyMMdd}", Convert.ToDateTime(m_FECHA_DESDE)); }
        }

        /// <summary>
        /// Devuelve Rango de Fecha fin en formato YYYYMMDD
        /// </summary>
        [DataMember()]
        public string FECHA_HASTA_YYYYMMDD
        {
            get { return String.Format("{0:yyyyMMdd}", Convert.ToDateTime(m_FECHA_HASTA)); }
        }

               [DataMember()]
        public string SORT_COLUMN { get; set; }
        [DataMember()]
        public string SORT_ORDER { get; set; }
        [DataMember()]
        public int PAGE_SIZE { get; set; }
        [DataMember()]
        public int PAGE_NUMBER { get; set; }

        [DataMember()]
        public ArrayList PARAMETRO_LISTA1 { get; set; }
        [DataMember()]
        public ArrayList PARAMETRO_LISTA2 { get; set; }
        [DataMember()]
        public ArrayList PARAMETRO_LISTA3 { get; set; }

        [DataMember()]
        public Nullable<System.DateTime> PARAMETRO_FECHA { get; set; }
        [DataMember()]
        public Nullable<System.DateTime> FECHA_SERVIDOR { get; set; }
    }
    [DataContract()]
    public class Parametro<T> : BaseEntitity
    {
        [DataMember]
        public string PARAMETRO1 { get; set; }
        [DataMember]
        public T DATA { get; set; }
        [DataMember]
        public string USUARIO_CREACION { get; set; }

        [DataMember()]
        public string SORT_COLUMN { get; set; }
        [DataMember()]
        public string SORT_ORDER { get; set; }
        [DataMember()]
        public int PAGE_SIZE { get; set; }
        [DataMember()]
        public int PAGE_NUMBER { get; set; }

    }
}
