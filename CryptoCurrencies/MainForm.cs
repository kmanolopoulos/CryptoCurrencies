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
            new Bitcoin.Bitcoin().Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new Ethereum.Ethereum().Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            new Dogecoin.Dogecoin().Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            new Ripple.Ripple().Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            new Tron.Tron().Show();
        }
    }
}
