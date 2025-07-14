using SCONT.Dominio.Entidades.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.Runtime.Serialization;
using SCONT.Dominio.Entidades;
using SCONT.Dominio.Entidades.Sistema;

namespace SCONT.Infraestructura.Transversal
{
    public class sisSesion
    {
        #region Propiedades

        public static string GetToken
        {
            get
            {
                string objToken = (string) Obtener(sisConstante.Session_Token);
                return objToken;
            }
        }
        public static string CodUsuario
        {
            get
            {
                usuario objUsuario = (usuario) Obtener(sisConstante.Session_Usuario);
                if (objUsuario == null)
                {
                    return "";
                }
                else
                {
                    return objUsuario.ID_USUARIO.ToString();
                }
            }
        }
        public static object Usuario
        {
            get { return Obtener(sisConstante.Session_Usuario); }
            set { Asignar(sisConstante.Session_Usuario, value); }
        }
        public static object Menus
        {
            get { return Obtener(sisConstante.Session_Menus); }
            set { Asignar(sisConstante.Session_Menus, value); }
        }
        public static object Forms
        {
            get { return Obtener(sisConstante.Session_Forms); }
            set { Asignar(sisConstante.Session_Forms, value); }
        }
        public static object Metodos
        {
            get { return Obtener(sisConstante.Session_Methods); }
            set { Asignar(sisConstante.Session_Methods, value); }
        }
        public static object Recursos
        {
            get { return Obtener(sisConstante.Session_Resources); }
            set { Asignar(sisConstante.Session_Resources, value); }
        }
        public static object Roles
        {
            get { return Obtener(sisConstante.Session_Roles); }
            set { Asignar(sisConstante.Session_Roles, value); }
        }
        public static object RolPrincipal
        {
            get { return Obtener(sisConstante.Session_RolPrincipal); }
            set { Asignar(sisConstante.Session_RolPrincipal, value); }
        }

        public static object Reporte_Parametro
        {
            get { return Obtener(sisConstante.Session_Reporte_Parametro); }
            set { Asignar(sisConstante.Session_Reporte_Parametro, value); }
        }

        public static object Lista_Reporte
        {
            get { return Obtener(sisConstante.Session_Lista_Reporte ); }
            set { Asignar(sisConstante.Session_Lista_Reporte , value); }
        }

        public static object Lista_Clasificador_Armada
        {
            get { return Obtener(sisConstante.Session_Lista_Data); }
            set { Asignar(sisConstante.Session_Lista_Data, value); }
        }

        public static object Lista_Clasificador_Adelanto
        {
            get { return Obtener(sisConstante.Session_Lista_Adelanto); }
            set { Asignar(sisConstante.Session_Lista_Adelanto, value); }
        }

        public static object Lista_Incidencia_Armada_Item
        {
            get { return Obtener(sisConstante.Session_Lista_Incidencia_Armada_Item); }
            set { Asignar(sisConstante.Session_Lista_Incidencia_Armada_Item, value); }
        }

        public static object Codigo
        {
            get { return Obtener(sisConstante.Session_Codigo); }
            set { Asignar(sisConstante.Session_Codigo, value); }
        }

        public static object Codigo2
        {
            get { return Obtener(sisConstante.Session_Codigo_2); }
            set { Asignar(sisConstante.Session_Codigo_2, value); }
        }

        public static object Tab_Seleccionado
        {
            get { return Obtener(sisConstante.Sesion_Tab_Seleccionado); }
            set { Asignar(sisConstante.Sesion_Tab_Seleccionado, value); }
        }
        #endregion
        #region MetodosPrivados
        private static void Asignar(string nomRecurso, object valor)
        {
            HttpContext.Current.Session[nomRecurso] = valor;
        }

        private static object Obtener(string nomRecurso)
        {
            if (HttpContext.Current.Session[nomRecurso] == null)
            {
                Asignar(nomRecurso, null);
            }
            return HttpContext.Current.Session[nomRecurso];
        }
        #endregion
        #region MetodosPublicos

        public static string NewToken()
        {
            string token = (HttpContext.Current.Request.UserHostAddress.ToString() + HttpContext.Current.Request.UserAgent.ToString() + Guid.NewGuid().ToString()).GetHashCode().ToString();
            Asignar(sisConstante.Session_Token, token);
            return token;
        }
        public static bool SesionEsValida()
        {
            return (Usuario != null);
        }
        public static bool CorreoEsPrueba()
        {
            return ConfigurationManager.AppSettings["Correo_Modo_Prueba"].ToUpper().Equals("S");
        }
        public static string Correo_Rol_Direccion()
        {
            return ConfigurationManager.AppSettings["Correo_Direccion"];
        }
        public static string Correo_Rol_Admin()
        {
            return ConfigurationManager.AppSettings["Correo_Admin"];
        }

        public static void RedirectToLoginPage(){
            FormsAuthentication.RedirectToLoginPage();
        }

        public static void RedirectToAccesoNoAutorizado(){
            HttpContext.Current.Response.Redirect(sisConstante.Url_Acceso_Denegado);
        }

        public static bool  FormularioValido(string ruta){
            //Dim lisRecursos = From x In Forms Where urlRecurso.ToUpper.Contains(x.dirUrl.ToUpper)
            /*List<recurso> listaOpciones = (List<recurso>)sisSesion.Menus;
             if (listaOpciones != null) {
                 List<recurso> listaRecursos = (from x in listaOpciones where ruta.Contains(x.URL) select x).ToList();
                 return (listaRecursos.Count > 0);
             }
             else
             {
                 return false;
             }*/
            usuario datUsuario = (usuario)sisSesion.Usuario;

            List<recurso> listaOpciones = (List<recurso>)sisSesion.Menus;
            if (listaOpciones != null) {
                if (ruta.ToUpper().Contains("INDEX"))
                {
                    List<recurso> listaRecursos = (from x in listaOpciones where ruta.Contains(x.URL) && x.TIPO == "M" select x).ToList();
                    return (listaRecursos.Count > 0);
                }
                else
                {
                    //es un metodo
                    return true;
                }
            }
            else
            {
                return false;
            }

        }

        #endregion
    }
}
