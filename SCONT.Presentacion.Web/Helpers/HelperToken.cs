using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCONT.Presentacion.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HelperTokenRequiredAttribute : Attribute
    {

    }
}