using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Numerics;
using System.Globalization;
using System.Text;
using CryptoCurrencies.Helper;

namespace CryptoCurrencies.Bitcoin
{
    public partial class Bitcoin : Form
    {
        public Bitcoin()
        {
            InitializeComponent();
        }

        private void GenerateKeys()
        {
            String clearPrivateKey = GetClearPrivateKey();
            String clearPublicKey = GetClearPublicKey(clearPrivateKey);
            String uncompressedWifPrivateKey = GetWifPrivateKey(clearPrivateKey, false);
            String compressedWifPrivateKey = GetWifPrivateKey(clearPrivateKey, true);

            textBox1.Text = clearPrivateKey;
            textBox2.Text = uncompressedWifPrivateKey;
            textBox3.Text = compressedWifPrivateKey;
            textBox4.Text = clearPublicKey;
        }

        private String GetClearPrivateKey()
        {
            BigInteger maxRandom = BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141", NumberStyles.HexNumber);
            BigInteger privKey;
            byte[] bytes = maxRandom.ToByteArray();
            Random random = new Random();

            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                privKey = new BigInteger(bytes);
            } while (privKey >= maxRandom);

            return privKey.ToString("X").Substring(1).PadLeft(64, '0');
        }

        private String GetClearPublicKey(String clearPrivateKey)
        {
            BigInteger p = BigInteger.Parse("0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F", NumberStyles.HexNumber);
            BigInteger b = (BigInteger)7;
            BigInteger a = BigInteger.Zero;
            BigInteger Gx = BigInteger.Parse("79BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798", NumberStyles.HexNumber);
            BigInteger Gy = BigInteger.Parse("483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8", NumberStyles.HexNumber);

            CurveFp curve256 = new CurveFp(p, a, b);
            Point generator256 = new Point(curve256, Gx, Gy);
            BigInteger secret = BigInteger.Parse(clearPrivateKey, NumberStyles.HexNumber);
            Point pubkeyPoint = generator256 * secret;
            return pubkeyPoint.X.ToString("X") + pubkeyPoint.Y.ToString("X");
        }

        private String GetWifPrivateKey(String clearPrivateKey, Boolean compressed)
        {
            String clearPrivateKeyBlock;
            String wifPrivateKey;
            SHA256 sha256 = SHA256.Create();
            byte[] hashValue1;
            byte[] hashValue2;
            StringOperations operations = new StringOperations();

            // Add header and compress byte
            if (compressed)
            {
                clearPrivateKeyBlock = "80" + clearPrivateKey + "01";
            }
            else
            {
                clearPrivateKeyBlock = "80" + clearPrivateKey;
            }

            // Hash the whole key block
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(clearPrivateKeyBlock));

            // Second hash on the result
            hashValue2 = sha256.ComputeHash(hashValue1);

            // Add hash first bytes to private key block
            clearPrivateKeyBlock += operations.AsciiToHex(hashValue2).Substring(0, 8);

            // Convert private key block to Base 58 format
            wifPrivateKey = operations.Base58Encode(clearPrivateKeyBlock);

            return wifPrivateKey;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }
    }
}
