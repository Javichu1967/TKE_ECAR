using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TK_ECAR.Framework.Utils
{
    public class Encriptar
    {
        //string pass = (DateTime.Today.Month + DateTime.Today.Day + "WEB0103" + DateTime.Today.Year).PadLeft(16,'0');
        byte[] Clave = Encoding.UTF8.GetBytes(($"{DateTime.Today.Month + DateTime.Today.Day}+C1fer#{DateTime.Today.Year + (DateTime.Today.Day * 2)}").PadLeft(16, '0'));
        byte[] IV = Encoding.UTF8.GetBytes("Devjoker7.37hAES");

        public string Encripta(string Cadena)
        {

            Cadena = HttpUtility.HtmlEncode(Cadena);
            byte[] inputBytes = Encoding.UTF8.GetBytes(Cadena);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(Clave, IV), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }
            return Convert.ToBase64String(encripted);
        }


        public string Desencripta(string Cadena)
        {
            byte[] inputBytes = Convert.FromBase64String(Cadena);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            textoLimpio = HttpUtility.HtmlDecode(textoLimpio);
            return textoLimpio;
        }
    }
}