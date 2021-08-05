using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication1
{
    class CryptoUtils
    {
        private static readonly Byte[] AESKEY = new Byte[32]
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
        private static readonly Byte[] VECTOR = new Byte[16]
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

        public static String Decrypt(String encrypted)
        {
            var rijndaelManaged = new RijndaelManaged();
            var decryptor = rijndaelManaged.CreateDecryptor(CryptoUtils.AESKEY, CryptoUtils.VECTOR);
            var encoder = new UTF8Encoding();
            var encryptedString = Convert.FromBase64String(encrypted);

            var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(encryptedString, 0, encryptedString.Length);
            }

            return encoder.GetString(memoryStream.ToArray());
        }
    }
}
