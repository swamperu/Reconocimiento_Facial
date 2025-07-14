using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SCONT.Presentacion.Web
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass)) 
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        //public static string MenuMED(HtmlHelper helper, object listaRecursos)
        //{
        //    //string writer = new String();

        //    //if ((listaRecursos == null))
        //    //{
        //    //    return MvcHtmlString.Create(writer.InnerWriter.ToString);
        //    //}

        //    //writer.WriteEndTag("ul");

        //    //writer.WriteEndTag("div");

        //    //return MvcHtmlString.Create(writer.InnerWriter.ToString);
        //}
	}
}
