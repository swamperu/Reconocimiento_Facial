using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SCONT.Dominio.Entidades.Sistema;
using SCONT.Dominio.Entidades.Custom;
using SCONT.Dominio.Contratos;
using Npgsql;

namespace SCONT.Infraestructura.Dao
{
    using SCONT.Infraestructura.BaseDatos;
    using SCONT.Infraestructura.Dao.Helper;

    /// <summary>
    /// Nombre       : recursoRepository
    /// Descripción  : Métodos de trabajo para recursoRepository
    /// </summary>
    /// <remarks>
    /// Creacion     : 11/08/2017 STC.TOOL.WIN
    /// Modificación :  
    /// </remarks>
    public class recursoRepository : IrecursoRepository
    {
        public List<recurso> SelectAll_recurso(recurso objdato)
        {
            List<recurso> lisData = new List<recurso>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "sistema.fn_sel_recurso_all";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("p_id_rol", objdato.ID_ROL);
            cmd.CommandType = CommandType.StoredProcedure;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd)) {
                if (dr.HasRows) {
                    while (dr.Read()){
                        lisData.Add(Get_recurso_FromReader(dr));
                    }
                    return lisData;
                } else {
                    return null;
                }
            }
        }

        private recurso Get_recurso_FromReader(IDataReader dr)
        {
            recurso objrecurso = new recurso();
            int nrocampos = dr.FieldCount - 1;
            object[] data = new object[nrocampos + 1];
            dr.GetValues(data);
            objrecurso.ID_RECURSO = (int) data[0];
            objrecurso.CODIGO = data[1].ToString();
            objrecurso.NOMBRE = data[2].ToString();
            objrecurso.TIPO = data[3].ToString();
            objrecurso.URL = data[4].ToString();
            objrecurso.RELACION = data[5].ToString();
            //objrecurso.TABLA = data[6].ToString();
            objrecurso.IMAGEN = data[6].ToString();
            objrecurso.DESCRIPCION = data[7].ToString();
            objrecurso.GRUPO = data[8].ToString();
            objrecurso.GRUPO_NOMBRE = data[9].ToString();
            objrecurso.ESTADO = (int) data[10];
            objrecurso.USUARIO_CREACION = data[11].ToString();
            if (data[12] != DBNull.Value) objrecurso.FECHA_CREACION = Convert.ToDateTime(data[12]);
            objrecurso.USUARIO_MODIFICACION = data[13].ToString();
            if (data[14] != DBNull.Value) objrecurso.FECHA_MODIFICACION = Convert.ToDateTime(data[14]);
            objrecurso.ORDEN = int.Parse(data[15].ToString());
            return objrecurso;
        }

        public Parametro Select_fecha_servidor()
        {
            Parametro datFecha = new Parametro();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT now() ");
            cmd.CommandType = CommandType.Text;

            using (NpgsqlDataReader dr = BaseDatosPSQL.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr[0] != DBNull.Value) datFecha.FECHA_SERVIDOR = Convert.ToDateTime(dr[0]);
                    return datFecha;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
