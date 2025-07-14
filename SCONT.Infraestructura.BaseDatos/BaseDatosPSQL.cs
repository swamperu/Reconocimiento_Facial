using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Npgsql;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCONT.Infraestructura.BaseDatos
{
    /// <summary>
    /// Nombre       : BaseDatos
    /// Descripción  : Metodos de manipulación para trabajar con las bases de datos
    /// </summary>
    /// <remarks>
    /// Creacion     : 15/02/2010 JMZ
    /// Modificacion : 
    /// </remarks>
    public static class BaseDatosPSQL
    {

        //constantes - conexiones del Sistema


        public const string ConnectionString_admin = "1";
        /// <summary>
        /// Devuelve un objeto NpgsqlConnection con la conexion abierta,
        /// desde la cadena de conexion Default para la aplicacion 
        /// (ConnectionString)
        /// </summary>
        public static NpgsqlConnection GetConnection(string nroConexion = "")
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString" + nroConexion].ConnectionString;
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            return connection;
        }


        // ''' <summary>
        // ''' Devuelve un objeto NpgsqlConnection con la conexion abierta,
        // ''' desde la cadena de conexion proporcionada por el usuario que debe estar en el archivo de configuracion
        // ''' (ConnectionString)
        // ''' </summary>
        //Public Shared Function GetConnection(ByVal nuevaConnectionString As String) As NpgsqlConnection
        //    Dim connectionString As String = ConfigurationManager.ConnectionStrings(nuevaConnectionString).ConnectionString
        //    Dim connection As New NpgsqlConnection(connectionString)
        //    connection.Open()

        //    Return connection
        //End Function
        /// <summary>
        /// Devuelve la cadena de conexion default del archivo de configuracion
        /// </summary>
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Lectura de datos mediante un objeto NpgsqlDataReader.
        /// </summary>
        /// <param name="cmd">
        /// Comando (NpgsqlCommand) que debe especificar:
        /// 1. CommandType
        /// 2. CommandName
        /// 3. Parameters (si los hubiera)
        /// </param>
        /// <returns>NpgsqlDataReader</returns>
        public static NpgsqlDataReader GetDataReader(NpgsqlCommand cmd, string nroconexion = "")
        {
            ValidCommand(cmd);
            cmd.Connection = GetConnection(nroconexion);

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>
        /// Metodo utilizado para la lectura de datos. Devuelve un objeto NpgsqlDataReader.
        /// </summary>
        /// <param name="instruccionSQL">instruccion Npgsql a ejecutar</param>
        public static NpgsqlDataReader GetDataReader(string instruccionSQL, string nroconexion = "")
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = GetConnection(nroconexion);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = instruccionSQL;

            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        /// <summary>
        /// Metodo utilizado para obtener un objeto DataSet.
        /// </summary>
        /// <param name="cmd">
        /// Comando (NpgsqlCommand) que debe especificar:
        /// 1. CommandType
        /// 2. CommandName
        /// 3. Parameters (si los hubiera)
        /// </param> 
        public static DataSet GetDataSet(NpgsqlCommand cmd, string nroconexion = "")
        {
            ValidCommand(cmd);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataSet ds = new DataSet();

            using (NpgsqlConnection cn = GetConnection(nroconexion))
            {
                cmd.Connection = cn;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }

            return ds;
        }
        /// <summary>
        /// Metodo utilizado para obtener un objeto DataSet.
        /// </summary>
        /// <param name="instruccionSQL">Cadena SQL para ejecutar el objeto DataAdapter.</param>
        public static DataSet GetDataSet(string instruccionSQL, string nroconexion = "")
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataSet ds = new DataSet();

            using (NpgsqlConnection cn = GetConnection(nroconexion))
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = instruccionSQL;
                da.SelectCommand = cmd;
                da.Fill(ds);
            }

            return ds;
        }
        /// <summary>
        /// Metodo utilizado para obtener un unico valor mediante una consulta.
        /// </summary>
        /// <param name="cmd">
        /// Comando (NpgsqlCommand) que debe especificar:
        /// 1. CommandType
        /// 2. CommandName
        /// 3. Parameters (si los hubiera)
        /// </param> 
        public static object GetDataScalar(NpgsqlCommand cmd, string nroconexion = "")
        {
            ValidCommand(cmd);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataSet ds = new DataSet();
            object valorScalar = null;

            using (NpgsqlConnection cn = GetConnection(nroconexion))
            {
                cmd.Connection = cn;
                valorScalar = cmd.ExecuteScalar();
            }

            return valorScalar;
        }

        /// <summary>
        /// Metodo utilizado para obtener un unico valor mediante una consulta.
        /// </summary>
        /// <param name="instruccionSQL">Cadena SQL para ejecutar la consulta</param>
        public static object GetDataScalar(string instruccionSQL, string nroconexion = "")
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            DataSet ds = new DataSet();
            object valorScalar = null;

            using (NpgsqlConnection cn = GetConnection(nroconexion))
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = instruccionSQL;
                valorScalar = cmd.ExecuteScalar();
            }

            return valorScalar;
        }
        /// <summary>
        /// Método utilizado para ejecutar las acciones de inserción, edición y eliminación.
        /// </summary>
        /// <param name="cmd">
        /// Comando (NpgsqlCommand) que debe especificar:
        /// 1. CommandType
        /// 2. CommandName
        /// 3. Parameters (si los hubiera)
        /// </param>
        /// <returns>Número de registros afectados</returns>
        public static int Execute(NpgsqlCommand cmd, string nroConexion = "")
        {
            ValidCommand(cmd);

            int nroRegistos = 1;
            try
            {
                using (NpgsqlConnection cn = GetConnection(nroConexion))
                {
                    cmd.Connection = cn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception Ex)
            {
                string msj = Ex.Message;
                nroRegistos = 0;
            }
            
            return nroRegistos;
        }
        /// <summary>
        /// Método utilizado para ejecutar las acciones de inserción, edición y eliminación en una transaccion.
        /// </summary>
        /// <param name="cmd">
        /// Comando (NpgsqlCommand) que debe especificar:
        /// 1. CommandType
        /// 2. CommandName
        /// 3. Parameters (si los hubiera)
        /// 4. Transaction
        /// </param>
        /// <returns>Número de registros afectados</returns>
        public static int Execute(NpgsqlCommand cmd, NpgsqlTransaction transaction)
        {
            ValidCommand(cmd);

            int nroRegistros = 0;
            cmd.Connection = transaction.Connection;
            cmd.Transaction = transaction;
            nroRegistros = cmd.ExecuteNonQuery();

            return nroRegistros;
        }

        /// <summary>
        /// Método utilizado para ejecutar las acciones de inserción, edición, eliminación.
        /// </summary>
        /// <param name="instruccionSQL">Cadena SQL para ejecutar la acción.</param>
        /// <returns>Número de registros afectados</returns>
        public static int Execute(string instruccionSQL, string nroconexion = "")
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            int nroRegistros = 0;

            using (NpgsqlConnection cn = GetConnection(nroconexion))
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = instruccionSQL;
                nroRegistros = cmd.ExecuteNonQuery();
            }

            return nroRegistros;
        }
        /// <summary>
        /// Valida los datos basicos de un comando
        /// </summary>
        private static bool ValidCommand(NpgsqlCommand cmd)
        {
            if (cmd == null)
                throw new ArgumentException("Debe proporcionar un objecto NpgsqlCommand");
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new ArgumentException("Debe proporcionar el nombre del procedimiento almacenado o instruccion SQL");
            return true;
        }

        public static IEnumerable<string> GetColumns(IDataReader dr)
        {
            var columns = from DataRow r in dr.GetSchemaTable().Rows select r["ColumnName"].ToString();
            return columns;
        }
        public static T DataReaderMap<T>(IDataReader dr, IEnumerable<string> columns)
        {
            string columnName = "";
            string columnValue = "";
            try
            {
                T objMapped = default(T);
                objMapped = Activator.CreateInstance<T>();

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
                                //Solo cuando devuelve Bytes[] ose cualquier tipo de Documento
                                if (prop.PropertyType.IsArray)
                                {

                                    var dataArray = (byte[])dr[prop.Name];
                                    if (Nullable.GetUnderlyingType(prop.PropertyType.GetElementType()) != null)
                                    {
                                        var dataArrayNull = Array.ConvertAll<byte, byte?>(dataArray, value => value);
                                        prop.SetValue(objMapped, dataArrayNull, null);

                                    }
                                    else
                                    {
                                        prop.SetValue(objMapped, dataArray, null);
                                    }
                                }
                                else
                                {
                                    Type propertyType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                    prop.SetValue(objMapped, Convert.ChangeType(dr[prop.Name], propertyType), null);
                                }

                            }
                        }
                    }

                }
                return objMapped;
            }
            catch (Exception ex)
            {
                //var detalle = ex.InnerException == null ? ex.Message : ex.InnerException.InnerException == null ? ex.InnerException.Message : ex.InnerException.InnerException.Message;
                //var exCustom = new Exception($"Ha ocurrido un error en la columna [{columnName}] \r\n Con el valor {columnValue} y el nro. de fila {drIndex}\n\r Detalle:{detalle}");
                throw ex;
            }
        }
    }

}
