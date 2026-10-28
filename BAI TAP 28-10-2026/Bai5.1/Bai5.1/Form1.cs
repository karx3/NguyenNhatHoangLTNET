using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;   

namespace Bai5._1
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=ql_sinhvien;Uid=root;Pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (txtUsername.Text == "")
            {
                errorProvider1.SetError(txtUsername, "Nhập tên đăng nhập");
            }

            if (txtPassword.Text == "")
            {
                errorProvider1.SetError(txtPassword, "Nhập mật khẩu");
            }

            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                MessageBox.Show("Đăng nhập thành công!");
                LoadDanhSachSinhVien(); 
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void LoadDanhSachSinhVien()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string sql = "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB FROM SinhVien";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvSinhVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu từ MySQL: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}