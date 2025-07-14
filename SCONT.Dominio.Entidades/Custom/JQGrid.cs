using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    /// <summary>
    /// Nombre       : JQGrid
    /// Descripción  : Métodos de trabajo para JQGrid
    /// </summary>
    /// <remarks>
    /// Creacion     : 09/05/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    [DataContract()]
    public class JQGrid
    {

        [DataContract()]
        public class Row
        {
            [DataMember()]
            public int id { get; set; }
            [DataMember()]
            public List<string> cell { get; set; }

            public Row()
            {
                cell = new List<string>();
            }
            public Row(int pId, List<string> pRow)
            {
                id = pId;
                cell = pRow;
            }

        }

        public JQGrid()
        {
            rows = new List<Row>();
        }

        [DataMember()]
        public int page { get; set; }
        [DataMember()]
        public int total { get; set; }
        [DataMember()]
        public int records { get; set; }
        [DataMember()]
        public List<Row> rows { get; set; }
        [DataMember()]
        public IEnumerable<string> colModel { get; set; }
    }
}
