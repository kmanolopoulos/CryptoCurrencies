using System;
using System.Windows.Forms;

namespace CryptoCurrencies
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new CryptoCurrencies.Bitcoin.Bitcoin().Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new CryptoCurrencies.Ethereum.Ethereum().Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            new CryptoCurrencies.Dogecoin.Dogecoin().Show();
        }
    }
}
