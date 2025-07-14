using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Transactions;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Contratos;
using SCONT.Aplicacion.Contratos;
using SCONT.Aplicacion.Servicios;
using SCONT.Infraestructura.Dao;
using SCONT.Infraestructura.Transversal;
using SCONT.Dominio.Entidades.Custom;
using System.Text.RegularExpressions;
using System.Data;

namespace SCONT.Aplicacion.Servicios
{
    public class usuarioService : IusuarioService
    {
        private IusuarioRepository _usuarioRepository;
        
        public usuarioService()
        {
            _usuarioRepository = new usuarioRepository();
        }

        public DatoRetorno<JQGrid> SelectPaginated_usuario(Parametro dato)
        {
            DatoRetorno<JQGrid> datRetorno = new DatoRetorno<JQGrid>();
            datRetorno.Dato = _usuarioRepository.SelectPaginated_usuario(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectPaginated_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_by_correo(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario_by_correo(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_by_dni(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario_by_dni(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_login(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario_login(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_recuperacion(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            dato.USUARIO = HelperAES.EncryptStringAES(dato.USUARIO.ToUpper());
            datRetorno.Dato = _usuarioRepository.Select_usuario_recuperacion(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_by_usuario(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario_by_usuario(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_by_token(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario_by_token(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<usuario> Select_usuario_by_datos(usuario dato)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Dato = _usuarioRepository.Select_usuario_by_datos(dato);
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.Select_Error : "");
            return datRetorno;
        }

        public DatoRetorno<bool> Insert_usuario(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            usuario datExist = _usuarioRepository.Select_usuario_by_usuario(new usuario { ID_USUARIO = 0, USUARIO = HelperAES.EncryptStringAES(dato.USUARIO.ToUpper()) });
            if (datExist != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Ya existe un registro con el mismo USUARIO, verifique";
                return datRetorno;
            }

            datExist = _usuarioRepository.Select_usuario_by_dni(new usuario { ID_USUARIO = 0, DNI = dato.DNI });
            if (datExist != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Ya existe un registro con el mismo DNI, verifique";
                return datRetorno;
            }

            datExist = _usuarioRepository.Select_usuario_by_datos(new usuario { ID_USUARIO = 0, NOMBRES = dato.NOMBRES, APELLIDO_PATERNO = dato.APELLIDO_PATERNO, APELLIDO_MATERNO = dato.APELLIDO_MATERNO });
            if (datExist != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Ya existe un registro con los mismos NOMBRES y APELLIDOS, verifique";
                return datRetorno;
            }

            //datExist = _usuarioRepository.Select_usuario_by_correo(new usuario { ID_USUARIO = 0, CORREO = dato.CORREO });
            //if (datExist != null)
            //{
            //    datRetorno.Dato = false;
            //    datRetorno.Msg = "Ya existe un registro con el mismo CORREO ELECTRÓNICO, verifique";
            //    return datRetorno;
            //}

            DatoRetorno<bool> datValidarContrasenia = new DatoRetorno<bool>();
            datValidarContrasenia = ContrasenaSegura(dato.CONTRASENIA);
            if (datValidarContrasenia.Dato == false)
            {
                datRetorno = datValidarContrasenia;
                return datRetorno;
            }

            dato.USUARIO = HelperAES.EncryptStringAES(dato.USUARIO.ToUpper());
            dato.CONTRASENIA = HelperAES.EncryptStringAES(dato.CONTRASENIA);

            datRetorno.Dato = _usuarioRepository.Insert_usuario(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Insert_Error : sisMensaje.Insert_Ok);
            using (TransactionScope tran = new TransactionScope())
            {
                tran.Complete();
            }
            return datRetorno;
        }

        public DatoRetorno<bool> Update_usuario(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            usuario datUsuario = _usuarioRepository.Select_usuario(dato);

            usuario datExist = _usuarioRepository.Select_usuario_by_usuario(new usuario { ID_USUARIO = dato.ID_USUARIO, USUARIO = HelperAES.EncryptStringAES(dato.USUARIO.ToUpper()) });
            if (datExist != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Ya existe un registro con el mismo USUARIO, verifique";
                return datRetorno;
            }

            datExist = _usuarioRepository.Select_usuario_by_dni(new usuario { ID_USUARIO = dato.ID_USUARIO, DNI = dato.DNI });
            if (datExist != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Ya existe un registro con el mismo DNI, verifique";
                return datRetorno;
            }

            datExist = _usuarioRepository.Select_usuario_by_datos(new usuario { ID_USUARIO = dato.ID_USUARIO, NOMBRES = dato.NOMBRES, APELLIDO_PATERNO = dato.APELLIDO_PATERNO, APELLIDO_MATERNO = dato.APELLIDO_MATERNO });
            if (datExist != null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "Ya existe un registro con los mismos NOMBRES y APELLIDOS, verifique";
                return datRetorno;
            }

            //datExist = _usuarioRepository.Select_usuario_by_correo(new usuario { ID_USUARIO = dato.ID_USUARIO, CORREO = dato.CORREO });
            //if (datExist != null)
            //{
            //    datRetorno.Dato = false;
            //    datRetorno.Msg = "Ya existe un registro con el mismo CORREO ELECTRÓNICO, verifique";
            //    return datRetorno;
            //}

            DatoRetorno<bool> datValidarContrasenia = new DatoRetorno<bool>();
            datValidarContrasenia = ContrasenaSegura(dato.CONTRASENIA);
            if (datValidarContrasenia.Dato == false)
            {
                datRetorno = datValidarContrasenia;
                return datRetorno;
            }

            string contra_decript = dato.CONTRASENIA;
            dato.USUARIO = HelperAES.EncryptStringAES(dato.USUARIO.ToUpper());
            dato.CONTRASENIA = HelperAES.EncryptStringAES(dato.CONTRASENIA);

            datRetorno.Dato = _usuarioRepository.Update_usuario(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            using (TransactionScope tran = new TransactionScope())
            {
                tran.Complete();
            }
            return datRetorno;
        }

        public DatoRetorno<bool> Update_usuario_ingreso(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _usuarioRepository.Update_usuario_ingreso(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            return datRetorno;
        }

        public DatoRetorno<bool> Update_usuario_recuperacion(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _usuarioRepository.Update_usuario_recuperacion(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            return datRetorno;
        }

        public DatoRetorno<bool> Delete_usuario(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = _usuarioRepository.Delete_usuario(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Delete_Error : sisMensaje.Delete_Ok);
            return datRetorno;
        }

        public DatoRetorno<bool> Update_usuario_password(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            usuario datUsuario = _usuarioRepository.Select_usuario(dato);
            if (datUsuario == null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "El usuario no existe";
                return datRetorno;
            }
            if (dato.CONTRASENIA != datUsuario.CONTRASENIA)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "La CONTRASEÑA ACTUAL no es correcta";
                return datRetorno;
            }
            if (dato.CONTRASENIA == dato.NUEVA_CONTRASENIA)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "La NUEVA CONTRASEÑA debe ser diferente a la CONTRASEÑA ACTUAL";
                return datRetorno;
            }
            DatoRetorno<bool> datValidarContrasenia = new DatoRetorno<bool>();
            datValidarContrasenia = ContrasenaSegura(dato.NUEVA_CONTRASENIA, true);
            if (datValidarContrasenia.Dato == false)
            {
                datRetorno = datValidarContrasenia;
                return datRetorno;
            }
            if (dato.NUEVA_CONTRASENIA != dato.CONFIRMAR_NUEVA_CONTRASENIA)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "La NUEVA CONTRASEÑA no coincide con la CONFIRMACIÓN";
                return datRetorno;
            }

            dato.NUEVA_CONTRASENIA = HelperAES.EncryptStringAES(dato.NUEVA_CONTRASENIA);
            datRetorno.Dato = _usuarioRepository.Update_usuario_password(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            //Guarda el historial de contraseñas
            if (datRetorno.Dato)
            {
            }
            return datRetorno;
        }

        public DatoRetorno<bool> Update_usuario_password_recuperacion(usuario dato)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            usuario datUsuario = _usuarioRepository.Select_usuario_by_token(dato);
            if (datUsuario == null)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "El TOKEN de recuperación es incorrecto";
                return datRetorno;
            }
            DatoRetorno<bool> datValidarContrasenia = new DatoRetorno<bool>();
            datValidarContrasenia = ContrasenaSegura(dato.NUEVA_CONTRASENIA, true);
            if (datValidarContrasenia.Dato == false)
            {
                datRetorno = datValidarContrasenia;
                return datRetorno;
            }
            if (dato.NUEVA_CONTRASENIA != dato.CONFIRMAR_NUEVA_CONTRASENIA)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "La NUEVA CONTRASEÑA no coincide con la CONFIRMACIÓN";
                return datRetorno;
            }

            dato.ID_USUARIO = datUsuario.ID_USUARIO;
            dato.CONTRASENIA = "";
            dato.NUEVA_CONTRASENIA = HelperAES.EncryptStringAES(dato.NUEVA_CONTRASENIA);
            dato.USUARIO_MODIFICACION = "0";
            datRetorno.Dato = _usuarioRepository.Update_usuario_password(dato);
            datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
            //Guarda el historial de contraseñas
            if (datRetorno.Dato)
            {
            }
            return datRetorno;
        }

        public DatoRetorno<bool> ContrasenaSegura(string contraseñaSinVerificar, bool es_cambiar_contrasenia = false)
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();
            datRetorno.Dato = true;

            if (contraseñaSinVerificar.Length < 8)
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "El campo " + (es_cambiar_contrasenia ? "NUEVA" : "") + " CONTRASEÑA debe tener como mínimo 8 caracteres.";
                return datRetorno;
            }

            if (contraseñaSinVerificar.Contains(" "))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "La " + (es_cambiar_contrasenia ? "NUEVA" : "") + " CONTRASEÑA no debe contener espacios.";
                return datRetorno;
            }

            //letras de la A a la Z, mayusculas y minusculas
            Regex letras = new Regex(@"[A-Z]");
            //letras de la A a la Z, mayusculas y minusculas
            Regex letrasMin = new Regex(@"[a-z]");
            //digitos del 0 al 9
            Regex numeros = new Regex(@"[0-9]");
            //cualquier caracter del conjunto
            Regex caracEsp = new Regex("[°!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");

            //si no contiene las letras, regresa false
            if (!letras.IsMatch(contraseñaSinVerificar))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "LA " + (es_cambiar_contrasenia ? "NUEVA" : "") + " CONTRASEÑA debe contener por lo menos una letra MAYÚSCULA.";
                return datRetorno;
            }
            //si no contiene las letras, regresa false
            if (!letrasMin.IsMatch(contraseñaSinVerificar))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "LA " + (es_cambiar_contrasenia ? "NUEVA" : "") + " CONTRASEÑA debe contener por lo menos una letra MINÚSCULA.";
                return datRetorno;
            }
            //si no contiene los numeros, regresa false
            if (!numeros.IsMatch(contraseñaSinVerificar))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "LA " + (es_cambiar_contrasenia ? "NUEVA" : "") + " CONTRASEÑA debe contener por lo menos un NÚMERO.";
                return datRetorno;
            }

            //si no contiene los caracteres especiales, regresa false
            if (!caracEsp.IsMatch(contraseñaSinVerificar))
            {
                datRetorno.Dato = false;
                datRetorno.Msg = "LA " + (es_cambiar_contrasenia ? "NUEVA" : "") + " CONTRASEÑA debe contener por lo menos un CARACTER ESPECIAL.";
                return datRetorno;
            }

            //si cumple con todo, regresa true
            return datRetorno;
        }

        public DatoRetorno<usuario> SelectAll_usuario_by_rol(string id_rol)
        {
            DatoRetorno<usuario> datRetorno = new DatoRetorno<usuario>();
            datRetorno.Lista = _usuarioRepository.SelectAll_usuario_by_rol(id_rol);
            datRetorno.Msg = (datRetorno.Lista == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;

        }

        public DatoRetorno<bool> Update_usuario_encriptado()
        {
            DatoRetorno<bool> datRetorno = new DatoRetorno<bool>();

            using (TransactionScope tran = new TransactionScope())
            {
                List<usuario> lisData = _usuarioRepository.SelectAll_usuario();
                if (lisData != null)
                {
                    foreach (usuario dato in lisData)
                    {
                        dato.USUARIO = HelperAES.EncryptStringAES(dato.USUARIO.ToUpper());
                        dato.CONTRASENIA = HelperAES.EncryptStringAES(dato.CONTRASENIA);
                        datRetorno.Dato = _usuarioRepository.Update_usuario_encriptado(dato);
                        datRetorno.Msg = (datRetorno.Dato == false ? sisMensaje.Update_Error : sisMensaje.Update_Ok);
                    }
                }
                tran.Complete();
            }

            return datRetorno;
        }

        public DatoRetorno<DataTable> SelectAll_reporte_usuario_logistica_toExport()
        {
            DatoRetorno<DataTable> datRetorno = new DatoRetorno<DataTable>();
            datRetorno.Dato = _usuarioRepository.SelectAll_reporte_usuario_logistica_toExport();
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;

        }

        public DatoRetorno<DataTable> SelectAll_reporte_usuario_oficina_usuaria_toExport()
        {
            DatoRetorno<DataTable> datRetorno = new DatoRetorno<DataTable>();
            datRetorno.Dato = _usuarioRepository.SelectAll_reporte_usuario_oficina_usuaria_toExport();
            datRetorno.Msg = (datRetorno.Dato == null ? sisMensaje.SelectAll_Error : "");
            return datRetorno;

        }

    }
}
