using System;

namespace CryptoCurrencies.HelperClasses
{
    class SeedGenerator
    {
        public SeedGenerator()
        {
        }

        public String GetSeed(int hexChars)
        {
            StringOperations operations = new StringOperations();
            byte[] bytes = new byte[hexChars / 2];
            new Random().NextBytes(bytes);
            return operations.AsciiToHex(bytes);
        }

        public String GetRootPrivateKey(String seed)
        {
            HashClass hash = new HashClass();
            DsaClass dsa = new DsaClass();
            String input, output;
            int keySequence = 0;

            do
            {
                input = seed + keySequence.ToString("").PadLeft(8, '0');
                output = hash.SHA512(input).Substring(0, 64);
                keySequence++;
            } while (String.Compare(output, dsa.GetMaxRandom()) > 0);

            return output;
        }
    }
}
