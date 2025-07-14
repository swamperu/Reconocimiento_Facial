using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCONT.Presentacion.Web.Startup))]
namespace SCONT.Presentacion.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
