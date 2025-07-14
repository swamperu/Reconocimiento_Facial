using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCONT.Presentacion.Web
{
    public class MvcRegisterConfig
    {
        public static void Initialize()
        {
            DynamicModuleUtility.RegisterModule(typeof(MvcModuleConfig));
        } 
    }
}