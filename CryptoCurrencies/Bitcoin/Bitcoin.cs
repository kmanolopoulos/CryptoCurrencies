using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using CryptoCurrencies.Helper;
using IronBarCode;

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
            DsaClass dsa = new DsaClass();

            String clearPrivateKey = dsa.GetPrivateKey();
            String clearPublicKey = dsa.GetPublicKey();
            String uncompressedWifPrivateKey = GetWifPrivateKey(clearPrivateKey, false);
            String compressedWifPrivateKey = GetWifPrivateKey(clearPrivateKey, true);
            String btcAddress = GetBtcAddress(clearPublicKey, true);
            String btcAddressSegWit = GetBtcAddressSegWit(clearPublicKey, true);

            textBox1.Text = clearPrivateKey;
            textBox2.Text = uncompressedWifPrivateKey;
            textBox3.Text = compressedWifPrivateKey;
            textBox4.Text = clearPublicKey;
            textBox5.Text = btcAddress;
            textBox6.Text = btcAddressSegWit;

            GenerateQRPrivateKeyWifUncompressed(uncompressedWifPrivateKey);
            GenerateQRPrivateKeyWifCompressed(compressedWifPrivateKey);
            GenerateQRPublicKeyBTC(btcAddress);
            GenerateQRPublicKeyBTCSegWit(btcAddressSegWit);
        }

        private String GetWifPrivateKey(String clearPrivateKey, Boolean compressed)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String clearPrivateKeyBlock;
            String wifPrivateKey;
            String checksum;

            // Add header and compress byte
            if (compressed)
            {
                clearPrivateKeyBlock = "80" + clearPrivateKey + "01";
            }
            else
            {
                clearPrivateKeyBlock = "80" + clearPrivateKey;
            }

            // Compute checksum
            checksum = hash.Hash256(clearPrivateKeyBlock);

            // Add checksum to private key block
            clearPrivateKeyBlock += checksum;

            // Convert private key block to Base 58 format
            wifPrivateKey = base58.Encode(clearPrivateKeyBlock);

            return wifPrivateKey;
        }

        private String GetBtcAddress(String clearPublicKey, Boolean compressed)
        {
            HashClass hash = new HashClass();
            Base58 base58 = new Base58();
            String clearPublicKeyBlock;
            String checksum;
            String address;
            String bitcoinAddress;

            // Add header
            if (compressed)
            {
                if ((Int16.Parse(clearPublicKey.Substring(127, 1), NumberStyles.HexNumber) % 2) == 1)
                {
                    clearPublicKeyBlock = "03" + clearPublicKey.Substring(0, 64);
                }
                else
                {
                    clearPublicKeyBlock = "02" + clearPublicKey.Substring(0, 64);
                }
            }
            else
            {
                clearPublicKeyBlock = "04" + clearPublicKey;
            }

            // Build P2PKH pattern
            address = "00" + hash.Hash160(clearPublicKeyBlock);

            // Compute checksum
            checksum = hash.Hash256(address);

            // Build full P2PKH pattern
            address += checksum;

            // Convert public key block to Base 58 format
            bitcoinAddress = base58.Encode(address);

            return bitcoinAddress;
        }

        private String GetBtcAddressSegWit(String clearPublicKey, Boolean compressed)
        {
            HashClass hash = new HashClass();
            Bech32 bech32 = new Bech32();
            String clearPublicKeyBlock;
            String clearPublicKeyHash;
            String bitcoinAddressSegWit;
            
            StringOperations operations = new StringOperations();

            // Add header
            if (compressed)
            {
                if ((Int16.Parse(clearPublicKey.Substring(127, 1), NumberStyles.HexNumber) % 2) == 1)
                {
                    clearPublicKeyBlock = "03" + clearPublicKey.Substring(0, 64);
                }
                else
                {
                    clearPublicKeyBlock = "02" + clearPublicKey.Substring(0, 64);
                }
            }
            else
            {
                clearPublicKeyBlock = "04" + clearPublicKey;
            }

            // Assign hash result
            clearPublicKeyHash = hash.Hash160(clearPublicKeyBlock);

            // Encode with bech 32 the hash
            bitcoinAddressSegWit = bech32.Encode("bc", 0, clearPublicKeyHash);

            return bitcoinAddressSegWit;
        }

        private void GenerateQRPrivateKeyWifUncompressed(String uncompressedWifPrivateKey)
        {
            BarcodeWriter.CreateBarcode(uncompressedWifPrivateKey, BarcodeWriterEncoding.QRCode).SaveAsJpeg("uncompressedWifPrivateKey.jpg");
            pictureBox2.ImageLocation = "uncompressedWifPrivateKey.jpg";
        }

        private void GenerateQRPrivateKeyWifCompressed(String compressedWifPrivateKey)
        {
            BarcodeWriter.CreateBarcode(compressedWifPrivateKey, BarcodeWriterEncoding.QRCode).SaveAsJpeg("compressedWifPrivateKey.jpg");
            pictureBox3.ImageLocation = "compressedWifPrivateKey.jpg";
        }

        private void GenerateQRPublicKeyBTC(String btcAddress)
        {
            BarcodeWriter.CreateBarcode(btcAddress, BarcodeWriterEncoding.QRCode).SaveAsJpeg("btcAddress.jpg");
            pictureBox4.ImageLocation = "btcAddress.jpg";
        }

        private void GenerateQRPublicKeyBTCSegWit(String btcAddressSegWit)
        {
            BarcodeWriter.CreateBarcode(btcAddressSegWit, BarcodeWriterEncoding.QRCode).SaveAsJpeg("btcAddressSegWit.jpg");
            pictureBox5.ImageLocation = "btcAddressSegWit.jpg";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateKeys();
        }

        private void Bitcoin_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.Delete("uncompressedWifPrivateKey.jpg");
            File.Delete("compressedWifPrivateKey.jpg");
            File.Delete("btcAddress.jpg");
            File.Delete("btcAddressSegWit.jpg");
        }
    }
}
