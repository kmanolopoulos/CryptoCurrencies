using System;
using System.Numerics;
using System.Globalization;

namespace CryptoCurrencies.HelperClasses
{
    class DsaClass
    {
        private String PrivateKey;
        private String PublicKey;
        private String maxRandomNum;
        private String pNum, GxNum, GyNum;

        public DsaClass()
        {
            Initialisation();
            GeneratePrivateKey();
            ExtractPublicKey();
        }

        public DsaClass(String privateKey)
        {
            Initialisation();
            SetPrivateKey(privateKey);
            ExtractPublicKey();
        }

        public String GetMaxRandom()
        {
            return maxRandomNum;
        }

        private void Initialisation()
        {
            maxRandomNum = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141";
            pNum = "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F";
            GxNum = "79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798";
            GyNum = "483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8";
        }

        private void SetPrivateKey(String value)
        {
            PrivateKey = value;
        }

        private void GeneratePrivateKey()
        {
            BigInteger maxRandom = BigInteger.Parse("0" + maxRandomNum, NumberStyles.HexNumber);
            BigInteger privKey;
            byte[] bytes = maxRandom.ToByteArray();
            Random random = new Random();

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                privKey = new BigInteger(bytes);
            } while (privKey >= maxRandom);

            PrivateKey = privKey.ToString("X").PadLeft(65, '0').Substring(1);
        }

        private void ExtractPublicKey()
        {
            BigInteger p = BigInteger.Parse("0" + pNum, NumberStyles.HexNumber);
            BigInteger b = (BigInteger)7;
            BigInteger a = BigInteger.Zero;
            BigInteger Gx = BigInteger.Parse(GxNum, NumberStyles.HexNumber);
            BigInteger Gy = BigInteger.Parse(GyNum, NumberStyles.HexNumber);

            DsaCurveFp curve256 = new DsaCurveFp(p, a, b);
            DsaPoint generator256 = new DsaPoint(curve256, Gx, Gy);
            BigInteger secret = BigInteger.Parse("0" + PrivateKey, NumberStyles.HexNumber);
            DsaPoint pubkeyPoint = generator256 * secret;
            PublicKey = pubkeyPoint.X.ToString("X").PadLeft(65, '0').Substring(1) + pubkeyPoint.Y.ToString("X").PadLeft(65, '0').Substring(1);
        }

        public String GetPrivateKey()
        {
            return PrivateKey;
        }

        public String GetPublicKey()
        {
            return PublicKey;
        }
    }
}
