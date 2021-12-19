using System;
using System.Numerics;

namespace CryptoCurrencies.HelperClasses
{
    class Base58
    {
        private StringOperations operations;
        private String defaultDictionary;

        public Base58()
        {
            operations = new StringOperations();
            defaultDictionary = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        }

        public String Encode(String value)
        {
            return Encode(value, defaultDictionary);
        }

        public String Encode(String value, String dictionary)
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
                result = dictionary[remainder] + result;
            }

            // Append first digit of dictionary for each leading 0 byte
            for (int i = 0; i < data.Length && data[i] == 0; i++)
            {
                result = dictionary[0] + result;
            }

            return result;
        }
    }
}
