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
    public partial class frmDoanBenh : Form
    {
        MyData mydata;
        LichSu lichsu;
        LogisticProgram lg = new LogisticProgram();
        public frmDoanBenh()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnDoan_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtTuoi.Text) || 
                String.IsNullOrEmpty(txtGioiTinh.Text) || 
                String.IsNullOrEmpty(txtLoaiDauNguc.Text) || 
                String.IsNullOrEmpty(txtHuyetApNghi.Text) || 
                String.IsNullOrEmpty(txtChol.Text) || 
                String.IsNullOrEmpty(txtDuongHuyetApNhanh.Text) || 
                String.IsNullOrEmpty(txtKetquaDienTam.Text) || 
                String.IsNullOrEmpty(txtTanSoTim.Text) || 
                String.IsNullOrEmpty(txtDauNgucDoHoatDong.Text) || 
                String.IsNullOrEmpty(txtChenhLech.Text) || 
                String.IsNullOrEmpty(txtGocDoCuaDuong.Text) || 
                String.IsNullOrEmpty(txtSoMachMauChinh.Text) || 
                String.IsNullOrEmpty(txtThala.Text) ) {
                MessageBox.Show("Dữ liệu không đầy đủ");
                return;
            }
            double t1 = double.Parse(txtTuoi.Text);
            if (t1 < 1.0 || t1 > 99.0)
            {
                MessageBox.Show("Độ tuổi ngoài giới hạn");
                txtTuoi.Focus();
                return;
            }
            double t2 = double.Parse(txtGioiTinh.Text);
            if (t2!=0.0 && t2!=1.0)
            {
                MessageBox.Show("Giới tính ở ngoài tầm kiểm soát");
                txtGioiTinh.Focus();
                return;
            }
            double t3 = double.Parse(txtLoaiDauNguc.Text);
            if (t3 != 0.0 && t3 != 1.0 && t3 != 2.0 && t3 != 3.0 && t3 != 4.0)
            {
                MessageBox.Show("Loại đau ngực chỉ chọn 1 - 2 - 3 - 4");
                txtLoaiDauNguc.Focus();
                return;
            }
            double t4 = double.Parse(txtHuyetApNghi.Text);
            if (t4 <= 10.0 || t4 >= 500.0)
            {
                MessageBox.Show("Huyết áp ngoài giới hạn");
                txtHuyetApNghi.Focus();
                return;
            }
            double t5 = double.Parse(txtChol.Text);
            if (t5>500.0 || t5<0)
            {
                MessageBox.Show("Cholesterol ngoài giới hạn");
                txtChol.Focus();
                return;
            }
            double t6 = double.Parse(txtDuongHuyetApNhanh.Text);
            if (t6 != 0.0 && t6 != 1.0) 
            {
                MessageBox.Show("Đường huyết áp không hợp lệ");
                txtDuongHuyetApNhanh.Focus();
                return;
            }
            double t7 = double.Parse(txtKetquaDienTam.Text);
            if (t7 != 0.0 && t7 != 1.0 && t7 != 2.0)
            {
                MessageBox.Show("Kết quả điện tâm không hợp lệ");
                txtKetquaDienTam.Focus();
                return;
            }
            double t8 = double.Parse(txtTanSoTim.Text);
            if (t8>300.0 || t8<0)
            {
                MessageBox.Show("Tần số tim ở ngoài tầm kiểm soát");
                txtTanSoTim.Focus();
                return;
            }
            double t9 = double.Parse(txtDauNgucDoHoatDong.Text);
            if (t2 != 0.0 && t2 != 1.0)
            {
                MessageBox.Show("Đau ngực chỉ chọn có(1) hoặc không(0)");
                txtDauNgucDoHoatDong.Focus();
                return;
            }
            double t10 = double.Parse(txtChenhLech.Text);
            if (t10>20.0 || t10<0)
            {
                MessageBox.Show("Độ chênh lệch ở ngoài tầm kiểm soát");
                txtChenhLech.Focus();
                return;
            }
            double t11 = double.Parse(txtGocDoCuaDuong.Text);
            if (t11 != 1.0 && t11 != 2.0 && t11 != 3.0)
            {
                MessageBox.Show("Góc độ của đường ST chỉ 1 - 2 - 3");
                txtGocDoCuaDuong.Focus();
                return;
            }
            double t12 = double.Parse(txtSoMachMauChinh.Text);
            if (t12 != 0.0 && t12 != 1.0 && t12 != 2.0 && t12 != 3.0)
            {
                MessageBox.Show("Số mạch máu chính chỉ 0 - 1 - 2 - 3");
                txtSoMachMauChinh.Focus();
                return;
            }
            double t13 = double.Parse(txtThala.Text);
            if (t13 != 3.0 && t13 != 6.0 && t13 != 7.0)
            {
                MessageBox.Show("Thalassemia chỉ 3 - 6 - 7");
                txtSoMachMauChinh.Focus();
                return;
            }
            LogisticProgram lg = new LogisticProgram();

            KetQua kq = lg.HuanLuyen(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            string show = "<<Độ chính xác mô hình là " + kq.DoChinhXac.ToString() + ">>\n KẾT QUẢ CỦA BẠN: " + (kq.KqtQuaDuDoan).ToString("##.# %");
            if (kq.KqtQuaDuDoan >= 0.5)
            {
                show += "\n Tỉ lệ bệnh của bạn khá cao. Khả năng cao là bạn bị bệnh tim.";
            }
            else
            {
                show += "\n Tỉ lệ bệnh của bạn khá thấp. Khả năng cao là bạn không bị bệnh tim.";
            }
            lichsu.ThemDuLieu(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, kq.KqtQuaDuDoan);
            MessageBox.Show(show);
        }

        private void frmDoanBenh_Load(object sender, EventArgs e)
        {
            lichsu = new LichSu();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txtChenhLech.Clear();
            txtChol.Clear();
            txtDauNgucDoHoatDong.Clear();
            txtDuongHuyetApNhanh.Clear();
            txtGocDoCuaDuong.Clear();
            txtGioiTinh.Clear();
            txtHuyetApNghi.Clear();
            txtKetquaDienTam.Clear();
            txtLoaiDauNguc.Clear();
            txtSoMachMauChinh.Clear();
            txtTanSoTim.Clear();
            txtTuoi.Clear();
            txtThala.Clear();
        }

        private void btnLichSu_Click(object sender, EventArgs e)
        {
            Form lichsu = new frmNhatKy();
            lichsu.ShowDialog();
        }

        private void btnDataset_Click(object sender, EventArgs e)
        {
            Form mydata = new frmData();
            mydata.ShowDialog();
        }
    }
}
