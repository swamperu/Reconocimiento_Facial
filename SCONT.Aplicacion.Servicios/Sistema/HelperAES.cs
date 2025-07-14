using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SCONT.Aplicacion.Servicios
{
    public class HelperAES
    {
        public HelperAES()
        {
        }

        public static string DecryptStringAES(string cipherText, string key, bool decode = false)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] numArray = Encoding.UTF8.GetBytes(key);
            if (decode)
            {
                cipherText = WebUtility.UrlDecode(cipherText);
            }
            return HelperAES.DecryptStringFromBytes(Convert.FromBase64String(cipherText), bytes, numArray);
        }

        public static string DecryptStringAES(string cipherText)
        {
            return HelperAES.DecryptStringAES(cipherText, "8080808080808O80", false);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length == 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length == 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length == 0)
            {
                throw new ArgumentNullException("key");
            }
            string end = null;
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;
                rijndaelManaged.Key = key;
                rijndaelManaged.IV = iv;
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                end = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    end = "";
                }
            }
            return end;
        }

        public static string EncryptStringAES(string plainText, string key, bool encode = true)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] numArray = Encoding.UTF8.GetBytes(key);
            string base64String = Convert.ToBase64String(HelperAES.EncryptStringToBytes(plainText, bytes, numArray));
            if (encode)
            {
                base64String = WebUtility.UrlEncode(base64String);
            }
            return base64String;
        }

        public static string EncryptStringAES(string plainText)
        {
            return HelperAES.EncryptStringAES(plainText, "8080808080808O80", true);
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            byte[] array;
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length == 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length == 0)
            {
                throw new ArgumentNullException("key");
            }
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                rijndaelManaged.FeedbackSize = 128;
                rijndaelManaged.Key = key;
                rijndaelManaged.IV = iv;
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return array;
        }
    }
}