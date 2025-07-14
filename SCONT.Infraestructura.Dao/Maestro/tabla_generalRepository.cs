using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Contratos;
using Npgsql;

namespace SCONT.Infraestructura.Dao
{
    using SCONT.Infraestructura.BaseDatos;
    using SCONT.Infraestructura.Dao.Helper;
    using SCONT.Dominio.Entidades.Custom;
    using NpgsqlTypes;

    public class tabla_generalRepository : Itabla_generalRepository
    {
        public JQGrid SelectPaginated_tabla_general(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            using (SqlCommand cmd = new SqlCommand("USP_SEL_TABLA_GENERAL_PAG"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_SORT_COLUMN", SqlDbType.NVarChar).Value = objdato.SORT_COLUMN;
                cmd.Parameters.Add("@P_SORT_ORDER", SqlDbType.NVarChar).Value = objdato.SORT_ORDER;
                cmd.Parameters.Add("@P_PAGE_SIZE", SqlDbType.Int).Value = objdato.PAGE_SIZE;
                cmd.Parameters.Add("@P_PAGE_NUMBER", SqlDbType.Int).Value = (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER);
                using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
                {
                    lisGrid = HelperDao.ReaderJQGrid(dr);
                }
            }
            return lisGrid;
        }

        public tabla_general Select_tabla_general(tabla_general objdato)
        {
            SqlCommand cmd = new SqlCommand("USP_SEL_TABLA_GENERAL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_TABLA_GENERAL", SqlDbType.Int).Value = objdato.ID_TABLA_GENERAL;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_tabla_general_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        private tabla_general Get_tabla_general_FromReader(IDataReader dr)
        {
            tabla_general objDato = new tabla_general();
            int nrocampos = dr.FieldCount - 1;
            object[] data = new object[nrocampos + 1];
            dr.GetValues(data);
            objDato.ID_TABLA_GENERAL = (int)data[0];
            objDato.ID_GRUPO = (int)data[1];
            objDato.NOMBRE_GRUPO = data[2].ToString();
            objDato.CODIGO = data[3].ToString();
            objDato.NOMBRE = data[4].ToString();
            objDato.DATO1 = data[5].ToString();
            objDato.DATO2 = data[6].ToString();
            objDato.DATO3 = data[7].ToString();
            objDato.ESTADO = (int)data[8];
            objDato.USUARIO_CREACION = (int)data[9];
            if (data[10] != DBNull.Value) objDato.FECHA_CREACION = Convert.ToDateTime(data[10]);
            objDato.USUARIO_MODIFICACION = (int)data[11];
            if (data[12] != DBNull.Value) objDato.FECHA_MODIFICACION = Convert.ToDateTime(data[12]);
            return objDato;
        }

        public List<tabla_general> SelectAll_tabla_general_by_grupo(int grupo)
        {
            List<tabla_general> lisData = new List<tabla_general>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_tabla_general_by_grupo_all";
            cmd.Parameters.AddWithValue("p_id_grupo", grupo);
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new tabla_general
                        {
                            ID_TABLA_GENERAL = (int)dr[0],
                            ID_GRUPO = (int)dr[1],
                            NOMBRE_GRUPO = dr[2].ToString(),
                            CODIGO = dr[3].ToString(),
                            NOMBRE = dr[4].ToString(),
                            DATO1 = dr[5].ToString(),
                            DATO2 = dr[6].ToString(),
                            DATO3 = dr[7].ToString(),
                            ESTADO = (int)dr[8]
                        });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }

        }

        public List<tabla_general> SelectAll_seguimiento_log(tabla_general dato)
        {
            List<tabla_general> lisData = new List<tabla_general>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "transaccional.fn_sel_seguimiento_log_all";
            cmd.Parameters.AddWithValue("p_numero_dni", dato.LOG_DNI);
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new tabla_general
                        {
                            LOG_FECHA_HORA = dr[0].ToString(),
                            LOG_COMENTARIO = dr[1].ToString()
                        });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }

        }


        public bool Delete_seguimiento_log(tabla_general dato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call transaccional.usp_del_seguimiento_log(:p_numero_dni)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_numero_dni", NpgsqlDbType.Integer).Value = dato.LOG_DNI;

                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Insert_tabla_general(tabla_general objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("USP_INS_TABLA_GENERAL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_TABLA_GENERAL", SqlDbType.Int).Value = objdato.ID_TABLA_GENERAL;
                cmd.Parameters.Add("@P_ID_GRUPO", SqlDbType.Int).Value = objdato.ID_GRUPO;
                cmd.Parameters.Add("@P_NOMBRE_GRUPO", SqlDbType.NVarChar).Value = objdato.NOMBRE_GRUPO;
                cmd.Parameters.Add("@P_CODIGO", SqlDbType.NVarChar).Value = objdato.CODIGO;
                cmd.Parameters.Add("@P_NOMBRE", SqlDbType.NVarChar).Value = objdato.NOMBRE;
                cmd.Parameters.Add("@P_DATO1", SqlDbType.NVarChar).Value = objdato.DATO1;
                cmd.Parameters.Add("@P_DATO2", SqlDbType.NVarChar).Value = objdato.DATO2;
                cmd.Parameters.Add("@P_DATO3", SqlDbType.NVarChar).Value = objdato.DATO3;
                cmd.Parameters.Add("@P_ESTADO", SqlDbType.Int).Value = objdato.ESTADO;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.Int).Value = objdato.USUARIO_CREACION;
                cmd.Parameters.Add("@P_FECHA_CREACION", SqlDbType.DateTime).Value = objdato.FECHA_CREACION;
                cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.Int).Value = objdato.USUARIO_MODIFICACION;
                cmd.Parameters.Add("@P_FECHA_MODIFICACION", SqlDbType.DateTime).Value = objdato.FECHA_MODIFICACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Update_tabla_general(tabla_general objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("USP_UPD_TABLA_GENERAL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_TABLA_GENERAL", SqlDbType.Int).Value = objdato.ID_TABLA_GENERAL;
                cmd.Parameters.Add("@P_ID_GRUPO", SqlDbType.Int).Value = objdato.ID_GRUPO;
                cmd.Parameters.Add("@P_NOMBRE_GRUPO", SqlDbType.NVarChar).Value = objdato.NOMBRE_GRUPO;
                cmd.Parameters.Add("@P_CODIGO", SqlDbType.NVarChar).Value = objdato.CODIGO;
                cmd.Parameters.Add("@P_NOMBRE", SqlDbType.NVarChar).Value = objdato.NOMBRE;
                cmd.Parameters.Add("@P_DATO1", SqlDbType.NVarChar).Value = objdato.DATO1;
                cmd.Parameters.Add("@P_DATO2", SqlDbType.NVarChar).Value = objdato.DATO2;
                cmd.Parameters.Add("@P_DATO3", SqlDbType.NVarChar).Value = objdato.DATO3;
                cmd.Parameters.Add("@P_ESTADO", SqlDbType.Int).Value = objdato.ESTADO;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.Int).Value = objdato.USUARIO_CREACION;
                cmd.Parameters.Add("@P_FECHA_CREACION", SqlDbType.DateTime).Value = objdato.FECHA_CREACION;
                cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.Int).Value = objdato.USUARIO_MODIFICACION;
                cmd.Parameters.Add("@P_FECHA_MODIFICACION", SqlDbType.DateTime).Value = objdato.FECHA_MODIFICACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Delete_tabla_general(tabla_general objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("USP_DEL_TABLA_GENERAL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_TABLA_GENERAL", SqlDbType.Int).Value = objdato.ID_TABLA_GENERAL;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public List<tabla_general> SelectAll_tabla_general_by_grupo_and_rol(int grupo, int rol)
        {
            List<tabla_general> lisData = new List<tabla_general>();
            SqlCommand cmd = new SqlCommand("maestro.USP_SEL_TABLA_GENERAL_BY_GRUPO_AND_ROL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_GRUPO", SqlDbType.Int).Value = grupo;
            cmd.Parameters.Add("@P_ID_ROL", SqlDbType.Int).Value = rol;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new tabla_general
                        {
                            ID_TABLA_GENERAL = (int)dr[0],
                            ID_GRUPO = (int)dr[1],
                            NOMBRE_GRUPO = dr[2].ToString(),
                            CODIGO = dr[3].ToString(),
                            NOMBRE = dr[4].ToString(),
                            DATO1 = dr[5].ToString(),
                            DATO2 = dr[6].ToString(),
                            DATO3 = dr[7].ToString(),
                            ESTADO = (int)dr[8]
                        });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<tabla_general> SelectAll_tabla_general_by_nombre_grupo(tabla_general grupo)
        {
            List<tabla_general> lisData = new List<tabla_general>();
            SqlCommand cmd = new SqlCommand("maestro.USP_SEL_TABLA_GENERAL_BY_NOMBRE_GRUPO");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_NOMBRE_GRUPO", SqlDbType.VarChar).Value = grupo.NOMBRE_GRUPO;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new tabla_general
                        {
                            ID_TABLA_GENERAL = (int)dr[0],
                            ID_GRUPO = (int)dr[1],
                            NOMBRE_GRUPO = dr[2].ToString(),
                            CODIGO = dr[3].ToString(),
                            NOMBRE = dr[4].ToString(),
                            DATO1 = dr[5].ToString(),
                            DATO2 = dr[6].ToString(),
                            DATO3 = dr[7].ToString(),
                            ESTADO = (int)dr[8]
                        });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
