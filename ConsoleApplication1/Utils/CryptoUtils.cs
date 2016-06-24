using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class CryptoUtils
    {
        private static readonly byte[] AESKEY = new byte[32]
         {
            13,
            27,
            119,
            151,
            224,
            216,
            8,
            95,
            14,
            184,
            217,
            168,
            137,
            34,
            32,
            122,
            21,
            241,
            158,
            194,
            137,
            153,
            96,
            9,
            24,
            26,
            47,
            118,
            231,
            236,
            59,
            209
         };
        private static readonly byte[] VECTOR = new byte[16]
        {
            81,
            101,
            222,
            3,
            25,
            8,
            213,
            21,
            34,
            55,
            89,
            144,
            233,
            32,
            114,
            56
        };

        public static string Decrypt(string encrypted)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(CryptoUtils.AESKEY,CryptoUtils.VECTOR);
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] encryptedString = Convert.FromBase64String(encrypted);

            MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(encryptedString, 0, encryptedString.Length);
            }

            return encoder.GetString(memoryStream.ToArray());
        }
    }
}
