using System.Security.Cryptography;
using System.Text;

namespace LunarDeckFoxyApi.Authentication
{
    public class Encryptor
    {
        /// <summary>
        /// Creates an MD5 Hex string from provided text
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Encrypted MD5 hexademical string</returns>
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            // compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after computing it
            byte[] result = md5.Hash;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                // change result into 2 hexadecimal digits
                // for each byte
                sb.Append(result[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
