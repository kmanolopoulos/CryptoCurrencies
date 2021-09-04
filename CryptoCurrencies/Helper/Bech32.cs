using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencies.Helper
{
    class Bech32
    {
        private String bech32Digits;

        public Bech32()
        {
            bech32Digits = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";
        }

        private Int16[] GetChecksum(Int16[] input)
        {
            Int16[] result;

            result = new Int16[6] { 0, 0, 0, 0, 0, 0 };

            return result;
        }

        public String Encode(String value)
        {
            Int16 witnessVersionByte = 0;
            String valueInBinary = "";
            Int16[] valueIn5bitArray;
            Int16[] checksum;
            String result;

            for (int i = 0; i < value.Length; i += 2)
            {
                valueInBinary += Convert.ToString(Convert.ToByte(value.Substring(i, 2), 16), 2).PadLeft(8, '0');
            }

            // Convert binary string into 5-bit numbers
            valueIn5bitArray = new Int16[valueInBinary.Length / 5];

            for (int i = 0; i < valueIn5bitArray.Length; i++)
            {
                valueIn5bitArray[i] = Convert.ToInt16(valueInBinary.Substring(5 * i, 5), 2);
            }

            // Compute 6 numbers checksum of 5-bit numbers
            checksum = GetChecksum(valueIn5bitArray);

            // Convert 5-bit numbers into Bech32 chars
            result = bech32Digits[witnessVersionByte].ToString();

            for (int i = 0; i < valueIn5bitArray.Length; i++)
            {
                result += bech32Digits[valueIn5bitArray[i]];
            }

            for (int i = 0; i < checksum.Length; i++)
            {
                result += bech32Digits[checksum[i]];
            }

            return result;
        }
    }
}
