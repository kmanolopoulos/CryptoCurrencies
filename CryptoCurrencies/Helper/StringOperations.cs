using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace CryptoCurrencies.Helper
{
    class StringOperations
    {
        public byte[] HexToAscii(String str)
        {
            char[] chars = str.ToCharArray();
            int len = chars.Length / 2;
            byte[] bytes = new byte[len];

            for (int i = 0; i < len; i++)
            {
                byte highNibble = byte.Parse(chars[2 * i].ToString(), NumberStyles.HexNumber);
                byte lowNibble = byte.Parse(chars[2 * i + 1].ToString(), NumberStyles.HexNumber);
                bytes[i] = (byte)((byte)(highNibble << 4) | lowNibble);
            }

            return bytes;
        }

        public String AsciiToHex(byte[] bytes)
        {
            StringBuilder str = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                byte bcdByte = bytes[i];
                byte idHigh = (byte)(bcdByte >> 4);
                byte idLow = (byte)(bcdByte & 0x0F);
                str.Append(String.Format("{0:X}{1:X}", idHigh, idLow));
            }

            return str.ToString();
        }

        public String Base58Encode(String value)
        {
            byte[] data;
            String Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            String result = "";

            // Convert String to byte array
            data = HexToAscii(value);

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
                result = Digits[remainder] + result;
            }

            // Append `1` for each leading 0 byte
            for (int i = 0; i < data.Length && data[i] == 0; i++)
            {
                result = '1' + result;
            }

            return result;
        }
    }
}
