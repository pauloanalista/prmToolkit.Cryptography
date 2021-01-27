using prmToolkit.Cryptography.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace prmToolkit.Cryptography
{
    public class Crypto : ICrypto
    {
        private const int IV_LENGTH = 16;
        private const int KEY_SIZE = 256;

        public string Encrypt(string contentToBeEncrypted, string privateEncryptionKey)
        {
            if (string.IsNullOrWhiteSpace(contentToBeEncrypted))
            {
                throw new ArgumentNullException(nameof(contentToBeEncrypted));
            }

            ValidatePrivateEncryptionKey(privateEncryptionKey);

            byte[] encryptedBytes;
            byte[] encryptedBytesAndIV;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KEY_SIZE;
                aes.Key = Encoding.UTF8.GetBytes(privateEncryptionKey);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(contentToBeEncrypted);
                        }

                        encryptedBytes = memoryStream.ToArray();
                    }
                }
                encryptedBytesAndIV = ConcatenateEncryptedBytesAndIV(encryptedBytes, aes.IV);
            }

            return Convert.ToBase64String(encryptedBytesAndIV);
        }
        
        public string Decrypt(string contentToBeDecrypted, string privateEncryptionKey)
        {
            if (string.IsNullOrWhiteSpace(contentToBeDecrypted))
            {
                throw new ArgumentNullException(nameof(contentToBeDecrypted));
            }

            ValidatePrivateEncryptionKey(privateEncryptionKey);

            byte[] bytesToBeDecrypted = Convert.FromBase64String(contentToBeDecrypted);

            (byte[] encryptedBytes, byte[] IV) = DissociateEncryptedBytesAndIV(bytesToBeDecrypted);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KEY_SIZE;
                aes.Key = Encoding.UTF8.GetBytes(privateEncryptionKey);
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private byte[] ConcatenateEncryptedBytesAndIV(byte[] encryptedBytes, byte[] IV)
        {
            var reversedIV = IV.Reverse();

            return encryptedBytes.Concat(reversedIV).ToArray();
        }

        private (byte[] encryptedBytes, byte[] IV) DissociateEncryptedBytesAndIV(byte[] bytesToBeDecrypted)
        {
            var totalLength = bytesToBeDecrypted.Length;

            var encryptedBytes = bytesToBeDecrypted.Take(totalLength - IV_LENGTH).ToArray();
            var IV = bytesToBeDecrypted.Skip(encryptedBytes.Length).Take(IV_LENGTH).Reverse().ToArray();

            return (encryptedBytes, IV);
        }

        private void ValidatePrivateEncryptionKey(string privateEncryptionKey)
        {
            if (string.IsNullOrWhiteSpace(privateEncryptionKey))
            {
                throw new ArgumentNullException(nameof(privateEncryptionKey));
            }

            if (privateEncryptionKey.Length != 32)
            {
                throw new ArgumentOutOfRangeException(nameof(privateEncryptionKey), "Private encryption key must be 256 bits long (32 characters)");
            }
        }
    }
}