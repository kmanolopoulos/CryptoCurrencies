﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CryptoCurrencies
{
    public partial class Form1 : Form
    {
        public Form1()
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
