using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace SCONT.Presentacion.Web.Helpers
{
    public class HelperSecurity : Page
    {
        public string EncriptarQueryString(params string[] Parametros)
        {
            TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 });
            for (int i = 0; i < Parametros.Length; i++)
            {
                QueryString[i.ToString()] = Parametros[i];
            }

            return HttpUtility.UrlEncode(QueryString.ToString());

        }

        public string DesencriptarQueryString(string ParametroQueryString)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 }, Request[ParametroQueryString]);
            for (int i = 0; i < QueryString.Count; i++)
            {
                oStringBuilder.Append(QueryString[i]);
                if (i < QueryString.Count - 1)
                    oStringBuilder.Append(";");
            }
            return oStringBuilder.ToString();
        }

        public string DesencriptarQueryStringEncriptado(string Encriptado)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            TSHAK.Components.SecureQueryString QueryString = new TSHAK.Components.SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 }, Encriptado);
            for (int i = 0; i < QueryString.Count; i++)
            {
                oStringBuilder.Append(QueryString[i]);
                if (i < QueryString.Count - 1)
                    oStringBuilder.Append(";");
            }
            return oStringBuilder.ToString();
        }
    }
}