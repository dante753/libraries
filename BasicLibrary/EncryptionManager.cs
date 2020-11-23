using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace BasicLibrary
{
    public class EncryptionManager
    {
        public class AesEncription
        {
            public string DecryptedResult;
            public string EncryptedResult;

            private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            private int BlockSize = 128;

            public bool Encrypt(string TextToEncrypt)
            {
                if (!string.IsNullOrEmpty(TextToEncrypt))
                {
                    try
                    {
                        byte[] bytes = Encoding.Unicode.GetBytes(TextToEncrypt);
                        //Encrypt
                        SymmetricAlgorithm crypt = Aes.Create();
                        HashAlgorithm hash = MD5.Create();
                        crypt.BlockSize = BlockSize;
                        crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(TextToEncrypt));
                        crypt.IV = IV;

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream =
                               new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes, 0, bytes.Length);
                            }

                            EncryptedResult = Convert.ToBase64String(memoryStream.ToArray());
                        }

                        return true;
                    }
                    catch (Exception ex) { }
                }

                return false;
            }
            public bool Decrypt(string TextToDecrypt)
            {
                bool SecurityResult = false;
                if (TextToDecrypt == "") return SecurityResult;

                //Decrypt
                byte[] bytes = Convert.FromBase64String(TextToDecrypt);
                SymmetricAlgorithm crypt = Aes.Create();
                HashAlgorithm hash = MD5.Create();
                crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(TextToDecrypt));
                crypt.IV = IV;

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    using (CryptoStream cryptoStream =
                       new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] decryptedBytes = new byte[bytes.Length];
                        cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        DecryptedResult = Encoding.Unicode.GetString(decryptedBytes);
                        SecurityResult = true;
                    }
                }

                return SecurityResult;
            }


        }

        public class MD5Encription
        {
            public static string Get(string word)
            {
                MD5 md5 = MD5.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = md5.ComputeHash(encoding.GetBytes(word));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }

        public class Base64Encription
        {
            public static string Encode(string word)
            {
                byte[] byt = Encoding.UTF8.GetBytes(word);
                return Convert.ToBase64String(byt);
            }

            public static string Decode(string word)
            {
                byte[] b = Convert.FromBase64String(word);
                return Encoding.UTF8.GetString(b);
            }
        }

        public class SHA1Encription
        {
            public static string Get(string str)
            {
                SHA1 sha1 = SHA1.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha1.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }

        public class SHA256Encription
        {
            public static string Get(string str)
            {
                SHA256 sha256 = SHA256.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha256.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }

        public class SHA384Encription
        {
            public static string Get(string str)
            {
                SHA384 sha384 = SHA384.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha384.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }

        public class SAH256Encription
        {
            public static string Get(string str)
            {
                SHA512 sha512 = System.Security.Cryptography.SHA512.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] stream = null;
                StringBuilder sb = new StringBuilder();
                stream = sha512.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
                return sb.ToString();
            }
        }

    }
}
