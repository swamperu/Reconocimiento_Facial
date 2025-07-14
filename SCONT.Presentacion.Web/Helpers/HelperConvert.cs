using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SCONT.Presentacion.Web.Helpers
{
    public static class HelperConvert
    {
        public static object SelectListAddItem<T>(List<T> lista, string dataValueField, string dataTextField, string Value, string Text, string SelectedValue = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            if ((lista != null))
            {
                items = new SelectList(lista, dataValueField, dataTextField).ToList();
            }

            if (Text != "")
            {
                items.Insert(0, (new SelectListItem
                {
                    Text = Text,
                    Value = Value
                }));
            }

            if (SelectedValue != null)
                return new SelectList(items, "Value", "Text", SelectedValue);
            else
                return new SelectList(items, "Value", "Text");
        }

        public static ExpandoObject convertToAnonymous(object obj)
        {
            //obtiene las propiedades con Reflection
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            PropertyInfo[] properties = obj.GetType().GetProperties(flags);

            //agregar propiedad a Expando excepto los campos de auditoria
            ExpandoObject expando = new ExpandoObject();
            foreach (PropertyInfo property in properties)
            {
                switch (property.Name.ToUpper())
                {
                    case "ESTADO_REGISTRO":
                    case "USUARIO_CREACION":
                    case "FECHA_CREACION":
                    case "USUARIO_MODIFICACION":
                    case "FECHA_MODIFICACION":
                        AddProperty(expando, property.Name, property.GetValue(obj));
                        break;
                    default:
                        break;
                }
            }

            return expando;
        }

        private static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, Object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        //public static string GetImageToBase64String(string pathImage)
        //{
        //    var result = "";

        //    if (File.Exists(pathImage))
        //    {
        //        var bitmap = new Bitmap(pathImage);
        //        var bitmapBytes = BitmapToBytes(bitmap);
        //        result = Convert.ToBase64String(bitmapBytes);
        //    }
           
        //    return result;
        //}
        //private static byte[] BitmapToBytes(Bitmap img)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
                
        //        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}

    }


}