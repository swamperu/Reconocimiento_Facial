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

	public class usuario_rolRepository : Iusuario_rolRepository
	{
	    public JQGrid SelectPaginated_usuario_rol(Parametro objdato)
	    {
	        JQGrid lisGrid = new JQGrid();
	        using (SqlCommand cmd = new SqlCommand("sistema.USP_SEL_USUARIO_ROL_PAG")) {
	            cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_ROL", SqlDbType.Int).Value = objdato.PARAMETRO1;
                cmd.Parameters.Add("@P_DOCUMENTO_IDENTIDAD", SqlDbType.NVarChar).Value = objdato.PARAMETRO2;
                cmd.Parameters.Add("@P_NOMBRE", SqlDbType.NVarChar).Value = objdato.PARAMETRO3;
	            cmd.Parameters.Add("@P_SORT_COLUMN", SqlDbType.NVarChar).Value = objdato.SORT_COLUMN;
	            cmd.Parameters.Add("@P_SORT_ORDER", SqlDbType.NVarChar).Value = objdato.SORT_ORDER;
	            cmd.Parameters.Add("@P_PAGE_SIZE", SqlDbType.Int).Value = objdato.PAGE_SIZE;
	            cmd.Parameters.Add("@P_PAGE_NUMBER", SqlDbType.Int).Value = (objdato.PAGE_NUMBER == 0 ? 1 : objdato.PAGE_NUMBER);
	            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd)) {
	                lisGrid = HelperDao.ReaderJQGrid(dr);
	            }
	        }
	        return lisGrid;
	    }

	    public usuario_rol Select_usuario_rol(usuario_rol objdato)
	    {
	        SqlCommand cmd = new SqlCommand("sistema.USP_SEL_USUARIO_ROL");
	        cmd.CommandType = CommandType.StoredProcedure;
	        cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
	        using (SqlDataReader dr = BaseDatos.GetDataReader(cmd)) {
	            if (dr.HasRows) {
	                dr.Read();
	                return Get_usuario_rol_FromReader(dr);
	            }else{
	                return null;
	            }
	        }
	    }

        public usuario_rol Select_usuario_rol_by_documento(usuario_rol objdato)
        {
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_USUARIO_BY_DOCUMENTO");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_DOCUMENTO_IDENTIDAD", SqlDbType.NVarChar).Value = objdato.DOCUMENTO_IDENTIDAD;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    return Get_usuario_rol_FromReader(dr);
                }
                else
                {
                    return null;
                }
            }
        }

	    private usuario_rol Get_usuario_rol_FromReader(IDataReader dr)
	    {
	        usuario_rol objDato = new usuario_rol();
	        int nrocampos = dr.FieldCount - 1;
	        object[] data = new object[nrocampos + 1];
	        dr.GetValues(data);
	        objDato.ID_USUARIO_ROL = (int) data[0];
	        objDato.ID_USUARIO = (int) data[1];
	        objDato.ID_ROL = (int) data[2];
	        objDato.DOCUMENTO_IDENTIDAD = data[3].ToString();
	        objDato.NOMBRE = data[4].ToString();
	        objDato.ESTADO = (int) data[5];
	        objDato.USUARIO_CREACION = data[6].ToString();
	        if (data[7] != DBNull.Value) objDato.FECHA_CREACION = Convert.ToDateTime(data[7]);
	        objDato.USUARIO_MODIFICACION = data[8].ToString();
	        if (data[9] != DBNull.Value) objDato.FECHA_MODIFICACION = Convert.ToDateTime(data[9]);
            objDato.NOMBRE_ROL = data[10].ToString();
            objDato.CUENTA_DIRECCION = (int)data[11];
            objDato.CUENTA_EJECUTORA = (int)data[12];
            objDato.CUENTA_ZONA = (int)data[13];
	        return objDato;
	    }

	    public bool Insert_usuario_rol(usuario_rol objdato)
	    {
	        bool ind = false;
	        using(SqlCommand cmd = new SqlCommand("sistema.USP_INS_USUARIO_ROL")){
	            cmd.CommandType = CommandType.StoredProcedure;
	            cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
	            cmd.Parameters.Add("@P_ID_ROL", SqlDbType.Int).Value = objdato.ID_ROL;
                cmd.Parameters.Add("@P_NOMBRE_ROL", SqlDbType.NVarChar).Value = objdato.NOMBRE_ROL;
	            cmd.Parameters.Add("@P_DOCUMENTO_IDENTIDAD", SqlDbType.NVarChar).Value = objdato.DOCUMENTO_IDENTIDAD;
	            cmd.Parameters.Add("@P_NOMBRE", SqlDbType.NVarChar).Value = objdato.NOMBRE;
	            ind = (BaseDatos.Execute(cmd) > 0);
	        }
	        return ind;
	    }

	    public bool Update_usuario_rol(usuario_rol objdato)
	    {
	        bool ind = false;
	        using(SqlCommand cmd = new SqlCommand("sistema.USP_UPD_USUARIO_ROL")){
	            cmd.CommandType = CommandType.StoredProcedure;
	            cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
	            cmd.Parameters.Add("@P_ID_ROL", SqlDbType.Int).Value = objdato.ID_ROL;
                cmd.Parameters.Add("@P_NOMBRE_ROL", SqlDbType.NVarChar).Value = objdato.NOMBRE_ROL;
	            cmd.Parameters.Add("@P_DOCUMENTO_IDENTIDAD", SqlDbType.NVarChar).Value = objdato.DOCUMENTO_IDENTIDAD;
	            ind = (BaseDatos.Execute(cmd) > 0);
	        }
	        return ind;
	    }

	    public bool Delete_usuario_rol(usuario_rol objdato)
	    {
	        bool ind = false;
	        using(SqlCommand cmd = new SqlCommand("sistema.USP_DEL_USUARIO_ROL")){
	            cmd.CommandType = CommandType.StoredProcedure;
	            cmd.Parameters.Add("@P_ID_USUARIO_ROL", SqlDbType.Int).Value = objdato.ID_USUARIO_ROL;
	            ind = (BaseDatos.Execute(cmd) > 0);
	        }
	        return ind;
	    }

        public List<usuario_rol> SelectAll_rol()
        {
            List<usuario_rol> lisData = new List<usuario_rol>();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_ROL_ALL");
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new usuario_rol { ID_ROL = (int)dr[0], NOMBRE_ROL = dr[1].ToString() });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
        }

        public JQGrid SelectPaginated_usuario_rol_ejecutora(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            using (SqlCommand cmd = new SqlCommand("sistema.USP_SEL_USUARIO_ROL_EJECUTORA_ALL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.PARAMETRO1;
                using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
                {
                    lisGrid = HelperDao.ReaderJQGrid(dr);
                }
            }
            return lisGrid;
        }

        public JQGrid SelectPaginated_usuario_rol_direccion(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            using (SqlCommand cmd = new SqlCommand("sistema.USP_SEL_USUARIO_ROL_DIRECCION_ALL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.PARAMETRO1;
                using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
                {
                    lisGrid = HelperDao.ReaderJQGrid(dr);
                }
            }
            return lisGrid;
        }

        public JQGrid SelectPaginated_usuario_rol_zona(Parametro objdato)
        {
            JQGrid lisGrid = new JQGrid();
            using (SqlCommand cmd = new SqlCommand("sistema.USP_SEL_USUARIO_ROL_ZONA_ALL"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.PARAMETRO1;
                using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
                {
                    lisGrid = HelperDao.ReaderJQGrid(dr);
                }
            }
            return lisGrid;
        }

        public bool Delete_usuario_rol_ejecutora(usuario_rol objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("sistema.USP_DEL_USUARIO_ROL_EJECUTORA"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Insert_usuario_rol_ejecutora(usuario_rol objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("sistema.USP_INS_USUARIO_ROL_EJECUTORA"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
                cmd.Parameters.Add("@P_SEC_EJEC", SqlDbType.NVarChar).Value = objdato.SEC_EJEC;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.Int).Value = objdato.USUARIO_CREACION ;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Delete_usuario_rol_direccion(usuario_rol objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("sistema.USP_DEL_USUARIO_ROL_DIRECCION"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Insert_usuario_rol_direccion(usuario_rol objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("sistema.USP_INS_USUARIO_ROL_DIRECCION"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
                cmd.Parameters.Add("@P_ID_DIRECCION", SqlDbType.Int).Value = objdato.ID_DIRECCION;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.Int).Value = objdato.USUARIO_CREACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Delete_usuario_rol_zona(usuario_rol objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("sistema.USP_DEL_USUARIO_ROL_ZONA"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }

        public bool Insert_usuario_rol_zona(usuario_rol objdato)
        {
            bool ind = false;
            using (SqlCommand cmd = new SqlCommand("sistema.USP_INS_USUARIO_ROL_ZONA"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
                cmd.Parameters.Add("@P_ID_ZONA", SqlDbType.Int).Value = objdato.ID_ZONA;
                cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.Int).Value = objdato.USUARIO_CREACION;
                ind = (BaseDatos.Execute(cmd) > 0);
            }
            return ind;
        }


        public List<usuario_rol> SelectAll_zona_by_usuario(usuario_rol objdato)
        {
            List<usuario_rol> lisData = new List<usuario_rol>();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_ZONA_BY_USUARIO_ALL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new usuario_rol { ID_ZONA = (int)dr[0], NOMBRE_ZONA = dr[1].ToString() });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<usuario_rol> SelectAll_zona_by_usuario_ejecutora(usuario_rol objdato)
        {
            List<usuario_rol> lisData = new List<usuario_rol>();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_ZONA_BY_USUARIO_EJECUTORA_ALL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new usuario_rol { ID_ZONA = (int)dr[0], NOMBRE_ZONA = dr[1].ToString() });
                    }
                    return lisData;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<usuario_rol> SelectAll_ejecutora_by_usuario_zona_region(usuario_rol objdato)
        {
            List<usuario_rol> lisData = new List<usuario_rol>();
            SqlCommand cmd = new SqlCommand("sistema.USP_SEL_EJECUTORA_BY_USUARIO_ZONA_REGION_ALL");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@P_ID_USUARIO", SqlDbType.Int).Value = objdato.ID_USUARIO;
            cmd.Parameters.Add("@P_ID_ZONA", SqlDbType.Int).Value = objdato.ID_ZONA;
            cmd.Parameters.Add("@P_NOMBRE_REGION", SqlDbType.NVarChar).Value = objdato.NOMBRE_REGION;
            using (SqlDataReader dr = BaseDatos.GetDataReader(cmd))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lisData.Add(new usuario_rol { SEC_EJEC  = dr[0].ToString(), NOMBRE_EJECUTORA  = dr[1].ToString() });
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
