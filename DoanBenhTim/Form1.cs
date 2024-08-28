using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoanBenhTim
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        int number = 0;
        Color[] color = { Color.Navy, Color.SteelBlue, Color.DodgerBlue, Color.MidnightBlue, Color.DarkBlue, Color.MediumBlue, Color.Blue, Color.RoyalBlue, Color.CornflowerBlue, Color.Indigo };

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            number = rand.Next(0, 9);
            btnStart.BackColor = color[number];
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            Form a = new frmDoanBenh();
            a.ShowDialog();



            
        }
    }
}
