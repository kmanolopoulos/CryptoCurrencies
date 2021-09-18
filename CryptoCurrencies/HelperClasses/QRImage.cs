using System;
using System.Drawing;
using ZXing;

namespace CryptoCurrencies.HelperClasses
{
    class QRImage
    {
        BarcodeWriter writer;

        public QRImage(int height, int width)
        {
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options.Height = height;
            writer.Options.Width = width;
        }

        public Image GetBitmap(String value)
        {
            return writer.Write(writer.Encode(value));
        }
    }
}
