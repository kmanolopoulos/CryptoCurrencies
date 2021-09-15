using System;
using System.Globalization;
using System.Text;

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
    }
}
