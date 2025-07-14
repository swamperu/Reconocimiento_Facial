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

    public class trabajadorRepository : ItrabajadorRepository
    {
        public JQGrid SelectPaginated_trabajador(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            JQGrid datGrid = new JQGrid();
            using (NpgsqlCommand cmd = new NpgsqlCommand("maestro.fn_sel_trabajador_pag"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO1);
                cmd.Parameters.AddWithValue("p_apellido_paterno", objdato.PARAMETRO2);
                cmd.Parameters.AddWithValue("p_apellido_materno", objdato.PARAMETRO3);
                cmd.Parameters.AddWithValue("p_page_size", objdato.PAGE_SIZE);
                cmd.Parameters.AddWithValue("p_page_number", (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER));

                using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
                {
                    datGrid = SelectPaginated_trabajador_datos(objdato);
                    lisGrid = HelperDao.ReaderJQGridPSQL(dr, datGrid);
                }
            }
            return lisGrid;
        }

        JQGrid SelectPaginated_trabajador_datos(Parametro objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_trabajador_pag_datos";
            cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO1);
            cmd.Parameters.AddWithValue("p_apellido_paterno", objdato.PARAMETRO2);
            cmd.Parameters.AddWithValue("p_apellido_materno", objdato.PARAMETRO3);
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

        public trabajador Select_trabajador(trabajador objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_trabajador";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_id_trabajador", objdato.ID_TRABAJADOR);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_trabajador_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        public trabajador Select_trabajador_by_dni(trabajador objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_sel_trabajador_by_dni";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_id_trabajador", objdato.ID_TRABAJADOR);
            cmd.Parameters.AddWithValue("p_numero_dni", objdato.NUMERO_DNI);

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_trabajador_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

        private trabajador Get_trabajador_FromReader(IDataReader dr)
        {
            trabajador objDato = new trabajador();
            int nrocampos = dr.FieldCount - 1;
            object[] data = new object[nrocampos + 1];
            dr.GetValues(data);
            objDato.ID_TRABAJADOR = (int)data[0];
            objDato.NUMERO_DNI = data[1].ToString();
            objDato.NOMBRES = data[2].ToString();
            objDato.APELLIDO_PATERNO = data[3].ToString();
            objDato.APELLIDO_MATERNO = data[4].ToString();
            objDato.ID_GENERO = data[5].ToString();
            if (data[6] != DBNull.Value) objDato.FECHA_NACIMIENTO = Convert.ToDateTime(data[6]);
            if (data[6] != DBNull.Value) objDato.FECHA_NACIMIENTO_TEXTO = objDato.FECHA_NACIMIENTO.Value.ToString("dd/MM/yyyy");
            objDato.CORREO_ELECTRONICO = data[7].ToString();
            objDato.TELEFONO = data[8].ToString();
            if (data[9] != DBNull.Value) objDato.FECHA_INGRESO = Convert.ToDateTime(data[9]);
            if (data[9] != DBNull.Value) objDato.FECHA_INGRESO_TEXTO = objDato.FECHA_INGRESO.Value.ToString("dd/MM/yyyy");
            objDato.DEP_CODIGO = data[10].ToString();
            objDato.PRV_CODIGO = data[11].ToString();
            objDato.DIS_CODIGO = data[12].ToString();
            objDato.DIRECCION = data[13].ToString();
            objDato.ESTADO = (int)data[14];
            objDato.USUARIO_CREACION = data[15].ToString();
            if (data[16] != DBNull.Value) objDato.FECHA_CREACION = Convert.ToDateTime(data[16]);
            objDato.USUARIO_MODIFICACION = data[17].ToString();
            if (data[18] != DBNull.Value) objDato.FECHA_MODIFICACION = Convert.ToDateTime(data[18]);
            objDato.INICIA_CAPTURA = data[19].ToString();
            objDato.ID_HORARIO_LABORAL = data[20].ToString();
            return objDato;
        }

        public bool Insert_trabajador(trabajador objdato)
        {
            bool ind = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "maestro.fn_ins_trabajador";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_numero_dni", objdato.NUMERO_DNI);
            cmd.Parameters.AddWithValue("p_nombres", objdato.NOMBRES);
            cmd.Parameters.AddWithValue("p_apellido_paterno", objdato.APELLIDO_PATERNO);
            cmd.Parameters.AddWithValue("p_apellido_materno", objdato.APELLIDO_MATERNO);
            cmd.Parameters.AddWithValue("p_id_genero", objdato.ID_GENERO);
            cmd.Parameters.AddWithValue("p_id_horario_laboral", objdato.ID_HORARIO_LABORAL);
            cmd.Parameters.AddWithValue("p_correo_electronico", objdato.CORREO_ELECTRONICO);
            cmd.Parameters.AddWithValue("p_telefono", objdato.TELEFONO);
            cmd.Parameters.AddWithValue("p_dep_codigo", objdato.DEP_CODIGO);
            cmd.Parameters.AddWithValue("p_prv_codigo", objdato.PRV_CODIGO);
            cmd.Parameters.AddWithValue("p_dis_codigo", objdato.DIS_CODIGO);
            cmd.Parameters.AddWithValue("p_direccion", objdato.DIRECCION);
            cmd.Parameters.AddWithValue("p_usuario_creacion", objdato.USUARIO_CREACION);
            if (objdato.FECHA_NACIMIENTO != null)
            {
                cmd.Parameters.AddWithValue("p_fecha_nacimiento", objdato.FECHA_NACIMIENTO);
            }
            else
            {
                cmd.Parameters.AddWithValue("p_fecha_nacimiento", DBNull.Value);
            }
            if (objdato.FECHA_INGRESO != null)
            {
                cmd.Parameters.AddWithValue("p_fecha_ingreso", objdato.FECHA_INGRESO);
            }
            else
            {
                cmd.Parameters.AddWithValue("p_fecha_ingreso", DBNull.Value);
            }

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    objdato.ID_TRABAJADOR = int.Parse(dr[0].ToString());
                }
            }
            ind = (objdato.ID_TRABAJADOR > 0);
            return ind;
        }

        public bool Update_trabajador(trabajador objdato)
        {
            bool ind = false;
            string cadena = "call maestro.usp_upd_trabajador(:p_id_trabajador, :p_numero_dni, :p_nombres, :p_apellido_paterno, :p_apellido_materno, :p_id_genero, :p_id_horario_laboral, :p_correo_electronico, :p_telefono, :p_dep_codigo, :p_prv_codigo, :p_dis_codigo, :p_direccion, :p_usuario_modificacion, :p_fecha_nacimiento , :p_fecha_ingreso )";

            using (NpgsqlCommand cmd = new NpgsqlCommand(cadena))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_trabajador", NpgsqlDbType.Integer).Value = objdato.ID_TRABAJADOR;
                cmd.Parameters.AddWithValue("p_numero_dni", NpgsqlDbType.Varchar).Value = objdato.NUMERO_DNI;
                cmd.Parameters.AddWithValue("p_nombres", NpgsqlDbType.Varchar).Value = objdato.NOMBRES;
                cmd.Parameters.AddWithValue("p_apellido_paterno", NpgsqlDbType.Varchar).Value = objdato.APELLIDO_PATERNO;
                cmd.Parameters.AddWithValue("p_apellido_materno", NpgsqlDbType.Varchar).Value = objdato.APELLIDO_MATERNO;
                cmd.Parameters.AddWithValue("p_id_genero", NpgsqlDbType.Varchar).Value = objdato.ID_GENERO;
                cmd.Parameters.AddWithValue("p_id_horario_laboral", NpgsqlDbType.Varchar).Value = objdato.ID_HORARIO_LABORAL;
                cmd.Parameters.AddWithValue("p_correo_electronico", NpgsqlDbType.Varchar).Value = objdato.CORREO_ELECTRONICO;
                cmd.Parameters.AddWithValue("p_telefono", NpgsqlDbType.Varchar).Value = objdato.TELEFONO;
                cmd.Parameters.AddWithValue("p_dep_codigo", NpgsqlDbType.Varchar).Value = objdato.DEP_CODIGO;
                cmd.Parameters.AddWithValue("p_prv_codigo", NpgsqlDbType.Varchar).Value = objdato.PRV_CODIGO;
                cmd.Parameters.AddWithValue("p_dis_codigo", NpgsqlDbType.Varchar).Value = objdato.DIS_CODIGO;
                cmd.Parameters.AddWithValue("p_direccion", NpgsqlDbType.Varchar).Value = objdato.DIRECCION;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;
                
                if (objdato.FECHA_NACIMIENTO != null)
                {
                    cmd.Parameters.AddWithValue("p_fecha_nacimiento", NpgsqlDbType.Date).Value = objdato.FECHA_NACIMIENTO;
                }
                else
                {
                    cmd.Parameters.AddWithValue("p_fecha_nacimiento", NpgsqlDbType.Date).Value = DBNull.Value;
                }
                if (objdato.FECHA_INGRESO != null)
                {
                    cmd.Parameters.AddWithValue("p_fecha_ingreso", NpgsqlDbType.Date).Value = objdato.FECHA_INGRESO;
                }
                else
                {
                    cmd.Parameters.AddWithValue("p_fecha_ingreso", NpgsqlDbType.Date).Value = DBNull.Value;
                }

                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Update_trabajador_captura(trabajador objdato)
        {
            bool ind = false;
            string cadena = "call maestro.usp_upd_trabajador_captura(:p_id_trabajador)";

            using (NpgsqlCommand cmd = new NpgsqlCommand(cadena))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_trabajador", NpgsqlDbType.Integer).Value = objdato.ID_TRABAJADOR;
                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }


        public bool Delete_trabajador(trabajador objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call maestro.usp_del_trabajador(:p_id_trabajador, :p_usuario_modificacion)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_trabajador", NpgsqlDbType.Integer).Value = objdato.ID_TRABAJADOR;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;
                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

    }
}
