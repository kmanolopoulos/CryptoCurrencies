using System;
using System.IO;
using System.Security.Cryptography;
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

            // SHA256 Hash the whole key block
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(clearPrivateKeyBlock));

            // Second SHA256 hash on the result
            hashValue2 = sha256.ComputeHash(hashValue1);

            // Add hash first bytes to private key block
            clearPrivateKeyBlock += operations.AsciiToHex(hashValue2).Substring(0, 8);

            // Convert private key block to Base 58 format
            wifPrivateKey = operations.Base58Encode(clearPrivateKeyBlock);

            return wifPrivateKey;
        }

        private String GetBtcAddress(String clearPublicKey, Boolean compressed)
        {
            String clearPublicKeyBlock;
            String clearPublicKeyHash;
            String address;
            String bitcoinAddress;
            SHA256 sha256 = SHA256.Create();
            RIPEMD160 ripemd160 = RIPEMD160.Create();
            byte[] hashValue1;
            byte[] hashValue2;
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

            // SHA256 Hash the whole key block
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(clearPublicKeyBlock));

            // RIPEMD60 hash on the result
            hashValue2 = ripemd160.ComputeHash(hashValue1);

            // Assign hash result
            clearPublicKeyHash = operations.AsciiToHex(hashValue2);

            // Build P2PKH pattern
            address = "00" + clearPublicKeyHash;

            // SHA256 Hash P2PKH pattern
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(address));

            // SHA256 Hash previous result
            hashValue2 = sha256.ComputeHash(hashValue1);

            // Build full P2PKH pattern
            address += operations.AsciiToHex(hashValue2).Substring(0, 8);

            // Convert public key block to Base 58 format
            bitcoinAddress = operations.Base58Encode(address);

            return bitcoinAddress;
        }

        private String GetBtcAddressSegWit(String clearPublicKey, Boolean compressed)
        {
            String clearPublicKeyBlock;
            String clearPublicKeyHash;
            String clearPublicKeyHashBinary;
            String address;
            String bitcoinAddressSegWit;
            SHA256 sha256 = SHA256.Create();
            RIPEMD160 ripemd160 = RIPEMD160.Create();
            byte[] hashValue1;
            byte[] hashValue2;
            Int16 witnessVersionByte = 0;
            Int16[] base32Array;
            Int16[] base32Checksum;
            String bech32Chars = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";
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

            // SHA256 Hash the whole key block
            hashValue1 = sha256.ComputeHash(operations.HexToAscii(clearPublicKeyBlock));

            // RIPEMD60 hash on the result
            hashValue2 = ripemd160.ComputeHash(hashValue1);

            // Assign hash result
            clearPublicKeyHash = operations.AsciiToHex(hashValue2);

            // Convert hash result into Binary string
            clearPublicKeyHashBinary = "";

            for (int i = 0; i < clearPublicKeyHash.Length; i += 2)
            {
                clearPublicKeyHashBinary += Convert.ToString(Convert.ToByte(clearPublicKeyHash.Substring(i, 2), 16), 2).PadLeft(8, '0');
            }

            // Convert binary string into 5-bit numbers
            base32Array = new Int16[clearPublicKeyHashBinary.Length / 5];

            for (int i = 0; i < base32Array.Length; i++)
            {
                base32Array[i] = Convert.ToInt16(clearPublicKeyHashBinary.Substring(5 * i, 5), 2);
            }

            // Compute 6 numbers checksum of 5-bit numbers
            base32Checksum = new Int16[6] { 0, 0, 0, 0, 0, 0 };

            // Convert 5-bit numbers into Bech32 chars
            address = bech32Chars[witnessVersionByte].ToString();

            for (int i = 0; i < base32Array.Length; i++)
            {
                address += bech32Chars[base32Array[i]];
            }

            for (int i = 0; i < base32Checksum.Length; i++)
            {
                address += bech32Chars[base32Checksum[i]];
            }

            // Add SegWit header to existing address
            bitcoinAddressSegWit = "bc1" + address;

            return bitcoinAddressSegWit;
        }

        private void GenerateQRPrivateKeyWifUncompressed(String uncompressedWifPrivateKey)
        {
            BarcodeWriter.CreateBarcode(uncompressedWifPrivateKey, BarcodeWriterEncoding.QRCode).SaveAsJpeg("uncompressedWifPrivateKey.jpg");
            pictureBox1.ImageLocation = "uncompressedWifPrivateKey.jpg";
        }

        private void GenerateQRPrivateKeyWifCompressed(String compressedWifPrivateKey)
        {
            BarcodeWriter.CreateBarcode(compressedWifPrivateKey, BarcodeWriterEncoding.QRCode).SaveAsJpeg("compressedWifPrivateKey.jpg");
            pictureBox2.ImageLocation = "compressedWifPrivateKey.jpg";
        }

        private void GenerateQRPublicKeyBTC(String btcAddress)
        {
            BarcodeWriter.CreateBarcode(btcAddress, BarcodeWriterEncoding.QRCode).SaveAsJpeg("btcAddress.jpg");
            pictureBox3.ImageLocation = "btcAddress.jpg";
        }

        private void GenerateQRPublicKeyBTCSegWit(String btcAddressSegWit)
        {
            BarcodeWriter.CreateBarcode(btcAddressSegWit, BarcodeWriterEncoding.QRCode).SaveAsJpeg("btcAddressSegWit.jpg");
            pictureBox4.ImageLocation = "btcAddressSegWit.jpg";
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
