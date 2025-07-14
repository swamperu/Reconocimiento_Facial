using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades
{
    [DataContract()]
    public class BaseEntitity
    {
        [DataMember()]
        public string TOKEN { get; set; }
    }
}
