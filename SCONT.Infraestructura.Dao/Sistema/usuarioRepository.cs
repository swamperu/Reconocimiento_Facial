using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Contratos;
using Npgsql;
using NpgsqlTypes;

namespace SCONT.Infraestructura.Dao
{
    using SCONT.Infraestructura.BaseDatos;
    using SCONT.Infraestructura.Dao.Helper;
    using SCONT.Dominio.Entidades.Custom;

    public class usuarioRepository : IusuarioRepository
    {
        public JQGrid SelectPaginated_usuario(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            JQGrid datGrid = new JQGrid();
            using (NpgsqlCommand cmd = new NpgsqlCommand("sistema.fn_sel_usuario_pag"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO1);
                cmd.Parameters.AddWithValue("p_apellido_paterno", objdato.PARAMETRO2);
                cmd.Parameters.AddWithValue("p_apellido_materno", objdato.PARAMETRO4);
                cmd.Parameters.AddWithValue("p_id_rol", objdato.PARAMETRO3);
                cmd.Parameters.AddWithValue("p_page_size", objdato.PAGE_SIZE);
                cmd.Parameters.AddWithValue("p_page_number", (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER));

                using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
                {
                    datGrid = SelectPaginated_usuario_datos(objdato);
                    lisGrid = HelperDao.ReaderJQGridPSQL(dr, datGrid);
                }
            }
            return lisGrid;
        }

        JQGrid SelectPaginated_usuario_datos(Parametro objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_pag_datos";
            cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO1);
            cmd.Parameters.AddWithValue("p_apellido_paterno", objdato.PARAMETRO2);
            cmd.Parameters.AddWithValue("p_apellido_materno", objdato.PARAMETRO4);
            cmd.Parameters.AddWithValue("p_id_rol", objdato.PARAMETRO3);
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
        
        public usuario Select_usuario(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", objdato.ID_USUARIO);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public usuario Select_usuario_by_usuario(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_by_usuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", objdato.ID_USUARIO);
            cmd.Parameters.AddWithValue("@p_usuario", objdato.USUARIO);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public usuario Select_usuario_by_correo(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_by_correo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", objdato.ID_USUARIO);
            cmd.Parameters.AddWithValue("@p_correo", objdato.CORREO);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public usuario Select_usuario_by_dni(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_by_dni";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", objdato.ID_USUARIO);
            cmd.Parameters.AddWithValue("@p_dni", objdato.DNI);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public usuario Select_usuario_by_datos(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_by_datos";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id_usuario", objdato.ID_USUARIO);
            cmd.Parameters.AddWithValue("@p_nombres", objdato.NOMBRES);
            cmd.Parameters.AddWithValue("@p_apellido_paterno", objdato.APELLIDO_PATERNO);
            cmd.Parameters.AddWithValue("@p_apellido_materno", objdato.APELLIDO_MATERNO);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public usuario Select_usuario_login(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_login";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_usuario", objdato.USUARIO);
            cmd.Parameters.AddWithValue("@p_contrasenia", objdato.CONTRASENIA);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        private usuario Get_usuario_FromReader(IDataReader dr)
        {
            usuario objDato = new usuario();
            int nrocampos = dr.FieldCount - 1;
            object[] data = new object[nrocampos + 1];
            dr.GetValues(data);
            objDato.ID_USUARIO = (int)data[0];
            objDato.USUARIO = HelperAES.DecryptStringAES(data[1].ToString());
            objDato.CONTRASENIA = HelperAES.DecryptStringAES(data[2].ToString());
            objDato.DNI = data[3].ToString();
            objDato.NOMBRES = data[4].ToString();
            objDato.APELLIDO_PATERNO = data[5].ToString();
            objDato.APELLIDO_MATERNO = data[6].ToString();
            objDato.ID_ROL = data[7].ToString();
            if (data[8] != DBNull.Value) objDato.ULTIMO_INGRESO = Convert.ToDateTime(data[8]);
            objDato.CORREO = data[9].ToString();
            objDato.ESTADO = (int)data[10];
            objDato.USUARIO_CREACION = data[11].ToString();
            if (data[12] != DBNull.Value) objDato.FECHA_CREACION = Convert.ToDateTime(data[12]);
            objDato.USUARIO_MODIFICACION = data[13].ToString();
            if (data[14] != DBNull.Value) objDato.FECHA_MODIFICACION = Convert.ToDateTime(data[14]);
            objDato.NOMBRE_ROL = data[15].ToString();
            return objDato;
        }

        public bool Insert_usuario(usuario objdato)
        {
            bool ind = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_ins_usuario";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_usuario", objdato.USUARIO);
            cmd.Parameters.AddWithValue("p_contrasenia", objdato.CONTRASENIA);
            cmd.Parameters.AddWithValue("p_dni", objdato.DNI);
            cmd.Parameters.AddWithValue("p_nombres", objdato.NOMBRES);
            cmd.Parameters.AddWithValue("p_apellido_paterno", objdato.APELLIDO_PATERNO);
            cmd.Parameters.AddWithValue("p_apellido_materno", objdato.APELLIDO_MATERNO);
            cmd.Parameters.AddWithValue("p_id_rol", objdato.ID_ROL);
            cmd.Parameters.AddWithValue("p_correo", objdato.CORREO);
            cmd.Parameters.AddWithValue("p_usuario_creacion", objdato.USUARIO_CREACION);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    objdato.ID_USUARIO = int.Parse(dr[0].ToString());
                }
            }
            ind = (objdato.ID_USUARIO > 0);
            return ind;
        }

        public bool Update_usuario(usuario objdato)
        {
            bool ind = false;
            string cadena = "call sistema.usp_upd_usuario(:p_id_usuario, :p_usuario, :p_contrasenia, :p_dni, :p_nombres, :p_apellido_paterno, :p_apellido_materno, :p_id_rol, :p_correo, :p_usuario_modificacion )";

            using (NpgsqlCommand cmd = new NpgsqlCommand(cadena))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_usuario", NpgsqlDbType.Integer).Value = objdato.ID_USUARIO;
                cmd.Parameters.AddWithValue("p_usuario", NpgsqlDbType.Text).Value = objdato.USUARIO;
                cmd.Parameters.AddWithValue("p_contrasenia", NpgsqlDbType.Text).Value = objdato.CONTRASENIA;
                cmd.Parameters.AddWithValue("p_dni", NpgsqlDbType.Varchar).Value = objdato.DNI;
                cmd.Parameters.AddWithValue("p_nombres", NpgsqlDbType.Varchar).Value = objdato.NOMBRES;
                cmd.Parameters.AddWithValue("p_apellido_paterno", NpgsqlDbType.Varchar).Value = objdato.APELLIDO_PATERNO;
                cmd.Parameters.AddWithValue("p_apellido_materno", NpgsqlDbType.Varchar).Value = objdato.APELLIDO_MATERNO;
                cmd.Parameters.AddWithValue("p_id_rol", NpgsqlDbType.Varchar).Value = objdato.ID_ROL;
                cmd.Parameters.AddWithValue("p_correo", NpgsqlDbType.Text).Value = objdato.CORREO;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;

                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Update_usuario_ingreso(usuario objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call sistema.usp_upd_usuario_ingreso(:p_id_usuario)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_usuario", NpgsqlDbType.Integer).Value = objdato.ID_USUARIO;

                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Delete_usuario(usuario objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call sistema.usp_del_usuario(:p_id_usuario, :p_usuario_modificacion)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_usuario", NpgsqlDbType.Integer).Value = objdato.ID_USUARIO;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;
                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Update_usuario_password(usuario objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call sistema.usp_upd_usuario_contrasenia(:p_id_usuario, :p_contrasenia, :p_nueva_contrasenia, :p_usuario_modificacion)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_usuario", NpgsqlDbType.Integer).Value = objdato.ID_USUARIO;
                cmd.Parameters.AddWithValue("p_contrasenia", NpgsqlDbType.Text).Value = objdato.CONTRASENIA;
                cmd.Parameters.AddWithValue("p_nueva_contrasenia", NpgsqlDbType.Text).Value = objdato.NUEVA_CONTRASENIA;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;

                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public List<usuario> SelectAll_usuario_by_rol(string id_rol)
        {
            List<usuario> lisData = new List<usuario>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_by_rol_all";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_id_rol", id_rol);
            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new usuario
                        {
                            ID_USUARIO = (int)dr[0],
                            USUARIO = dr[1].ToString(),
                            CONTRASENIA = dr[2].ToString(),
                            DNI = dr[4].ToString(),
                            NOMBRES = dr[5].ToString(),
                            APELLIDO_PATERNO = dr[6].ToString(),
                            APELLIDO_MATERNO = dr[7].ToString(),
                            ID_ROL = dr[8].ToString(),
                            CORREO = dr[10].ToString(),
                            ESTADO = (int)dr[11],
                            USUARIO_CREACION = dr[12].ToString(),
                            USUARIO_MODIFICACION = dr[14].ToString(),
                            NOMBRE_COMPLETO = dr[16].ToString()
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

        public List<usuario> SelectAll_usuario()
        {
            List<usuario> lisData = new List<usuario>();
            SqlCommand cmd = new SqlCommand("SELECT ID_USUARIO, USUARIO_INI, CONTRASENIA_INI FROM sistema.usuario");
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new usuario
                        {
                            ID_USUARIO = (int)dr[0],
                            USUARIO = dr[1].ToString(),
                            CONTRASENIA = dr[2].ToString(),
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

        public bool Update_usuario_encriptado(usuario objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("UPDATE sistema.usuario SET USUARIO = '" + objdato.USUARIO + "' , CONTRASENIA = '" + objdato.CONTRASENIA + "' WHERE ID_USUARIO = " + objdato.ID_USUARIO))
            {
                cmd.CommandType = CommandType.Text;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public usuario Select_usuario_recuperacion(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_recuperacion";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_usuario", objdato.USUARIO);
            cmd.Parameters.AddWithValue("@p_correo", objdato.CORREO);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public usuario Select_usuario_by_token(usuario objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_usuario_by_token";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_token_recuperacion", objdato.TOKEN_RECUPERACION);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool Update_usuario_recuperacion(usuario objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call sistema.usp_upd_usuario_recuperacion(:p_id_usuario, :p_token_recuperacion)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_usuario", NpgsqlDbType.Integer).Value = objdato.ID_USUARIO;
                cmd.Parameters.AddWithValue("p_token_recuperacion", NpgsqlDbType.Text).Value = objdato.TOKEN_RECUPERACION;
                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public DataTable SelectAll_reporte_usuario_logistica_toExport()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_REPORTE_USUARIO_LOGISTICA_TO_EXPORT_ALL");
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
                {
                    if (dr.HasRows)
                    {
                        dt.Columns.Add("USUARIO");
                        dt.Columns.Add("CONTRASEÑA");
                        dt.Columns.Add("DNI");
                        dt.Columns.Add("NOMBRES");
                        dt.Columns.Add("APELLIDO PATERNO");
                        dt.Columns.Add("APELLIDO MATERNO");
                        dt.Columns.Add("CORREO");
                        dt.Columns.Add("ROL");
                        dt.Columns.Add("EQUIPO");


                        while (dr.Read())
                        {
                            DataRow row = dt.NewRow();
                            row["USUARIO"] = HelperAES.DecryptStringAES(dr[0].ToString());
                            row["CONTRASEÑA"] = HelperAES.DecryptStringAES(dr[1].ToString());
                            row["DNI"] = dr[2].ToString();
                            row["NOMBRES"] = dr[3].ToString();
                            row["APELLIDO PATERNO"] = dr[4].ToString();
                            row["APELLIDO MATERNO"] = dr[5].ToString();
                            row["CORREO"] = dr[6].ToString();
                            row["ROL"] = dr[7].ToString();
                            row["EQUIPO"] = dr[8].ToString();
                            dt.Rows.Add(row);
                        }
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public DataTable SelectAll_reporte_usuario_oficina_usuaria_toExport()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_REPORTE_USUARIO_OFICINA_USUARIA_TO_EXPORT_ALL");
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
                {
                    if (dr.HasRows)
                    {
                        dt.Columns.Add("USUARIO");
                        dt.Columns.Add("CONTRASEÑA");
                        dt.Columns.Add("DNI");
                        dt.Columns.Add("NOMBRES");
                        dt.Columns.Add("APELLIDO PATERNO");
                        dt.Columns.Add("APELLIDO MATERNO");
                        dt.Columns.Add("CORREO");
                        dt.Columns.Add("ROL");
                        dt.Columns.Add("OFICINA USUARIA");


                        while (dr.Read())
                        {
                            DataRow row = dt.NewRow();
                            row["USUARIO"] = HelperAES.DecryptStringAES(dr[0].ToString());
                            row["CONTRASEÑA"] = HelperAES.DecryptStringAES(dr[1].ToString());
                            row["DNI"] = dr[2].ToString();
                            row["NOMBRES"] = dr[3].ToString();
                            row["APELLIDO PATERNO"] = dr[4].ToString();
                            row["APELLIDO MATERNO"] = dr[5].ToString();
                            row["CORREO"] = dr[6].ToString();
                            row["ROL"] = dr[7].ToString();
                            row["OFICINA USUARIA"] = dr[8].ToString();
                            dt.Rows.Add(row);
                        }
                        return dt;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


    }
}
