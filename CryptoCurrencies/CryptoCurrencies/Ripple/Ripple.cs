using System;
using System.Windows.Forms;
using System.Globalization;
using CryptoCurrencies.HelperClasses;

namespace CryptoCurrencies.Ripple
{
    public partial class Ripple : Form
    {
        private String base58Dictionary;

        public Ripple()
        {
            InitializeVariables();
            InitializeComponent();
        }

        private void InitializeVariables()
        {
            base58Dictionary = "rpshnaf39wBUDNEGHJKLM4PQRST7VWXYZ2bcdeCg65jkm8oFqi1tuvAxyz";
        }

        private void GenerateKeys()
        {
            String seedValue = GetSeed();
            String rootPrivateKey = GetRootPrivateKey(seedValue);
            String rootPublicKey = GetPublicKey(rootPrivateKey);
            String intermediatePrivateKey = GetIntermediatePrivateKey(rootPublicKey);
            String intermediatePublicKey = GetPublicKey(intermediatePrivateKey);
            String masterPrivateKey = GetMasterPrivateKey(rootPrivateKey, intermediatePrivateKey);
            String masterPublicKey = GetPublicKey(masterPrivateKey);
            String seedValueAddress = GetSeedValueAddress(seedValue);
            String accountAddress = GetAccountAddress(masterPublicKey);

            textBox1.Text = seedValue;
            textBox2.Text = seedValueAddress;
            textBox3.Text = masterPublicKey;
            textBox4.Text = accountAddress;

            GenerateQRPrivateKeyRipple(accountAddress);
            GenerateQRPublicKeyRipple(masterPrivateKey);
        }

        private String GetSeed()
        {
            SeedGenerator sg = new SeedGenerator();
            return sg.GetSeed(32);
        }

        private String GetPublicKey(String privateKey)
        {
            DsaClass dsa = new DsaClass(privateKey);
            String uncompressedPublicKey = dsa.GetPublicKey();
            String compressedPublicKey;

            if ((Int16.Parse(uncompressedPublicKey.Substring(127, 1), NumberStyles.HexNumber) % 2) == 1)
            {
                compressedPublicKey = "03" + uncompressedPublicKey.Substring(0, 64);
            }
            else
            {
                compressedPublicKey = "02" + uncompressedPublicKey.Substring(0, 64);
            }

            return compressedPublicKey;
        }

        private String GetRootPrivateKey(String seedValue)
        {
            SeedGenerator sg = new SeedGenerator();
            return sg.GetRootPrivateKey(seedValue);
        }

        private String GetIntermediatePrivateKey(String rootPublicKey)
        {
            HashClass hash = new HashClass();
            DsaClass dsa = new DsaClass();
            String input, output;
            int familyNumber = 0;
            int keySequence = 0;

            do
            {
                input = rootPublicKey + familyNumber.ToString("").PadLeft(8, '0') + keySequence.ToString("").PadLeft(8, '0');
                output = hash.SHA512(input).Substring(0, 64);
                keySequence++;
            } while (String.Compare(output, dsa.GetMaxRandom()) > 0);

            return output;
        }

        private String GetMasterPrivateKey(String rootPrivateKey, String intermediatePrivateKey)
        {
            BigIntegerOperations operations = new BigIntegerOperations();
            DsaClass dsa = new DsaClass();
            String addedValue = operations.Add(rootPrivateKey, intermediatePrivateKey, 64);
            String finalValue = operations.Modulo(addedValue, dsa.GetMaxRandom(), 64);
            return finalValue;
        }

        private String GetSeedValueAddress(String seedValue)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String input = "21" + seedValue;
            String checksum = hash.Hash256(input);
            String address = input + checksum;
            return base58.Encode(address, base58Dictionary);
        }

        private String GetAccountAddress(String masterPublicKey)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String address = "00" + hash.Hash160(masterPublicKey);
            String checksum = hash.Hash256(address);
            address = address + checksum;
            return base58.Encode(address, base58Dictionary);
        }

        private void GenerateQRPrivateKeyRipple(String ripplePrivateKey)
        {
            pictureBox2.Image = new QRImage(pictureBox2.Height, pictureBox2.Width).GetBitmap(ripplePrivateKey);
        }

        private void GenerateQRPublicKeyRipple(String rippleAddress)
        {
            pictureBox3.Image = new QRImage(pictureBox3.Height, pictureBox3.Width).GetBitmap(rippleAddress);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }
    }
}
