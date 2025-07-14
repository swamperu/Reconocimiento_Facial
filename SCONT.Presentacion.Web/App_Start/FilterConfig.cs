using SCONT.Presentacion.Web.Helpers;
using System.Web;
using System.Web.Mvc;

namespace SCONT.Presentacion.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HelperActionFilterAttribute());
        }
    }
}
