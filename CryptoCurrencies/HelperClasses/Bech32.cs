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
        private uint PolyMod(Byte[] input)
        {
            uint startValue = 1;
            for (uint i = 0; i < input.Length; i++)
            {
                uint c0 = startValue >> 25;
                startValue = (uint)(((startValue & 0x1ffffff) << 5) ^
                    (input[i]) ^
                    (-((c0 >> 0) & 1) & 0x3b6a57b2) ^
                    (-((c0 >> 1) & 1) & 0x26508e6d) ^
                    (-((c0 >> 2) & 1) & 0x1ea119fa) ^
                    (-((c0 >> 3) & 1) & 0x3d4233dd) ^
                    (-((c0 >> 4) & 1) & 0x2a1462b3));
            }
            return startValue ^ 1;
        }

        private static void HrpExpand(String hrp, Byte[] hrpExpanded)
        {
            for (int i = 0; i < hrp.Length; i++)
            {
                hrpExpanded[i] = (byte)(hrp[i] >> 5);
            }
            hrpExpanded[hrp.Length] = 0;
            for (int i = 0; i < hrp.Length; i++)
            {
                hrpExpanded[hrp.Length + 1 + i] = (byte)(hrp[i] & 31);
            }
        }

        private Byte[] GetChecksum(String hrp, Byte witnessVersionByte, Byte[] input)
        {
            int hrpLen = hrp.Length * 2 + 1;
            Byte[] checksum = new Byte[6];
            Byte[] values = new Byte[hrpLen + 1 + input.Length + checksum.Length];

            HrpExpand(hrp, values);

            values[hrpLen] = witnessVersionByte;

            System.Buffer.BlockCopy(input, 0, values, hrpLen + 1, input.Length);

            uint mod = PolyMod(values);

            for (int i = 0; i < checksum.Length; i++)
            {
                checksum[i] = (byte)((mod >> (5 * (5 - i))) & 31);
            }
            return checksum;
        }

        public String Encode(String hrp, Byte witnessVersionByte, String value)
        {
            String valueInBinary = "";
            Byte[] valueIn5bitArray;
            Byte[] checksum;
            String result;

            // Convert value to binary string
            for (int i = 0; i < value.Length; i += 2)
            {
                valueInBinary += Convert.ToString(Convert.ToByte(value.Substring(i, 2), 16), 2).PadLeft(8, '0');
            }

            // Convert binary string into 5-bit numbers
            valueIn5bitArray = new Byte[valueInBinary.Length / 5];

            for (int i = 0; i < valueIn5bitArray.Length; i++)
            {
                valueIn5bitArray[i] = Convert.ToByte(valueInBinary.Substring(5 * i, 5), 2);
            }

            // Compute 6 numbers checksum of hrp + witnesVersionByte + 5-bit numbers
            checksum = GetChecksum(hrp, witnessVersionByte, valueIn5bitArray);

            // Convert hrp + witnesVersionByte + 5-bit numbers
            result = hrp + "1";
            result += bech32Digits[witnessVersionByte].ToString();

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
