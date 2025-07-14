using SCONT.Dominio.Entidades.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Drawing;
using SCONT.Infraestructura.Dao;
using System.Globalization;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using SCONT.Infraestructura.Dao.Helper;
using System.Runtime.Caching;
using System.Net;
using SCONT.Dominio.Entidades.Parametrica;

namespace SCONT.Infraestructura.Transversal
{
    public static class sisFuncion
    {
        public static String ToProper(String s)
        {
            if (s == null) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }

        public static List<JQCombo> ListToJQCombo<T>(string id, string valor, List<T> lst)
        {
            List<JQCombo> lstJqCombo = new List<JQCombo>();
            if ((lst != null))
            {
                if (lst.Count > 0)
                {
                    Type tipo = lst[0].GetType();
                    foreach (object obj in lst)
                    {
                        JQCombo itemCombo = new JQCombo();
                        PropertyInfo propId = tipo.GetProperty(id);
                        itemCombo.Id = ((propId == null) ? string.Empty : propId.GetValue(obj, null)).ToString();
                        PropertyInfo propValor = tipo.GetProperty(valor);
                        itemCombo.Valor = ((propValor == null) ? string.Empty : propValor.GetValue(obj, null)).ToString();
                        lstJqCombo.Add(itemCombo);
                    }
                }
            }
            return lstJqCombo;
        }
        public static string GetImageToBase64String(string pathImage)
        {
            var result = "";

            if (File.Exists(pathImage))
            {
                var bitmap = new Bitmap(pathImage);
                var bitmapBytes = BitmapToBytes(bitmap);
                result = Convert.ToBase64String(bitmapBytes);
            }

            return result;
        }
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {

                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static void ValidaSesionAcceso(string url)
        {
            if (!sisSesion.SesionEsValida())
            {
                sisSesion.RedirectToLoginPage();
                return;
            }
            else
            {
                if (!sisSesion.FormularioValido(url))
                {
                    sisSesion.RedirectToAccesoNoAutorizado();
                    return;
                }
            }
        }

        public static Nullable<System.DateTime> GetFechaServidor()
        {
            recursoRepository _recursoRepository = new recursoRepository();
            Parametro datRetorno = _recursoRepository.Select_fecha_servidor();
            if (datRetorno != null)
            {
                return datRetorno.FECHA_SERVIDOR;
            }
            else
            {
                return null;
            }

        }

        public static bool IsDate(String date)
        {
            //if(date.Length != 10)
            //{
            //    return false;
            //}
            try
            {
                DateTime dt = DateTime.Parse(date);
                if(dt.Year < 1950)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        public static void Obtiene_DiaIniFin_Semana(int anio, int semana,ref string fIni,ref string fFin){
            if (semana < 0 && semana > 53)
            {
                return; 
            }

            DateTime fechaInicial = new DateTime(anio, 1, 1);
            DateTime fechaFinal = new DateTime();

            if (fechaInicial.DayOfWeek == DayOfWeek.Monday)
            {
                //fechaInicial = fechaInicial;
            }
            else
            {
                fechaInicial = fechaInicial.AddDays (- ((fechaInicial.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)fechaInicial.DayOfWeek) -1) );
            }

            fechaFinal = fechaInicial.AddDays(6);

            if (semana == 2)
            {
                fechaInicial = fechaFinal.AddDays(1);
                fechaFinal = fechaInicial.AddDays(6);
            }
            else if (semana > 2)
            {
                fechaInicial = fechaFinal.AddDays(1 + ((semana - 2) * 7));
                fechaFinal = fechaInicial.AddDays(6);
            }

            fIni = fechaInicial.ToShortDateString();
            fFin = fechaFinal.ToShortDateString();
        }

        public static string Right(string texto, int tamanio, string relleno) {
            string relleno_full = string.Concat(Enumerable.Repeat(relleno, tamanio));
            string texto_full = relleno_full + texto;
            return texto_full.Substring(texto_full.Length - tamanio, tamanio);
        }

        public static string ToExcelIndexColum(long n)
        {
            // MINIMO -> 1 = A
            // MAXIMO -> 9223372036854775807 = CRPXNLSKVLJFHG
            string result = string.Empty;
            if (n == 0) return result;
            int res = Convert.ToInt32(n % 26);
            n = n / 26;
            n = res == 0 ? n - 1 : n;
            result = ToExcelIndexColum(n);
            res += res == 0 ? 90 : 64;
            result += Char.ConvertFromUtf32(res);
            return result;
        }


        public static string NombreMes(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-PE", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        public static bool Enviar_Correo_Html(string e_mail, string asunto, string contenido, string ruta_archivo_adjunto, List<string> lisCC = null)
        {
            var correo_envio = ConfigurationManager.AppSettings["Correo_Envio"];
            var contrasenia = ConfigurationManager.AppSettings["Contrasenia_Envio"];
            var correo = new System.Net.Mail.MailMessage();
            var ind = new bool();

            correo.From = new System.Net.Mail.MailAddress(correo_envio, "Sistema de Gestión de Contrataciones", System.Text.Encoding.UTF8);
            correo.To.Add(e_mail);
            if (lisCC != null)
            {
                foreach (string email in lisCC)
                {
                    correo.CC.Add(email);
                }
            }
            correo.Subject = asunto;
            correo.Body = contenido;
            correo.IsBodyHtml = true;
            correo.Priority = System.Net.Mail.MailPriority.Normal;
            if (ruta_archivo_adjunto != "")
            {
                Attachment data = new Attachment(ruta_archivo_adjunto, MediaTypeNames.Application.Octet);
                correo.Attachments.Add(data);
            }

            parametro_generalRepository _parametroDAO = new parametro_generalRepository();
            parametro_general datParametro = _parametroDAO.Select_parametro_general(new parametro_general { CODIGO = "SERVIDOR_SMTP" });
            string nombre_servidor_smtp = "smtp.gmail.com";
            if(datParametro != null)
            {
                nombre_servidor_smtp = datParametro.VALOR;
            }
            SmtpClient smtp = new SmtpClient(nombre_servidor_smtp);
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            datParametro = _parametroDAO.Select_parametro_general(new parametro_general { CODIGO = "USO_CLAVE_CORREO" });
            string usa_clave = "S";
            if(datParametro != null)
            {
                usa_clave = datParametro.VALOR;
            }
            if(usa_clave == "S")
            {
                smtp.Credentials = new System.Net.NetworkCredential(correo_envio, contrasenia);
            }            
            smtp.EnableSsl = true;
            ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12; //SecurityProtocolType.Tls;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 10000;


            try
            {
                smtp.Send(correo);
                ind = true;
            }
            catch (Exception ex)
            {
                ind = false;
            }

            return ind;
        }

        public static bool IsEmail(String email)
        {
            String expresion;
            expresion = "\\w+([-+.'[\\]]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void Write_(string mensaje)
        {
            try
            {
                string log = DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm:ss") + " " + mensaje;
                using (StreamWriter w = File.AppendText(Path.Combine(ConfigurationManager.AppSettings["RutaLog"], "Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt")))
                {
                    w.WriteLine(log);
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

    }
}
