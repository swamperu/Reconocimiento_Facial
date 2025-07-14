using SCONT.Dominio.Entidades.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SCONT.Presentacion.Web.Helpers
{
    public static class MEDMapper
    {
        /// <summary>
        /// Crea un objeto DatoRetorno con la propiedad Dato de tipo anónimo.
        /// Se excluyen las propiedades de estado y auditoría.
        /// </summary>
        /// <param name="obj">Objeto a llenar</param>
        /// <returns>
        /// Retorna un objeto DatoRetorno, donde la propiedad Dato es un string en formato json que representa al objeto anonimo.
        /// Tener en cuenta que esa propiedad deberá ser casteada en el cliente de la siguiente forma: JSON.parse(objeto.Dato)
        /// De esa forma se obtendrá el objeto en el cliente.
        /// </returns>
        public static DatoRetorno<object> DatoRetornoToAnonymous<T>(DatoRetorno<T> obj)
        {
            Type type = typeof(T);
            int indice = 0;
            
            dynamic objAnonymous = new Newtonsoft.Json.Linq.JObject();
            string valor;

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                switch (pi.Name.ToUpper())
                {
                    case "ESTADO":
                    case "USUARIO_CREACION":
                    case "FECHA_CREACION":
                    case "USUARIO_MODIFICACION":
                    case "FECHA_MODIFICACION":
                    case "TOKEN":
                        break;
                    default:

                        if (pi.PropertyType.FullName.ToUpper().IndexOf("DATE") > 0)
                        {
                            valor = "";
                            if (pi.GetValue(obj.Dato, null) != null) valor = ((DateTime)pi.GetValue(obj.Dato, null)).ToShortDateString();
                            objAnonymous.Add(pi.Name, valor);
                        }
                        else
                        {
                            valor = "";
                            if (pi.GetValue(obj.Dato, null) != null) valor = pi.GetValue(obj.Dato, null).ToString();
                            objAnonymous.Add(pi.Name, valor);
                        }


                        break;
                }
                indice += 1;
            }

            DatoRetorno<object> dato = new DatoRetorno<object>();
            if (obj != null)
            {
                dato.Autorizado = obj.Autorizado;
                dato.Msg = obj.Msg;
                dato.Dato = objAnonymous; //.ToString();
            }

            return dato;
        }
    }
}