using System;
using System.Numerics;

namespace CryptoCurrencies.Helper
{
    class Base58
    {
        private StringOperations operations;
        private String base58Digits;

        public Base58()
        {
            operations = new StringOperations();
            base58Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        }

        public String Encode(String value)
        {
            byte[] data;
            
            String result = "";

            // Convert String to byte array
            data = operations.HexToAscii(value);

            // Decode byte[] to BigInteger
            BigInteger intData = 0;

            for (int i = 0; i < data.Length; i++)
            {
                intData = intData * 256 + data[i];
            }

            // Encode BigInteger to Base58 string
            while (intData > 0)
            {
                int remainder = (int)(intData % 58);
                intData /= 58;
                result = base58Digits[remainder] + result;
            }

            // Append '1' for each leading 0 byte
            for (int i = 0; i < data.Length && data[i] == 0; i++)
            {
                result = '1' + result;
            }

            return result;
        }
    }
}
