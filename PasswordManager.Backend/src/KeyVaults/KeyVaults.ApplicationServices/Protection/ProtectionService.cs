using System.Security.Cryptography;

namespace PasswordManager.KeyVaults.ApplicationServices.Protection
{
    public class ProtectionService : IProtectionService
    {
        public string Protect(string item, string key)
        {
            using (var aes = Aes.Create())
            {
                byte[] keyBytes;

                // Converts from base64 string to byte array
                try
                {
                    keyBytes = Convert.FromBase64String(key);
                }
                catch (Exception ex)
                {
                    throw new ProtectionServiceException("ERROR: Could not convert key from Base64");
                }

                if (aes.ValidKeySize(keyBytes.Length * 8) == false)
                {
                    throw new ProtectionServiceException($"ERROR: Key size does not meet requirements! Key size: '{keyBytes.Length * 8} bit', Accepted key sizes: 128 bit, 192 bit, 256 bit");
                }

                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, null);

                byte[] encryptedBytes;

                // Encrypt the bytes
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(item);
                        }
                    }
                    encryptedBytes = ms.ToArray();
                }

                // Returns the encrypted bytes as a base64 string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public string Unprotect(string protectedItem, string key)
        {
            using (var aes = Aes.Create())
            {
                // Extract the protected item from the base64 string
                byte[] encryptedBytes;
                try
                {
                    encryptedBytes = Convert.FromBase64String(protectedItem);
                } catch (Exception ex)
                {
                    throw new ProtectionServiceException("ERROR: Could not convert protected item from Base64");
                }

                // Extract the key from the base64 string
                byte[] keyBytes;
                try
                {
                    keyBytes = Convert.FromBase64String(key);
                } catch (Exception ex)
                {
                    throw new ProtectionServiceException("ERROR: Could not convert key from Base64");
                }

                // Set the key
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, null);

                string decryptedItem;

                // Decrypt the bytes
                using (var ms = new MemoryStream(encryptedBytes))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            decryptedItem = sr.ReadToEnd();
                        }
                    }
                }

                // Returns the decrypted item
                return decryptedItem;
            }
        }
    }
}
