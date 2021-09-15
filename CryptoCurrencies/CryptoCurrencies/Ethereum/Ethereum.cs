using System;
using System.IO;
using System.Windows.Forms;
using CryptoCurrencies.Helper;
using IronBarCode;

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
            BarcodeWriter.CreateBarcode(privateKey, BarcodeWriterEncoding.QRCode).SaveAsJpeg("privateKey.jpg");
            pictureBox2.ImageLocation = "privateKey.jpg";
        }

        private void GenerateQRPublicKeyEthereum(String ethereumAddress)
        {
            BarcodeWriter.CreateBarcode(ethereumAddress, BarcodeWriterEncoding.QRCode).SaveAsJpeg("ethereumAddress.jpg");
            pictureBox3.ImageLocation = "ethereumAddress.jpg";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }

        private void Ethereum_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete("privateKey.jpg");
            File.Delete("ethereumAddress.jpg");
        }
    }
}
