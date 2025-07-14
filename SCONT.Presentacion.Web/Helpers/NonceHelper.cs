using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCONT.Presentacion.Web
{
    public static  class NonceHelper
    {
        public static IHtmlString ScriptNonce(this HtmlHelper helper)
        {
            var owinContext = helper.ViewContext.HttpContext.GetOwinContext();
            return new HtmlString(owinContext.Get<string>("ScriptNonce"));
        }
    }
}