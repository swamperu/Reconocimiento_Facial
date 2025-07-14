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

    public class marcacionRepository : ImarcacionRepository
    {
        public JQGrid SelectPaginated_marcacion(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            JQGrid datGrid = new JQGrid();
            using (NpgsqlCommand cmd = new NpgsqlCommand("transaccional.fn_sel_marcacion_pag"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO1);
                cmd.Parameters.AddWithValue("p_fecha_inicio", objdato.FECHA_DESDE_YYYYMMDD);
                cmd.Parameters.AddWithValue("p_fecha_fin", objdato.FECHA_HASTA_YYYYMMDD);
                cmd.Parameters.AddWithValue("p_page_size", objdato.PAGE_SIZE);
                cmd.Parameters.AddWithValue("p_page_number", (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER));

                using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
                {
                    datGrid = SelectPaginated_marcacion_datos(objdato);
                    lisGrid = HelperDao.ReaderJQGridPSQL(dr, datGrid);
                }
            }
            return lisGrid;
        }

        JQGrid SelectPaginated_marcacion_datos(Parametro objdato)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "transaccional.fn_sel_marcacion_pag_datos";
            cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO1);
            cmd.Parameters.AddWithValue("p_fecha_inicio", objdato.FECHA_DESDE_YYYYMMDD);
            cmd.Parameters.AddWithValue("p_fecha_fin", objdato.FECHA_HASTA_YYYYMMDD);
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

        public bool Insert_marcacion(marcacion objdato)
        {
            bool ind = false;
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "transaccional.fn_ins_marcacion";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_numero_dni", objdato.NUMERO_DNI);
            cmd.Parameters.AddWithValue("p_fecha_hora", objdato.FECHA_HORA);
            cmd.Parameters.AddWithValue("p_ingreso_salida", objdato.INGRESO_SALIDA);
            cmd.Parameters.AddWithValue("p_usuario_creacion", objdato.USUARIO_CREACION);
            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    objdato.ID_MARCACION = int.Parse(dr[0].ToString());
                }
            }
            ind = (objdato.ID_MARCACION > 0);
            return ind;
        }

        public bool Delete_marcacion(marcacion objdato)
        {
            bool ind = false;
            using (NpgsqlCommand cmd = new NpgsqlCommand("call transaccional.usp_del_marcacion(:p_id_marcacion, :p_usuario_modificacion)"))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("p_id_marcacion", NpgsqlDbType.Integer).Value = objdato.ID_MARCACION;
                cmd.Parameters.AddWithValue("p_usuario_modificacion", NpgsqlDbType.Varchar).Value = objdato.USUARIO_MODIFICACION;
                ind = (BaseDatosPSQL.Execute(cmd) > 0);
            }
            return ind;
        }

        public DataTable SelectAll_reporte_horas_trabajadas_toExport(Parametro objdato)
        {
            DataSet dataset = new DataSet();
            NpgsqlCommand cmd = new NpgsqlCommand("transaccional.fn_sel_reporte_horas_trabajadas_to_export");
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_dni", objdato.PARAMETRO2);
                cmd.Parameters.AddWithValue("p_fecha_inicio", objdato.FECHA_DESDE);
                cmd.Parameters.AddWithValue("p_fecha_fin", objdato.FECHA_HASTA);
                //cmd.CommandTimeout = 999999999;
                dataset = BaseDatosPSQL.GetDataSet(cmd);
            }
            return (dataset.Tables.Count > 0 ? dataset.Tables[0] : null);
        }

    }
}
