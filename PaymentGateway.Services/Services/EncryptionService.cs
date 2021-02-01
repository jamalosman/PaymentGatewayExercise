using PaymentGateway.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PaymentGateway.Services.Services
{
    public class EncryptionService : IEncryptionService
    {
        public EncryptionService()
        {
        }

        public string Decrypt(string encryptedValue, string keyPrefix, string key)
        {
            //if (keyPrefix.Length < 8 || key.Length < 24)
            //{
            //    throw new ArgumentException("keyPrefix must be alteast 8 characters and key must be atleast 24")
            //}

            var keyPrefixBytes = Encoding.UTF8.GetBytes(keyPrefix).Take(8);
            var keyBytes = Encoding.UTF8.GetBytes(key).Take(24);
            var fullKey = keyPrefixBytes.Concat(keyBytes).ToArray();
            var encryptedBytes = Convert.FromBase64String(encryptedValue);

            var tag = encryptedBytes.Take(16).ToArray();
            var nonce = encryptedBytes.Skip(16).Take(12).ToArray();
            var cipherBytes = encryptedBytes.Skip(28).ToArray();
            var plainBytes = new byte[cipherBytes.Length];

            using (var aesGcm = new AesGcm(fullKey))
            {
                aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
                //return Concat(tag, Concat(nonce, cipherBytes));
                return Encoding.UTF8.GetString(plainBytes);
            }
        }

        public string Encrypt(string plainValue, string keyPrefix, string key)
        {
            //if (keyPrefix.Length < 8 || key.Length < 24)
            //{
            //    throw new ArgumentException("keyPrefix must be alteast 8 characters and key must be atleast 24")
            //}

            var keyPrefixBytes = Encoding.UTF8.GetBytes(keyPrefix).Take(8);
            var keyBytes = Encoding.UTF8.GetBytes(key).Take(24);
            var fullKey = keyPrefixBytes.Concat(keyBytes).ToArray();
            var plainBytes = Encoding.UTF8.GetBytes(plainValue);
            
            var tag = new byte[16];
            var nonce = new byte[12];
            var cipherBytes = new byte[plainBytes.Length];

            var random = new Random();
            random.NextBytes(tag);
            random.NextBytes(nonce);

            using (var aesGcm = new AesGcm(fullKey))
            {
                aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);
                //return Concat(tag, Concat(nonce, cipherBytes));
                var encryptedBytes = tag.Concat(nonce).Concat(cipherBytes).ToArray();
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }
}
