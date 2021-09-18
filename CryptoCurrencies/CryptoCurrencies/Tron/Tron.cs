using System;
using System.Windows.Forms;
using CryptoCurrencies.HelperClasses;

namespace CryptoCurrencies.Tron
{
    public partial class Tron : Form
    {
        public Tron()
        {
            InitializeComponent();
        }

        private void GenerateKeys()
        {
            DsaClass dsa = new DsaClass();

            String clearPrivateKey = dsa.GetPrivateKey();
            String clearPublicKey = dsa.GetPublicKey();
            String tronPrivateKey = GetTronPrivateKey(clearPrivateKey);
            String tronAddress = GetTronAddress(clearPublicKey);

            textBox1.Text = clearPrivateKey;
            textBox2.Text = tronPrivateKey;
            textBox3.Text = clearPublicKey;
            textBox4.Text = tronAddress;

            GenerateQRPrivateKey(tronPrivateKey);
            GenerateQRPublicKeyEthereum(tronAddress);
        }

        private String GetTronPrivateKey(String clearPrivateKey)
        {
            String tronPrivateKey;

            tronPrivateKey = clearPrivateKey;

            return tronPrivateKey;
        }

        private String GetTronAddress(String clearPublicKey)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String checksum;
            String address;
            String tronAddress;

            // Compute Keccak-256 hash
            address = hash.Keccak256(clearPublicKey);

            // Get 40 rightmost hex digits and add header byte
            address = "41" + address.Substring(24);

            // Compute checksum
            checksum = hash.Hash256(address);

            // Form tron address
            address += checksum;

            // Convert tron address to Base 58 format
            tronAddress = base58.Encode(address);

            return tronAddress;
        }

        private void GenerateQRPrivateKey(String privateKey)
        {
            pictureBox2.Image = new QRImage(pictureBox2.Height, pictureBox2.Width).GetBitmap(privateKey);
        }

        private void GenerateQRPublicKeyEthereum(String tronAddress)
        {
            pictureBox3.Image = new QRImage(pictureBox3.Height, pictureBox3.Width).GetBitmap(tronAddress);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }
    }
}
