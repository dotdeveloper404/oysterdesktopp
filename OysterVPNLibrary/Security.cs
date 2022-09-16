namespace OysterVPNLibrary
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal class Security
    {
        private static string key = "v8y/A?D(G+KbPeSh";

        public static string Decrypt(string input)
        {
            try
            {
                byte[] inputBuffer = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                byte[] bytes = provider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                provider.Clear();
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }

        public static string Encrypt(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            byte[] inArray = provider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            provider.Clear();
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }
    }
}

