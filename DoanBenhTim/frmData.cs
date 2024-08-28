using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DoanBenhTim
{
    public partial class frmData : Form
    {
        public frmData()
        {
            InitializeComponent();
        }

        private void frmData_Load(object sender, EventArgs e)
        {
            MyData mydata = new MyData();
            string[] a = File.ReadAllLines("processed.cleveland.txt");
            for (int i = 0; i < 303; i++)
            {
                string[] t = a[i].Split(',');
                double[] d = new double[14];
                for (int j = 0; j < 14; j++)
                {
                    d[j] = double.Parse(t[j]);
                }
                mydata.ThemDuLieu(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7], d[8], d[9], d[10], d[11], d[12], d[13]);
            }
            dgvLichSu.DataSource = MyData.dt;
        }
    }
}
