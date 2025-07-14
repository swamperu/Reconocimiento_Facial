using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.UI;

namespace System.Web.Mvc.Html
{
    public static class MvcHtmlExtensions
    {
        #region HelpersWithModel
        
        public static MvcHtmlString MEDTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
                                                                     Expression<Func<TModel, TProperty>> expression, 
                                                                     IDictionary<string, object> htmlAttributes)
        {
            // nombre del campo
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            // atributos completos del campo
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            // obtiene el nombre del campo completo
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            return BuildTextBox(fieldName, fullName, "", true, metadata, htmlAttributes, false);
        }

        public static MvcHtmlString MEDTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
                                                                     Expression<Func<TModel, TProperty>> expression,
                                                                     IDictionary<string, object> htmlAttributes,
                                                                     ButtonAddons buttonPosition,
                                                                     string textOfButton)
        {
            // nombre del campo
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            // atributos completos del campo
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            // obtiene el nombre del campo completo
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            return BuildTextBox(fieldName, fullName, "", true, metadata, htmlAttributes, true, buttonPosition, textOfButton);
        }
        public static MvcHtmlString MEDPickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                   Expression<Func<TModel, TProperty>> expression,
                                                                   IDictionary<string, object> htmlAttributes)
        {
            // nombre del campo
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            // atributos completos del campo
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            // obtiene el nombre del campo completo
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);

            return BuildPicker(fullName, "", true, metadata, htmlAttributes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">Model enlazado al control</param>
        /// <param name="selectList">SelectList que contiene la relación de elementos a mostrarse en el control</param>
        /// <param name="htmlAttributes">Lista de atributos que se establecen para el control</param>
        /// <returns></returns>
        public static MvcHtmlString MEDDropDownList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                   Expression<Func<TModel, TProperty>> expression,
                                                                   IEnumerable<SelectListItem> selectList, 
                                                                   IDictionary<string, object> htmlAttributes)
        {
            // nombre del campo
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            // atributos completos del campo
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            // obtiene el nombre del campo completo
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            //valor
            string value = "";
            if (metadata.Model != null) value = metadata.Model.ToString();

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            //construye label
            writer.WriteBeginTag("label");
            writer.WriteAttribute("class", "control-label");
            writer.WriteAttribute("for", "txt" + fullName.Replace(".", "_"));
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(metadata.DisplayName);
            writer.WriteEndTag("label");

            //construye select
            writer.WriteBeginTag("select");
            writer.WriteAttribute("class", "form-control input-sm");
            writer.WriteAttribute("id", "ddl" + fullName.Replace(".", "_"));
            writer.WriteAttribute("name", "ddl" + fullName.Replace(".", "_"));

            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> item in htmlAttributes)
                {
                    writer.WriteAttribute(item.Key, (string)item.Value);
                }
            }

            writer.Write(HtmlTextWriter.TagRightChar);

            if (selectList != null)
            {
                foreach (SelectListItem item in selectList)
                {
                    writer.WriteLine("<option value='" + item.Value + "' " + (item.Value.Equals(value) ? "selected" : "") + ">" + item.Text + "</option>");
                }
            }

            writer.WriteEndTag("select");

            return MvcHtmlString.Create(writer.InnerWriter.ToString());
        }

        #endregion

        #region HelpersWithoutModel
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="id">Nombre base para el control, el helper adicionará los prefijos necesarios para los componentes del control en base al ID que usted proporcione</param>
        /// <param name="displayName">Nombre del label que acompaña al control</param>        
        public static MvcHtmlString MEDTextBox<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                       string id,
                                                       string displayName)
        {
            return BuildTextBox(id, id, displayName, false, null, null, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="id">Nombre base para el control, el helper adicionará los prefijos necesarios para los componentes del control en base al ID que usted proporcione</param>
        /// <param name="displayName">Nombre del label que acompaña al control</param>   
        public static MvcHtmlString MEDTextBox<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                       string id,
                                                       string displayName,
                                                       ButtonAddons buttonPosition = 0,
                                                       string textOfButton = null)
        {
            return BuildTextBox(id, id, displayName, false, null, null, true, buttonPosition, textOfButton);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="id">Nombre base para el control, el helper adicionará los prefijos necesarios para los componentes del control en base al ID que usted proporcione</param>
        /// <param name="displayName">Nombre del label que acompaña al control</param>
        /// <param name="htmlAttributes">Lista de atributos que se establecen para el control</param>
        /// <returns></returns>
        public static MvcHtmlString MEDPicker<TModel>(this HtmlHelper<TModel> htmlHelper,
                                                      string id,
                                                      string displayName,
                                                      IDictionary<string, object> htmlAttributes,
                                                      bool es_requerido = false)
        {
            return BuildPicker(id, displayName, false, null, htmlAttributes, es_requerido);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id">Nombre base para el control, el helper adicionará los prefijos necesarios para los componentes del control en base al ID que usted proporcione</param>
        /// <returns></returns>
        public static MvcHtmlString MEDJQGrid(this HtmlHelper html, 
                                              string id) 
        {
            return BuildJQGrid(id, false, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="id">Nombre base para el control, el helpera adicionará los prefijos necesarios para los componentes del control en base al ID que usted proporcione</param>
        /// <param name="buttons">Lista de botones que incluirá el control segmentado de la siguiente forma: ID,CAPTION,ICON</param>
        /// <returns></returns>
        public static MvcHtmlString MEDJQGrid(this HtmlHelper html, 
                                              string id,
                                              List<string> buttons)
        {
            return BuildJQGrid(id, true, buttons);
        }
        #endregion

        #region HelpersBuild
        private static MvcHtmlString BuildTextBox(string fieldName,
                                                  string fullName,
                                                  string displayName,
                                                  bool withMetadata,
                                                  ModelMetadata metadata,
                                                  IDictionary<string, object> htmlAttributes,
                                                  bool withButtonAddons,
                                                  ButtonAddons buttonPosition = 0,
                                                  string textOfButton = null)
        {
            //valor
            string value = "";
            if (withMetadata)
            {
                if (metadata.Model != null) value = metadata.Model.ToString();
            }

            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            //construye label
            writer.WriteBeginTag("label");
            writer.WriteAttribute("class", "control-label");
            writer.WriteAttribute("for", "txt" + fullName.Replace(".", "_"));
            writer.Write(HtmlTextWriter.TagRightChar);
            if (withMetadata)
            {
                writer.Write(metadata.DisplayName);
            }
            else
            {
                writer.Write(displayName);
            }
            writer.WriteEndTag("label");

            //construye boton addons izquierda
            if (withButtonAddons)
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", "input-group m-b");
                writer.Write(HtmlTextWriter.TagRightChar);

                if (buttonPosition == ButtonAddons.Izquierda)
                {
                    writer.WriteBeginTag("span");
                    writer.WriteAttribute("class", "input-group-btn");
                    writer.WriteAttribute("for", "btn" + fullName.Replace(".", "_"));
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("button");
                    writer.WriteAttribute("class", "btn btn-danger btn-sm");
                    writer.WriteAttribute("id", "btn" + fullName.Replace(".", "_"));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(textOfButton);
                    writer.WriteEndTag("button");

                    writer.WriteEndTag("span");
                }
            }

            //construye textbox
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "text");
            writer.WriteAttribute("class", "form-control input-sm");
            writer.WriteAttribute("id", "txt" + fullName.Replace(".", "_"));
            writer.WriteAttribute("name", "txt" + fullName.Replace(".", "_"));
            if (value != "") writer.WriteAttribute("value", value);

            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> item in htmlAttributes)
                {
                    writer.WriteAttribute(item.Key, (string)item.Value);
                }
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("input");

            //construye boton addons derecha
            if (withButtonAddons)
            {
                if (buttonPosition == ButtonAddons.Derecha)
                {
                    writer.WriteBeginTag("span");
                    writer.WriteAttribute("class", "input-group-btn");
                    writer.WriteAttribute("for", "btn" + fullName.Replace(".", "_"));
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("button");
                    writer.WriteAttribute("class", "btn btn-danger btn-sm");
                    writer.WriteAttribute("id", "btn" + fullName.Replace(".", "_"));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(textOfButton);
                    writer.WriteEndTag("button");
                    
                    writer.WriteEndTag("span");
                }
                writer.WriteEndTag("div");
            }


            return MvcHtmlString.Create(writer.InnerWriter.ToString());
        }
        private static MvcHtmlString BuildPicker(string fullName,
                                                 string displayName,
                                                 bool withMetadata,
                                                 ModelMetadata metadata,
                                                 IDictionary<string, object> htmlAttributes,
                                                 bool es_requerido = false)
        {
            //valor
            string value = "";
            if (withMetadata)
            {
                if (metadata.Model != null) value = metadata.Model.ToString();
            }

             HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            //construye label
            writer.WriteBeginTag("label");
            writer.WriteAttribute("class", "control-label");
            writer.WriteAttribute("for", "txt" + fullName.Replace(".", "_"));
            writer.Write(HtmlTextWriter.TagRightChar);
            if (withMetadata)
            {
                writer.Write(metadata.DisplayName);
            }
            else
            {
                writer.Write(displayName);
            }
            writer.WriteEndTag("label");

            if (es_requerido)
            {
                writer.WriteBeginTag("span");
                writer.WriteAttribute("class", "requerido");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(" (*)");
                writer.WriteEndTag("span");
            }

            //construye picker
            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "input-group date");
            writer.Write(HtmlTextWriter.TagRightChar);

            //ini construye textbox
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "text");
            writer.WriteAttribute("class", "form-control input-sm");
            writer.WriteAttribute("data-mask", "99/99/9999");
            writer.WriteAttribute("id", "txt" + fullName.Replace(".", "_"));
            writer.WriteAttribute("name", "txt" + fullName.Replace(".", "_"));
            if (value != "") writer.WriteAttribute("value", value);

            if (htmlAttributes != null) {
                foreach (KeyValuePair<string, object> item in htmlAttributes)
                {
                    writer.WriteAttribute(item.Key, (string)item.Value);
                }
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("input");
            //fin construye textbox

            //ini boton
            writer.WriteBeginTag("span");
            writer.WriteAttribute("class", "input-group-addon");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("i");
            writer.WriteAttribute("class", "fa fa-calendar");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("i");

            writer.WriteEndTag("span");
            //fin boton

            writer.WriteEndTag("div");

            return MvcHtmlString.Create(writer.InnerWriter.ToString());
        }
        private static MvcHtmlString BuildJQGrid(string id,
                                                 bool withButtons,
                                                 List<string> buttons)
        {
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

            //
            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "jqGrid_wrapper");
            writer.WriteAttribute("id", "cnt" + id);
            writer.Write(HtmlTextWriter.TagRightChar);

            if (withButtons)
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("id", "toolBar" + id);
                writer.WriteAttribute("class", "area-tool");
                writer.WriteAttribute("style", "display: block;");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (string item in buttons)
                {
                    string[] properties = item.Split(',');
                    writer.WriteBeginTag("button");
                    writer.WriteAttribute("id", "btn" + id + "_" + properties[0]);
                    writer.WriteAttribute("class", "btn btn-sm btn-tool");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("span");
                    writer.WriteAttribute("style", "float: left;");
                    writer.WriteAttribute("class", "ui-icon ui-icon-" + properties[2]);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("span");

                    writer.WriteBeginTag("span");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(properties[1]);
                    writer.WriteEndTag("span");

                    writer.WriteEndTag("button");

                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", "separadorgrilla");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteEndTag("div");
                }

                writer.WriteEndTag("div");
            }

            writer.WriteBeginTag("table");
            writer.WriteAttribute("id", "tbl" + id);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("table");

            writer.WriteBeginTag("div");
            writer.WriteAttribute("id", "pag" + id);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("div");

            writer.WriteEndTag("div");

            return MvcHtmlString.Create(writer.InnerWriter.ToString());
        }        
        #endregion
    }

    public enum ButtonAddons
	{
        Izquierda = 1,
        Derecha = 2
	}
}