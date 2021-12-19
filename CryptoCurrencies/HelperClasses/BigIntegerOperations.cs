using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencies.HelperClasses
{
    class BigIntegerOperations
    {
        public String Add(String operandA, String operandB, int order)
        {
            StringOperations operations = new StringOperations();
            byte[] byteArrayA = operations.HexToAscii(operandA.PadLeft(order + 2, '0')).Reverse().ToArray();
            byte[] byteArrayB = operations.HexToAscii(operandB.PadLeft(order + 2, '0')).Reverse().ToArray();
            BigInteger a = new BigInteger(byteArrayA);
            BigInteger b = new BigInteger(byteArrayB);
            BigInteger result = a + b;

            byte[] byteArrayResult = result.ToByteArray().Reverse().ToArray();

            return operations.AsciiToHex(byteArrayResult).PadLeft(order + 2, '0');
        }

        public String Modulo(String operandA, String operandB, int order)
        {
            StringOperations operations = new StringOperations();
            byte[] byteArrayA = operations.HexToAscii(operandA.PadLeft(order + 2, '0')).Reverse().ToArray();
            byte[] byteArrayB = operations.HexToAscii(operandB.PadLeft(order + 2, '0')).Reverse().ToArray();
            BigInteger a = new BigInteger(byteArrayA);
            BigInteger b = new BigInteger(byteArrayB);
            BigInteger result = a % b;

            byte[] byteArrayResult = result.ToByteArray().Reverse().ToArray();

            return operations.AsciiToHex(byteArrayResult).PadLeft(order + 2, '0').Substring(2);
        }
    }
}
