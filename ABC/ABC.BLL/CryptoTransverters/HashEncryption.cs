using ABC.BLL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.CryptoTransverters
{
    public class HashEncryption : IEncrypt
    {
        public static HashEncryption Default { get; set; }
        static HashEncryption()
        {
            Default = new HashEncryption();
        }

        protected HashAlgorithm HA { get; set; }
        public HashEncryption()
        {
            HA = SHA256.Create();
        }

        public byte[] Encrypt(byte[] inputBuffer)
        {
            return HA.ComputeHash(inputBuffer);
        }

        public string Encrypt(string inString)
        {
            return Encrypt(inString.FromUTF8String()).ToBase64String();
        }
    }
}
