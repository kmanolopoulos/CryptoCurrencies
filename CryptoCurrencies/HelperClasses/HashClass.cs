using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;

namespace CryptoCurrencies.HelperClasses
{
    class HashClass
    {
        private StringOperations operations;
        private SHA256 sha256;
        private SHA512 sha512;
        private RIPEMD160 ripemd160;
        private KeccakDigest keccak256;

        public HashClass()
        {
            operations = new StringOperations();
            sha256 = SHA256.Create();
            sha512 = new SHA512Managed();
            ripemd160 = RIPEMD160.Create();
            keccak256 = new KeccakDigest(256);
        }

        public String SHA512(String input)
        {
            byte[] hashValue = sha512.ComputeHash(operations.HexToAscii(input));
            return operations.AsciiToHex(hashValue);
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

        public String Keccak256(String input)
        {
            Byte[] inputValue = operations.HexToAscii(input);
            Byte[] hashValue = new byte[32];

            // KECCAK256 Hash the whole key block
            keccak256.BlockUpdate(inputValue, 0, inputValue.Length);
            keccak256.DoFinal(hashValue, 0);

            // Assign hash result
            return operations.AsciiToHex(hashValue);
        }
    }
}
