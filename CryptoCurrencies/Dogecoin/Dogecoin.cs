using System;
using System.IO;
using System.Windows.Forms;
using CryptoCurrencies.Helper;
using IronBarCode;

namespace CryptoCurrencies.Dogecoin
{
    public partial class Dogecoin : Form
    {
        public Dogecoin()
        {
            InitializeComponent();
        }

        private void GenerateKeys()
        {
            DsaClass dsa = new DsaClass();

            String clearPrivateKey = dsa.GetPrivateKey();
            String clearPublicKey = dsa.GetPublicKey();
            String wifPrivateKey = GetWifPrivateKey(clearPrivateKey);
            String dogecoinAddress = GetDogecoinAddress(clearPublicKey);

            textBox1.Text = clearPrivateKey;
            textBox2.Text = wifPrivateKey;
            textBox3.Text = clearPublicKey;
            textBox4.Text = dogecoinAddress;

            GenerateQRPrivateKeyWif(wifPrivateKey);
            GenerateQRPublicKeyDogecoin(dogecoinAddress);
        }

        private String GetDogecoinAddress(String clearPublicKey)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String clearPublicKeyBlock;
            String checksum;
            String address;
            String bitcoinAddress;

            // Add header
            clearPublicKeyBlock = "04" + clearPublicKey;

            // Build P2PKH pattern
            address = "1E" + hash.Hash160(clearPublicKeyBlock);

            // Compute checksum
            checksum = hash.Hash256(address);

            // Build full P2PKH pattern
            address += checksum;

            // Convert public key block to Base 58 format
            bitcoinAddress = base58.Encode(address);

            return bitcoinAddress;
        }

        private String GetWifPrivateKey(String clearPrivateKey)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String clearPrivateKeyBlock;
            String wifPrivateKey;
            String checksum;

            // Add header and compress byte
            clearPrivateKeyBlock = "9E" + clearPrivateKey;

            // Compute checksum
            checksum = hash.Hash256(clearPrivateKeyBlock);

            // Add checksum to private key block
            clearPrivateKeyBlock += checksum;

            // Convert private key block to Base 58 format
            wifPrivateKey = base58.Encode(clearPrivateKeyBlock);

            return wifPrivateKey;
        }

        private void GenerateQRPrivateKeyWif(String wifPrivateKey)
        {
            BarcodeWriter.CreateBarcode(wifPrivateKey, BarcodeWriterEncoding.QRCode).SaveAsJpeg("wifPrivateKey.jpg");
            pictureBox1.ImageLocation = "wifPrivateKey.jpg";
        }

        private void GenerateQRPublicKeyDogecoin(String dogecoinAddress)
        {
            BarcodeWriter.CreateBarcode(dogecoinAddress, BarcodeWriterEncoding.QRCode).SaveAsJpeg("dogecoinAddress.jpg");
            pictureBox2.ImageLocation = "dogecoinAddress.jpg";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }

        private void Dogecoin_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete("wifPrivateKey.jpg");
            File.Delete("dogecoinAddress.jpg");
        }
    }
}
