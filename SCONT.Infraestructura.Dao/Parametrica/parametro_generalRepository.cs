using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SCONT.Dominio.Entidades.Parametrica;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Dominio.Contratos;
using Npgsql;

namespace SCONT.Infraestructura.Dao
{
    using NpgsqlTypes;
    using SCONT.Infraestructura.BaseDatos;
    using SCONT.Infraestructura.Dao.Helper;

    /// <summary>
    /// Nombre       : parametro_generalRepository
    /// Descripción  : Métodos de trabajo para parametro_generalRepository
    /// </summary>
    /// <remarks>
    /// Creacion     : 25/10/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    public class parametro_generalRepository : Iparametro_generalRepository
    {
        public bool Insert_parametro_general(parametro_general objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("USP_INS_PARAMETRO_GENERAL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@P_ID_PARAMETRO_GENERAL", SqlDbType.Int).Value = objdato.ID_PARAMETRO_GENERAL;
                cmd.Parameters.Add("@P_NOMBRE", SqlDbType.NVarChar, 250).Value = objdato.NOMBRE;
                cmd.Parameters.Add("@P_VALOR", SqlDbType.Int).Value = objdato.VALOR;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.NVarChar, 50).Value = objdato.USUARIO_CREACION;

                cmd.Parameters["@P_ID_PARAMETRO_GENERAL"].Direction = ParameterDirection.InputOutput;

                ind = (BaseDatos.Execute(cmd) > 0);

                objdato.ID_PARAMETRO_GENERAL = (int)cmd.Parameters["@P_ID_PARAMETRO_GENERAL"].Value;

            }
            return ind;
        }

        public bool Update_parametro_general(parametro_general objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call maestro.usp_upd_parametro(:p_id_parametro, :p_nombre, :p_valor, :p_usuario_modificacion)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_parametro", NpgsqlDbType.Integer).Value = objdato.ID_PARAMETRO_GENERAL;
                cmd.Parameters.AddWithValue("p_nombre", NpgsqlDbType.Varchar).Value = objdato.NOMBRE;
                cmd.Parameters.AddWithValue("p_valor", NpgsqlDbType.Varchar).Value = objdato.VALOR;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;

                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Delete_parametro_general(parametro_general objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("USP_DEL_PARAMETRO_GENERAL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@P_ID_PARAMETRO_GENERAL", SqlDbType.Int).Value = objdato.ID_PARAMETRO_GENERAL;
                cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.NVarChar, 50).Value = objdato.USUARIO_MODIFICACION;

                ind = (BaseDatos.Execute(cmd) > 0);

            }
            return ind;
        }

        public parametro_general Select_parametro_general(parametro_general objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_parametro";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_codigo", objdato.CODIGO);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_parametro_general_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }
        
        public List<parametro_general> SelectAll_parametro_general()
        {
            List<parametro_general> lisData = new List<parametro_general>();
            SqlCommand cmd = new SqlCommand("USP_SEL_PARAMETRO_GENERAL_ALL");
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(Get_parametro_general_FromReader(dr));
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
        }

        public JQGrid SelectPaginated_parametro_general(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            JQGrid datGrid = new JQGrid();
            using (NpgsqlCommand cmd = new NpgsqlCommand("maestro.fn_sel_parametro_pag"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_page_size", objdato.PAGE_SIZE);
                cmd.Parameters.AddWithValue("p_page_number", (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER));

                using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
                {
                    datGrid = SelectPaginated_parametro_general_datos(objdato);
                    lisGrid = HelperDao.ReaderJQGridPSQL(dr, datGrid);
                }
            }
            return lisGrid;
        }

        JQGrid SelectPaginated_parametro_general_datos(Parametro objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_parametro_pag_datos";
            cmd.Parameters.AddWithValue("p_page_size", objdato.PAGE_SIZE);
            cmd.Parameters.AddWithValue("p_page_number", (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER));

            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return new JQGrid
                    {
                        records = int.Parse(dr[0].ToString()),
                        total = int.Parse(dr[1].ToString()),
                        page = int.Parse(dr[2].ToString()),
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        private parametro_general Get_parametro_general_FromReader(IDataReader dr)
        {
            parametro_general objparametro_general = new parametro_general();
            int nrocampos = dr.FieldCount - 1;
            object[] data = new object[nrocampos + 1];
            dr.GetValues(data);
            objparametro_general.ID_PARAMETRO_GENERAL = (int)data[0];
            objparametro_general.CODIGO = data[1].ToString();
            objparametro_general.NOMBRE = data[2].ToString();
            objparametro_general.VALOR = data[3].ToString();
            objparametro_general.ESTADO = (int)data[4];
            objparametro_general.USUARIO_CREACION = data[5].ToString();
            if (data[6] != DBNull.Value) objparametro_general.FECHA_CREACION = Convert.ToDateTime(data[6]);
            objparametro_general.USUARIO_MODIFICACION = data[7].ToString();
            if (data[8] != DBNull.Value) objparametro_general.FECHA_MODIFICACION = Convert.ToDateTime(data[8]);
            return objparametro_general;
        }


        public List<parametro_general> SelAll_ejecutora()
        {
            List<parametro_general> lisData = new List<parametro_general>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_ejecutora_all";
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    lisData.Add(new parametro_general
                    {
                        CODIGO = dr[0].ToString(),
                        NOMBRE = dr[1].ToString(),
                        EJECUTORA = dr[2].ToString()

                    });
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
          
        }

        public List<parametro_general> SelAll_ejecutora_usuario_centro_costo(Parametro objdato)
        {
            List<parametro_general> lisData = new List<parametro_general>();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_EJECUTORA_BY_USUARIO_CC_ALL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.PARAMETRO1;
            cmd.Parameters.Add("@P_ANO_EJE", SqlDbType.Int).Value = objdato.PARAMETRO2;

            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new parametro_general
                        {
                            CODIGO = dr[0].ToString(),
                            NOMBRE = dr[1].ToString(),

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

        public List<parametro_general> SelAll_tipo_proceso()
        {
            List<parametro_general> lisData = new List<parametro_general>();
            SqlCommand cmd = new SqlCommand("maestro.USP_SEL_TIPO_PROCESO_ALL");
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new parametro_general
                        {
                            CODIGO = dr[0].ToString(),
                            NOMBRE = dr[1].ToString()
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
        
        public List<parametro_general> SelAll_tipo_proceso_ps()
        {
            List<parametro_general> lisData = new List<parametro_general>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "transaccional.fn_sel_ps_tipo_proceso_all";
            cmd.CommandType = CommandType.StoredProcedure;
            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new parametro_general
                        {
                            CODIGO = dr[0].ToString(),
                            NOMBRE = dr[1].ToString()
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

        public List<parametro_general> SelAll_modalidad_compra_asp()
        {
            List<parametro_general> lisData = new List<parametro_general>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "transaccional.fn_sel_asp_modalidad_compra_all";
            cmd.CommandType = CommandType.StoredProcedure;
            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new parametro_general
                        {
                            CODIGO = dr[0].ToString(),
                            NOMBRE = dr[1].ToString(),
                            CODIGO_NOMBRE = dr[0].ToString() + "-" + dr[1].ToString()
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
