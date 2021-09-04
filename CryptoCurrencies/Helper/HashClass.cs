using System;
using System.Security.Cryptography;

namespace CryptoCurrencies.Helper
{
    class HashClass
    {
        private StringOperations operations;
        private SHA256 sha256;
        private RIPEMD160 ripemd160;

        public HashClass()
        {
            operations = new StringOperations();
            sha256 = SHA256.Create();
            ripemd160 = RIPEMD160.Create();
        }

        public String Hash256(String input)
        {
            byte[] hashValue1;
            byte[] hashValue2;

            // SHA256 Hash the whole input
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(input));

            // Second SHA256 hash on the result
            hashValue2 = sha256.ComputeHash(hashValue1);

            // Get only first 4 bytes (8 hexadecimals)
            return operations.AsciiToHex(hashValue2).Substring(0, 8);
        }

        public String Hash160(String input)
        {
            byte[] hashValue1;
            byte[] hashValue2;

            // SHA256 Hash the whole key block
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(input));

            // RIPEMD60 hash on the result
            hashValue2 = ripemd160.ComputeHash(hashValue1);

            // Assign hash result
            return operations.AsciiToHex(hashValue2);
        }
    }
}
