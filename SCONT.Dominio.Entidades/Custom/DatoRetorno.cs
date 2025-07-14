using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCONT.Dominio.Entidades.Custom
{
    /// <summary>
    /// Nombre       : DatoRetorno
    /// Descripción  : Métodos de trabajo para DatoRetorno
    /// </summary>
    /// <remarks>
    /// Creacion     : 09/05/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract]
    public class DatoRetorno<T>
    {
        [DataMember]
        public string Msg { get; set; }
        [DataMember]
        public IEnumerable<string> MsgList { get; set; }
        [DataMember]
        public T Dato { get; set; }
        [DataMember]
        public List<T> Lista { get; set; }
        [DataMember]
        public bool Autorizado { get; set; }
        [DataMember]
        public bool Success { get; set; }
        public DatoRetorno()
        {
            Autorizado = true;
            Success = true;
        }
        //public DatoRetorno(T DatoDeRetorno, string Mensaje, int RetornoAutorizado)
        //{
        //    Msg = Mensaje;
        //    Dato = DatoDeRetorno;
        //    Autorizado = RetornoAutorizado;
        //}

    }
}
