using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Dominio.Entidades.Custom
{
    public class TableExcel
    {
        private StringBuilder m_Content = new StringBuilder();
        private StringBuilder m_ContentAux = new StringBuilder();
        private List<StringBuilder> m_ContentList = new List<StringBuilder>();
        //private int m_nroCharacters = new int();

        public List<StringBuilder> ContentList()
        {
            return m_ContentList;
        }


        public string filename { get; set; }
        public byte[] bytes { get; set; }

        public int rows { get; set; }
        public int page { get; set; }
        public int total { get; set; }
        public int records { get; set; }

        public void Add(List<string> pRow)
        {
            rows += 1;
            int nroCol = pRow.Count();
            int nroAva = 0;
            string cadRow = "";

            while (nroAva < nroCol)
            {
                cadRow = cadRow + String.Format("{0},", pRow[nroAva].ToString() );
                nroAva += 1;
            }

            if (m_ContentList.Count() == 0)
            {
                m_ContentList.Add(m_Content);
            }

            m_ContentAux.Clear();
            m_ContentAux.Append(cadRow);

            if ((m_Content.Length + m_ContentAux.Length) > Int16.MaxValue)
            {
                m_Content = new StringBuilder();
                rows = 1;

                m_ContentList.Add(m_Content);
                m_Content.AppendLine(cadRow);
            }
            else
            {
                m_Content.AppendLine(cadRow);
            }
        }

    }
}
