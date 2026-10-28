using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Bai5._2
{
    public partial class Form1 : Form
    {
        // Cấu hình chuỗi kết nối đến MySQL của bạn
        private string connectionString = "Server=localhost;Database=ql_sinhvien;Uid=root;Pwd=;";

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        // Tải dữ liệu từ MySQL lên DataGridView
        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB FROM sinhvien";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvStudents.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Họ tên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = txtMaSV.Text.Trim();
            string hoTen = txtFullName.Text.Trim();
            string ngaySinh = dtpBirthDate.Value.ToString("yyyy-MM-dd");
            string gioiTinh = rdoMale.Checked ? "Nam" : "Nữ";
            string lop = txtLop.Text.Trim();

            if (!decimal.TryParse(txtDiemTB.Text.Trim(), out decimal diemTB))
            {
                MessageBox.Show("Điểm trung bình không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO sinhvien (MaSV, HoTen, NgaySinh, GioiTinh, Lop, DiemTB) VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Lop, @DiemTB)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", maSV);
                        cmd.Parameters.AddWithValue("@HoTen", hoTen);
                        cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                        cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                        cmd.Parameters.AddWithValue("@Lop", lop);
                        cmd.Parameters.AddWithValue("@DiemTB", diemTB);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Làm mới dữ liệu hiển thị
                        LoadData();

                        // Xóa trắng form sau khi thêm thành công
                        txtMaSV.Clear();
                        txtFullName.Clear();
                        txtLop.Clear();
                        txtDiemTB.Clear();
                        rdoMale.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu (Trùng Mã SV hoặc lỗi kết nối): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}