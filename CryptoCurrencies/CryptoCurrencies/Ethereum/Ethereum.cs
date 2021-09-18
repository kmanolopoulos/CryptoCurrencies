using System;
using System.Windows.Forms;
using CryptoCurrencies.HelperClasses;
using ZXing;

namespace CryptoCurrencies.Ethereum
{
    public partial class Ethereum : Form
    {
        public Ethereum()
        {
            InitializeComponent();
        }

        private void GenerateKeys()
        {
            DsaClass dsa = new DsaClass();

            String clearPrivateKey = dsa.GetPrivateKey();
            String clearPublicKey = dsa.GetPublicKey();
            String ethereumPrivateKey = GetEthereumPrivateKey(clearPrivateKey);
            String ethereumAddress = GetEthereumAddress(clearPublicKey);

            textBox1.Text = clearPrivateKey;
            textBox2.Text = ethereumPrivateKey;
            textBox3.Text = clearPublicKey;
            textBox4.Text = ethereumAddress;

            GenerateQRPrivateKey(ethereumPrivateKey);
            GenerateQRPublicKeyEthereum(ethereumAddress);
        }

        private String GetEthereumPrivateKey(String clearPrivateKey)
        {
            String ethereumPrivateKey;

            // Add header of ethereum private key
            ethereumPrivateKey = "0x" + clearPrivateKey;

            return ethereumPrivateKey;
        }

        private String GetEthereumAddress(String clearPublicKey)
        {
            HashClass hash = new HashClass();
            String address;
            String ethereumAddress;

            // Compute Keccak-256 hash
            address = hash.Keccak256(clearPublicKey);

            // Get 40 rightmost hex digits
            address = address.Substring(24);

            // Add header of ethereum address
            ethereumAddress = "0x" + address;

            return ethereumAddress;
        }

        private void GenerateQRPrivateKey(String privateKey)
        {
            pictureBox2.Image = new QRImage(pictureBox2.Height, pictureBox2.Width).GetBitmap(privateKey);
        }

        private void GenerateQRPublicKeyEthereum(String ethereumAddress)
        {
            pictureBox3.Image = new QRImage(pictureBox3.Height, pictureBox3.Width).GetBitmap(ethereumAddress);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }
    }
}
