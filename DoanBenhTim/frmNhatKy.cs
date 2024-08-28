using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DoanBenhTim
{
    public partial class frmNhatKy : Form
    {

        public frmNhatKy()
        {
            InitializeComponent();
        }

        private void frmNhatKy_Load(object sender, EventArgs e)
        {
            dgvLichSu.DataSource = LichSu.dt;
        }
    }
}
