using System;
using System.IO;
using System.Security.Cryptography;

namespace avj.Common
{
    /// <summary>
    /// SmartCloudPortal 암/복호화
    /// </summary>
    public class CryptHelper
    {
        static private byte[] key = { 32, 57, 111, 174, 201, 151, 44, 79, 81, 13, 249, 253, 1, 31, 119, 67, 77, 64, 53, 84, 18, 7, 4, 101 };

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="str"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        static public string EncryptData(string str, byte[] iv)
        {
            string encryptStr = string.Empty;
            byte[] bytIn = null;
            byte[] bytOut = null;
            MemoryStream ms = null;
            TripleDESCryptoServiceProvider tcs = null;
            ICryptoTransform ct = null;
            CryptoStream cs = null;

            try
            {
                bytIn = System.Text.Encoding.UTF8.GetBytes(str);
                ms = new MemoryStream();
                tcs = new TripleDESCryptoServiceProvider();
                ct = tcs.CreateEncryptor(key, iv);
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);                
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();

                bytOut = ms.ToArray();

                encryptStr = System.Convert.ToBase64String(bytOut, 0, bytOut.Length);
            }
            catch (Exception)
            {

            }
            finally
            {
                if (cs != null) { cs.Clear(); cs = null; }
                if (ct != null) { ct.Dispose(); ct = null; }
                if (tcs != null) { tcs.Clear(); tcs = null; }
                if (ms != null) { ms = null; }
            }

            return encryptStr;
        }

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="str"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        static public string DecryptData(string str, byte[] iv)
        {
            string decryptStr = string.Empty;
            byte[] bytIn = null;
            MemoryStream ms = null;
            TripleDESCryptoServiceProvider tcs = null;
            CryptoStream cs = null;
            ICryptoTransform ct = null;
            StreamReader sr = null;

            try
            {
                bytIn = System.Convert.FromBase64String(str);
                ms = new MemoryStream(bytIn, 0, bytIn.Length);
                tcs = new TripleDESCryptoServiceProvider();
                ct = tcs.CreateDecryptor(key, iv);
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
                sr = new StreamReader(cs);

                decryptStr = sr.ReadToEnd();
            }
            catch (Exception)
            {
            }

            finally
            {
                if (sr != null) { sr.Close(); sr = null; }
                if (cs != null) { cs.Clear(); cs = null; }
                if (ct != null) { ct.Dispose(); ct = null; }
                if (tcs != null) { tcs.Clear(); tcs = null; }
                if (ms != null) { ms.Close(); ms = null; }
            }

            return decryptStr;
        }
    }
}
