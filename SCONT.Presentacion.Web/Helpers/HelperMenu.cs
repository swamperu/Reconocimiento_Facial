using MED.Security.API.AuthorizationService;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Sistema;
using SCONT.Infraestructura.Transversal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Helpers
{
    public static class MEDHelpers
    {

       
        public static MvcHtmlString MEDMenu(List<recurso> listaRecursos)
        {
            //string cadena = "desarrollo$$pronied";
            //string modulo = ConfigurationManager.AppSettings["CodigoModulo"];
            usuario datUsuario = new usuario();
            datUsuario = (usuario)sisSesion.Usuario;

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            if ((listaRecursos == null))
            {
                return MvcHtmlString.Create(writer.InnerWriter.ToString());
            }

            List<recurso> listaMenuNivel1 = (from x in listaRecursos where x.TIPO.Equals("T") orderby x.ORDEN select x).ToList();

            if ((listaMenuNivel1.Count() == 0))
            {
                return MvcHtmlString.Create(writer.InnerWriter.ToString());
            }

            foreach (recurso itemNivel1 in listaMenuNivel1)
            {
                List<recurso> listaMenuNivel2 = (from x in listaRecursos where x.RELACION.Equals(itemNivel1.CODIGO) orderby x.ORDEN select x).ToList();
                if (listaMenuNivel2.Count() > 0)
                {
                    writer.WriteBeginTag("li");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    //--------------------------
                    writer.WriteBeginTag("a");
                    writer.WriteAttribute("id", itemNivel1.CODIGO);
                    //writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteAttribute("href", "#");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteBeginTag("i");
                    writer.WriteAttribute("class", GetIconForMenu(itemNivel1.CODIGO)); //imagen
                    //writer.WriteAttribute("class", "fa fa-search");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("i");
                    writer.WriteBeginTag("span");
                    writer.WriteAttribute("class", "nav-label");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(itemNivel1.NOMBRE);
                    writer.WriteEndTag("span");
                    writer.WriteBeginTag("span");
                    writer.WriteAttribute("class", "fa arrow");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("span");
                    writer.WriteEndTag("a");
                    //--------------------------
                    writer.WriteBeginTag("ul");
                    writer.WriteAttribute("class", "nav nav-second-level collapse");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    foreach (recurso itemNivel2 in listaMenuNivel2)
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteBeginTag("a");
                        writer.WriteAttribute("id", itemNivel2.CODIGO);
                        writer.WriteAttribute("href", VirtualPathUtility.ToAbsolute("~/" + itemNivel2.URL));
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteBeginTag("i");
                        writer.WriteAttribute("class", "fa fa-angle-double-right");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteEndTag("i");
                        writer.Write(itemNivel2.NOMBRE);
                        writer.WriteEndTag("a");
                        writer.WriteEndTag("li");
                    }
                    writer.WriteEndTag("ul");
                    writer.WriteEndTag("li");
                }

            }


            return MvcHtmlString.Create(writer.InnerWriter.ToString());
        }

        private static string GetIconForMenu(string codMenu)
        {
            string classIcon = "";

            switch (codMenu)
            {
                case "0100":
                    classIcon = "fa fa-clipboard";
                    break;
                case "0200":
                    classIcon = "fa fa-file-text-o";
                    break;
                case "0300":
                    classIcon = "fa fa-cog";
                    break;
                case "0400":
                    classIcon = "fa fa-handshake-o";
                    break;
                case "0500":
                    classIcon = "fa fa-cog";
                    break;
                default:
                    classIcon = "";
                    break;
            }
            return classIcon;
        }

        private static recurso GetRecursoForMenu(string codRecursoPassport)
        {
            List<recurso> listaRecursos = (List<recurso>)sisSesion.Menus;
            recurso datRecurso = (from x in listaRecursos where x.CODIGO.Equals(codRecursoPassport) select x).SingleOrDefault();
            return datRecurso;
        }

    }
}