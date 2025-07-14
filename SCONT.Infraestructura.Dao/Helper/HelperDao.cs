using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCONT.Dominio.Entidades.Custom;
using System.Reflection;
using Npgsql;

namespace SCONT.Infraestructura.Dao.Helper
{
    public static class HelperDao
    {
        public static JQGrid ReaderJQGrid(SqlDataReader dr, bool readerSimple = false)
        {
            JQGrid ListaJGrid = new JQGrid();

            try
            {
                int i = 1;
                object[] valuesLista = new object[dr.FieldCount];

                while (dr.Read())
                {
                    dr.GetValues(valuesLista);
                    ListaJGrid.rows.Add(new JQGrid.Row(i, valuesLista.Select(x => x.ToString()).ToList()));
                    i += 1;
                }
                ListaJGrid.colModel = from DataRow r in dr.GetSchemaTable().Rows select r["ColumnName"].ToString();
                if (!readerSimple)
                {
                    dr.NextResult();

                    object[] valuesBase = new object[dr.FieldCount];
                    while (dr.Read())
                    {
                        dr.GetValues(valuesBase);
                        if (valuesBase != null)
                        {
                            ListaJGrid.records = Convert.ToInt32(valuesBase[0]);
                            ListaJGrid.total = Convert.ToInt32(valuesBase[1]);
                            ListaJGrid.page = Convert.ToInt32(valuesBase[2]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return ListaJGrid;
        }

        public static JQGrid ReaderJQGridPSQL(NpgsqlDataReader dr, JQGrid datos = null)
        {
            JQGrid ListaJGrid = new JQGrid();

            try
            {
                int i = 1;
                object[] valuesLista = new object[dr.FieldCount];

                while (dr.Read())
                {
                    dr.GetValues(valuesLista);
                    ListaJGrid.rows.Add(new JQGrid.Row(i, valuesLista.Select(x => x.ToString()).ToList()));
                    i += 1;
                }
                ListaJGrid.colModel = from DataRow r in dr.GetSchemaTable().Rows select r["ColumnName"].ToString();
                //
                if (datos != null)
                {
                    ListaJGrid.records = Convert.ToInt32(datos.records);
                    ListaJGrid.total = Convert.ToInt32(datos.total);
                    ListaJGrid.page = Convert.ToInt32(datos.page);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return ListaJGrid;
        }

        /// <summary>
        /// Crea un objeto tipificado a partir de un objeto OracleDataReader
        /// </summary>
        /// <param name="obj">Objeto a llenar</param>
        /// <param name="dr">OracleDataReader con el que se llenara el objeto</param>
        /// <returns>Retorna un objeto tipificado</returns>
        public static T CreaObjeto<T>(ref T obj, SqlDataReader dr)
        {
            Type type = typeof(T);
            int indice = 0;
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!dr[indice].Equals(DBNull.Value))
                {
                    obj.GetType().GetProperty(pi.Name).SetValue(obj, dr.GetValue(indice), null);
                }
                indice += 1;
            }
            return obj;
        }
        /// <summary>
        /// Crea una lista generica de objetos tipificados a partir de un objeto OracleDataReader
        /// </summary>
        /// <param name="lstObj">Lista generica a llenar</param>
        /// <param name="dr">OracleDataReader con el que se llenara la lista generica</param>
        /// <returns>Retorna una lista generica</returns>
        public static List<T> CreaLista<T>(ref List<T> lstObj, SqlDataReader dr, bool ignorarPrimerCursor = false)
        {
            if (dr.HasRows)
            {
                if (ignorarPrimerCursor)
                {
                    if (dr.Read())
                    {
                        dr.NextResult();
                    }
                }
                while (dr.Read())
                {
                    Type type = typeof(T);
                    T obj = (T)Activator.CreateInstance(type);
                    lstObj.Add(CreaObjeto(ref obj, dr));
                }
            }
            return lstObj;
        }

        //public static TableExcel ReaderTableExcel(SqlDataReader dr, bool readerSimple = false)
        //{
        //    TableExcel ListaJGrid = new TableExcel();

        //    try
        //    {
        //        object[] valuesLista = new object[dr.FieldCount];

        //        while (dr.Read())
        //        {
        //            dr.GetValues(valuesLista);
        //            ListaJGrid.Add(valuesLista.Select(x => x.ToString()).ToList());
        //        }

        //        if (!readerSimple)
        //        {
        //            dr.NextResult();

        //            object[] valuesBase = new object[dr.FieldCount];
        //            while (dr.Read())
        //            {
        //                dr.GetValues(valuesBase);
        //                if (Information.IsDBNull(valuesBase(0)) == false)
        //                {
        //                    ListaJGrid.records = Convert.ToInt32(valuesBase(0));
        //                    ListaJGrid.total = Convert.ToInt32(valuesBase(1));
        //                    ListaJGrid.page = Convert.ToInt32(valuesBase(2));
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //    return ListaJGrid;
        //}

        public static DataTable GetDataTable<T>(IEnumerable<T> dato)
        {
            var dt = new DataTable();
            var prop = dato.GetType().GetProperties();
            foreach (var p in prop)
                dt.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);

            foreach (var item in dato)
            {
                var row = dt.NewRow();

                foreach (var p in prop)
                    row[p.Name] = p.GetValue(item) ?? DBNull.Value;

                dt.Rows.Add(row);
            }

            return dt;
        }

        public static DataTable GetDataTableFormat<T>()
        {
            //BindingFlags flags = (BindingFlags.Public|BindingFlags.Instance)
            var dt = new DataTable();
            var prop = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var p in prop)
                dt.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);

            return dt;
        }
        public static T DataReaderMap<T>(IDataReader dr)
        {
            var columns = from DataRow r in dr.GetSchemaTable().Rows select r["ColumnName"].ToString();
            string columnName = "";
            string columnValue = "";
            int drIndex = 1;
            bool isArrayType = IsArrayType<T>();
            try
            {
                T objMappedReturn = default(T);
                objMappedReturn = Activator.CreateInstance<T>();

                Type typeObject = isArrayType ? objMappedReturn.GetType().GetGenericArguments()[0] : typeof(T);

                //var objMapped = Activator.CreateInstance(typeObject);

                while (dr.Read())
                {
                    var objMapped = Activator.CreateInstance(typeObject);
                    foreach (var column in columns)
                    {
                        columnName = column;

                        PropertyInfo prop = objMapped.GetType().GetProperty(column);
                        if (prop != null && !string.IsNullOrEmpty(prop.Name))
                        {
                            if (!Equals(dr[prop.Name], DBNull.Value))
                            {
                                columnValue = dr[prop.Name].ToString();
                                if (prop.PropertyType.FullName.ToUpper() == "SYSTEM.STRING")
                                    prop.SetValue(objMapped, dr[prop.Name].ToString(), null);
                                else
                                {
                                    Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                    prop.SetValue(objMapped, Convert.ChangeType(dr[prop.Name], propertyType), null);
                                }
                            }
                        }

                    }

                    if (!isArrayType)
                        objMappedReturn = (T)objMapped;
                    else
                    {
                        objMappedReturn.GetType().GetMethod("Add").Invoke(objMappedReturn, new[] { objMapped });
                    }
                }


                return objMappedReturn;
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException == null ? ex.Message : ex.InnerException.InnerException == null ? ex.InnerException.Message : ex.InnerException.InnerException.Message;
                var exCustom = new Exception(String.Format("Ha ocurrido un error en la columna [{0}] \r\n Con el valor {1} y el nro. de fila {2}\n\r Detalle:{3}", columnName, columnValue, drIndex, detalle));
                throw exCustom;
            }
        }

        public static bool IsArrayType<T>()
        {
            T element = default(T);
            element = Activator.CreateInstance<T>();

            if (element.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                return true;
            }

            return false;
        }
    }


}
