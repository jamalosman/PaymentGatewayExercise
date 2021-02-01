using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainValue, string keyPrefix, string key);
        string Decrypt(string encryptedValue, string keyPrefix, string key);
    }
}
