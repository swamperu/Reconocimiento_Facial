using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.Mvc;
using MED.Security.API;
using MED.Security.API.Authorization;
using SCONT.Aplicacion.Contratos;

namespace SCONT.Presentacion.Web
{
    public class DIConfig
    {
        public static void Register()
        {
            //var host = new Uri(ConfigurationManager.AppSettings["SCONT.Host.Url"]);
            var binding = ConfigurationManager.AppSettings["SCONT.Host.Binding"];

            var builder = new ContainerBuilder();
            builder.RegisterModule<ServicesClass>();
            //ServiceProxy(builder, host, binding);

            /*begin:security*/
            builder.RegisterType<BasicAuthorization>().AsImplementedInterfaces();
            APIDependencyInjection.Resolve(builder);
            /*end:security*/

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired(); 

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        static void ServiceProxy(ContainerBuilder builder, Uri url, string binding)
        {
            const string eb = "endpointBinding"; //es el mismo nombre que el web.config
            /*
            <bindings>
                  <basicHttpBinding>
                    <binding name="endpointBinding">          
                    </binding>
                  </basicHttpBinding>
                  <customBinding>
                    <binding name="endpointBinding">        
                    </binding>
                  </customBinding>
            <bindings>	               
             */


            //builder.RegisterServiceProxy<IafpService>(url, "afpService.svc", binding, eb);
          
        }
    
    }
    public class ServicesClass : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("SCONT.Aplicacion.Servicios"))
                .Where(type => type.Name.EndsWith("Service", StringComparison.Ordinal))
                .AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(Assembly.Load("SCONT.Aplicacion.Legajos.Servicios"))
            //    .Where(type => type.Name.EndsWith("Service", StringComparison.Ordinal))
            //    .AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(Assembly.Load("SCONT.Aplicacion.Plazas.Servicios"))
            //   .Where(type => type.Name.EndsWith("Service", StringComparison.Ordinal))
            //   .AsImplementedInterfaces();
        }
    }
    public static class ContainerBuilderExtensions
    {
        public static IRegistrationBuilder<TChannel, SimpleActivatorData, SingleRegistrationStyle> RegisterServiceProxy<TChannel>(this ContainerBuilder builder, Uri baseUri, string relativeUri, string bindingName, string configurationName)
        {
            builder.Register(c => new ChannelFactory<TChannel>(MakeBinding(bindingName, configurationName), new EndpointAddress(new Uri(baseUri, relativeUri)))).SingleInstance();

            return builder.Register(c => c.Resolve<ChannelFactory<TChannel>>().CreateChannel()).InstancePerLifetimeScope();
            //return builder.Register(c => c.Resolve<ChannelFactory<TChannel>>().CreateChannel())
            //    .UseWcfSafeRelease()
            //    .InstancePerHttpRequest();
        }
        private static Binding MakeBinding(string binding, string configurationName)
        {
            switch (binding)
            {
                case "basicHttpBinding":
                    return string.IsNullOrEmpty(configurationName) ? new BasicHttpBinding() : new BasicHttpBinding(configurationName);
                    //break;
                case "customBinding":
                    return string.IsNullOrEmpty(configurationName) ? new CustomBinding() : new CustomBinding(configurationName);
                    //break;
                //TODO: Se comentó por que el Servicio Host no usa este tipo de binding en su configuracion, el Net.Tcp esta soportado en el "customBinding" 
                //case "netTcpBinding":
                //    return string.IsNullOrEmpty(configurationName) ? new NetTcpBinding() : new NetTcpBinding(configurationName);
                //    break;
                default:
                    return new BasicHttpBinding();
            }
        }
    }

}