using SCONT.Infraestructura.Transversal;
using System.Web;
using System.Web.Optimization;

namespace SCONT.Presentacion.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS style (bootstrap tuning for siper)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/plugins.css",
                      "~/Content/vendors.bundle.css",
                      "~/Content/main.css",
                      "~/Content/themes.css",
                      "~/Content/jquery-ui.min.css",
                      "~/Content/ui.jqgrid.css",
                     // "~/RibbonDev/css/Ribbon.css",
                      "~/Content/custom.css",
                      "~/Content/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                      "~/Scripts/vendor/modernizr-2.7.1-respond-1.4.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/toprint").Include(
                      "~/Scripts/print/printThis.js"));


            bundles.Add(new ScriptBundle("~/bundles/session").Include(
                      "~/Scripts/utils/jquery.blockUI.js",
                      "~/Scripts/utils/bootstrap-session-timeout.min.js",
                      "~/Scripts/utils/jquery.blockUI.start.js",
                      "~/Scripts/views/MenuMantenimiento.js"));

        }
    }
}
