using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Contratos;

namespace SCONT.Infraestructura.Dao
{
    using SCONT.Infraestructura.BaseDatos;
    using SCONT.Infraestructura.Dao.Helper;
    using SCONT.Dominio.Entidades.Custom;
    using Npgsql;

    public class ubigeoRepository : IubigeoRepository
    {
        public JQGrid SelectPaginated_ubigeo(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            using (SqlCommand cmd = new SqlCommand("maestro.USP_SEL_UBIGEO_PAG"))
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

        public ubigeo Select_ubigeo(ubigeo objdato)
        {
            SqlCommand cmd = new SqlCommand("maestro.USP_SEL_UBIGEO");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_UBIGEO", SqlDbType.NVarChar).Value = objdato.ID_UBIGEO;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_ubigeo_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<ubigeo> SelectAll_departamento()
        {
            List<ubigeo> lisData = new List<ubigeo>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_departamento_all";
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new ubigeo
                        {
                            DEP_CODIGO = dr[0].ToString(),
                            DEP_NOMBRE = dr[1].ToString()
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

        public List<ubigeo> SelectAll_provincia(ubigeo objdato)
        {
            List<ubigeo> lisData = new List<ubigeo>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_provincia_all";
            cmd.Parameters.AddWithValue("p_dep_codigo", objdato.DEP_CODIGO);
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new ubigeo
                        {
                            DEP_CODIGO = dr[0].ToString(),
                            DEP_NOMBRE = dr[1].ToString(),
                            PRV_CODIGO = dr[2].ToString(),
                            PRV_NOMBRE = dr[3].ToString()
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

        public List<ubigeo> SelectAll_distrito(ubigeo objdato)
        {
            List<ubigeo> lisData = new List<ubigeo>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_distrito_all";
            cmd.Parameters.AddWithValue("p_dep_codigo", objdato.DEP_CODIGO);
            cmd.Parameters.AddWithValue("p_prv_codigo", objdato.PRV_CODIGO);
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new ubigeo
                        {
                            DEP_CODIGO = dr[0].ToString(),
                            DEP_NOMBRE = dr[1].ToString(),
                            PRV_CODIGO = dr[2].ToString(),
                            PRV_NOMBRE = dr[3].ToString(),
                            DIS_CODIGO = dr[4].ToString(),
                            DIS_NOMBRE = dr[5].ToString()
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

        private ubigeo Get_ubigeo_FromReader(IDataReader dr)
        {
            ubigeo objDato = new ubigeo();
            int nrocampos = dr.FieldCount - 1;
            object[] data = new object[nrocampos + 1];
            dr.GetValues(data);
            objDato.ID_UBIGEO = data[0].ToString();
            objDato.DEP_CODIGO = data[1].ToString();
            objDato.DEP_NOMBRE = data[2].ToString();
            objDato.PRV_CODIGO = data[3].ToString();
            objDato.PRV_NOMBRE = data[4].ToString();
            objDato.DIS_CODIGO = data[5].ToString();
            objDato.DIS_NOMBRE = data[6].ToString();
            objDato.ESTADO = (int)data[7];
            objDato.USUARIO_CREACION = data[8].ToString();
            if (data[9] != DBNull.Value) objDato.FECHA_CREACION = Convert.ToDateTime(data[9]);
            objDato.USUARIO_MODIFICACION = data[10].ToString();
            if (data[11] != DBNull.Value) objDato.FECHA_MODIFICACION = Convert.ToDateTime(data[11]);
            return objDato;
        }

        public bool Insert_ubigeo(ubigeo objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("maestro.USP_INS_UBIGEO"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_UBIGEO", SqlDbType.NVarChar).Value = objdato.ID_UBIGEO;
                cmd.Parameters.Add("@P_DEP_CODIGO", SqlDbType.NVarChar).Value = objdato.DEP_CODIGO;
                cmd.Parameters.Add("@P_DEP_NOMBRE", SqlDbType.NVarChar).Value = objdato.DEP_NOMBRE;
                cmd.Parameters.Add("@P_PRV_CODIGO", SqlDbType.NVarChar).Value = objdato.PRV_CODIGO;
                cmd.Parameters.Add("@P_PRV_NOMBRE", SqlDbType.NVarChar).Value = objdato.PRV_NOMBRE;
                cmd.Parameters.Add("@P_DIS_CODIGO", SqlDbType.NVarChar).Value = objdato.DIS_CODIGO;
                cmd.Parameters.Add("@P_DIS_NOMBRE", SqlDbType.NVarChar).Value = objdato.DIS_NOMBRE;
                cmd.Parameters.Add("@P_ESTADO", SqlDbType.Int).Value = objdato.ESTADO;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.NVarChar).Value = objdato.USUARIO_CREACION;
                cmd.Parameters.Add("@P_FECHA_CREACION", SqlDbType.DateTime).Value = objdato.FECHA_CREACION;
                cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.NVarChar).Value = objdato.USUARIO_MODIFICACION;
                cmd.Parameters.Add("@P_FECHA_MODIFICACION", SqlDbType.DateTime).Value = objdato.FECHA_MODIFICACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Update_ubigeo(ubigeo objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("maestro.USP_UPD_UBIGEO"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_UBIGEO", SqlDbType.NVarChar).Value = objdato.ID_UBIGEO;
                cmd.Parameters.Add("@P_DEP_CODIGO", SqlDbType.NVarChar).Value = objdato.DEP_CODIGO;
                cmd.Parameters.Add("@P_DEP_NOMBRE", SqlDbType.NVarChar).Value = objdato.DEP_NOMBRE;
                cmd.Parameters.Add("@P_PRV_CODIGO", SqlDbType.NVarChar).Value = objdato.PRV_CODIGO;
                cmd.Parameters.Add("@P_PRV_NOMBRE", SqlDbType.NVarChar).Value = objdato.PRV_NOMBRE;
                cmd.Parameters.Add("@P_DIS_CODIGO", SqlDbType.NVarChar).Value = objdato.DIS_CODIGO;
                cmd.Parameters.Add("@P_DIS_NOMBRE", SqlDbType.NVarChar).Value = objdato.DIS_NOMBRE;
                cmd.Parameters.Add("@P_ESTADO", SqlDbType.Int).Value = objdato.ESTADO;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.NVarChar).Value = objdato.USUARIO_CREACION;
                cmd.Parameters.Add("@P_FECHA_CREACION", SqlDbType.DateTime).Value = objdato.FECHA_CREACION;
                cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.NVarChar).Value = objdato.USUARIO_MODIFICACION;
                cmd.Parameters.Add("@P_FECHA_MODIFICACION", SqlDbType.DateTime).Value = objdato.FECHA_MODIFICACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Delete_ubigeo(ubigeo objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("maestro.USP_DEL_UBIGEO"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_UBIGEO", SqlDbType.NVarChar).Value = objdato.ID_UBIGEO;
                cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.NVarChar).Value = objdato.USUARIO_MODIFICACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

    }
}
