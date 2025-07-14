using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{  
    [DataContract]
    public class TemplateColumnsExport : BaseEntitity
    {
        public string Name { get; set; }
        public string Field { get; set; }
        public string Format { get; set; }
        public int Width { get; set; }
    }
}
