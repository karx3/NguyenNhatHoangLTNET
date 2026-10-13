using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Bai4_6
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Student> students = new BindingList<Student>();
        private readonly BindingSource bsStudents = new BindingSource();
        private bool inputSelectionActive = false;

        public Form1()
        {
            InitializeComponent();
            ConfigureForm();
            ConfigureBindings();
        }

        private void ConfigureForm()
        {
            Text = "Bài 4.6 - Quản lý sinh viên";
            StartPosition = FormStartPosition.CenterScreen;

            cboGender.Items.Clear();
            cboGender.Items.Add("Nam");
            cboGender.Items.Add("Nữ");
            cboGender.DropDownStyle = ComboBoxStyle.DropDownList;

            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            dgvStudents.AutoGenerateColumns = true;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.CellClick += dgvStudents_CellClick;
            dgvStudents.MouseClick += dgvStudents_MouseClick;
        }

        private void ConfigureBindings()
        {
            bsStudents.DataSource = students;
            dgvStudents.DataSource = bsStudents;

            ConfigureInputBindings();

            bsStudents.CurrentChanged += BsStudents_CurrentChanged;
            bsStudents.AddingNew += (s, e) => e.NewObject = new Student();
            UpdateAvatar();
        }

        private void ConfigureInputBindings()
        {
            // Xóa binding cũ trước khi tạo lại để tránh bị trùng.
            txtStudentId.DataBindings.Clear();
            txtFullName.DataBindings.Clear();
            txtAge.DataBindings.Clear();
            cboGender.DataBindings.Clear();

            txtStudentId.DataBindings.Add("Text", bsStudents, "StudentId",
                true, DataSourceUpdateMode.OnPropertyChanged);
            txtFullName.DataBindings.Add("Text", bsStudents, "FullName",
                true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", bsStudents, "Age",
                true, DataSourceUpdateMode.OnPropertyChanged, 0);
            cboGender.DataBindings.Add("SelectedItem", bsStudents, "Gender",
                true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void ClearInputBindings()
        {
            txtStudentId.DataBindings.Clear();
            txtFullName.DataBindings.Clear();
            txtAge.DataBindings.Clear();
            cboGender.DataBindings.Clear();
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvStudents.Rows.Count)
                return;

            // Chọn lại sinh viên và nối lại các ô nhập với BindingSource.
            bsStudents.Position = e.RowIndex;
            inputSelectionActive = true;
            ConfigureInputBindings();
            UpdateAvatar();
        }

        private Student CurrentStudent
        {
            get { return inputSelectionActive ? bsStudents.Current as Student : null; }
        }

        private void BsStudents_CurrentChanged(object sender, EventArgs e)
        {
            UpdateAvatar();
        }

        private void UpdateAvatar()
        {
            string path = CurrentStudent == null ? null : CurrentStudent.AvatarPath;

            Image oldImage = picAvatar.Image;
            picAvatar.Image = null;
            if (oldImage != null)
                oldImage.Dispose();

            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                try
                {
                    using (Image source = Image.FromFile(path))
                    {
                        picAvatar.Image = new Bitmap(source);
                    }
                }
                catch
                {
                    picAvatar.Image = null;
                }
            }
        }

        private bool TryReadInput(out string id, out string name, out int age, out string gender)
        {
            id = txtStudentId.Text.Trim();
            name = txtFullName.Text.Trim();
            gender = cboGender.Text;
            age = 0;

            if (id.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên.");
                txtStudentId.Focus();
                return false;
            }

            if (name.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập họ tên.");
                txtFullName.Focus();
                return false;
            }

            if (!int.TryParse(txtAge.Text.Trim(), out age) || age < 1 || age > 120)
            {
                MessageBox.Show("Tuổi phải là số nguyên từ 1 đến 120.");
                txtAge.Focus();
                return false;
            }

            if (gender != "Nam" && gender != "Nữ")
            {
                MessageBox.Show("Vui lòng chọn giới tính.");
                cboGender.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string id, name, gender;
            int age;
            if (!TryReadInput(out id, out name, out age, out gender))
                return;

            foreach (Student item in students)
            {
                if (string.Equals(item.StudentId, id, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã sinh viên đã tồn tại.");
                    return;
                }
            }

            Student student = new Student
            {
                StudentId = id,
                FullName = name,
                Age = age,
                Gender = gender,
                AvatarPath = ""
            };

            students.Add(student);
            inputSelectionActive = true;
            bsStudents.Position = students.Count - 1;
            ConfigureInputBindings();
            MessageBox.Show("Đã thêm sinh viên.");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Student current = CurrentStudent;
            if (current == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần cập nhật.");
                return;
            }

            string id, name, gender;
            int age;
            if (!TryReadInput(out id, out name, out age, out gender))
                return;

            foreach (Student item in students)
            {
                if (!ReferenceEquals(item, current) &&
                    string.Equals(item.StudentId, id, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã sinh viên đã được sử dụng bởi sinh viên khác.");
                    return;
                }
            }

            current.StudentId = id;
            current.FullName = name;
            current.Age = age;
            current.Gender = gender;
            bsStudents.ResetCurrentItem();
            dgvStudents.Refresh();
            MessageBox.Show("Đã cập nhật thông tin sinh viên.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Student current = CurrentStudent;
            if (current == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa.");
                return;
            }

            DialogResult answer = MessageBox.Show(
                "Bạn có chắc muốn xóa sinh viên " + current.FullName + "?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer == DialogResult.Yes)
            {
                int index = bsStudents.Position;
                students.Remove(current);
                if (students.Count > 0)
                    bsStudents.Position = Math.Min(index, students.Count - 1);
                UpdateAvatar();
            }
        }

        private void btnChooseAvatar_Click(object sender, EventArgs e)
        {
            Student current = CurrentStudent;
            if (current == null)
            {
                MessageBox.Show("Hãy thêm hoặc chọn một sinh viên trước khi chọn ảnh.");
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Chọn ảnh đại diện";
                dialog.Filter = "Tệp hình ảnh|*.jpg;*.jpeg;*.png;*.bmp";
                dialog.Multiselect = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Image source = Image.FromFile(dialog.FileName))
                        {
                            Image newImage = new Bitmap(source);
                            Image oldImage = picAvatar.Image;
                            picAvatar.Image = newImage;
                            if (oldImage != null)
                                oldImage.Dispose();
                        }

                        current.AvatarPath = dialog.FileName;
                        bsStudents.ResetCurrentItem();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể mở ảnh: " + ex.Message);
                    }
                }
            }
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Xuất danh sách sinh viên";
                dialog.Filter = "CSV UTF-8 (*.csv)|*.csv";
                dialog.FileName = "DanhSachSinhVien.csv";
                dialog.DefaultExt = "csv";
                dialog.AddExtension = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    // UTF-8 có BOM giúp Excel nhận diện tiếng Việt tốt hơn.
                    using (StreamWriter writer = new StreamWriter(
                        dialog.FileName, false, new UTF8Encoding(true)))
                    {
                        writer.WriteLine("StudentId,FullName,Age,Gender,AvatarPath");

                        foreach (Student student in students)
                        {
                            writer.WriteLine(
                                CsvEscape(student.StudentId) + "," +
                                CsvEscape(student.FullName) + "," +
                                student.Age + "," +
                                CsvEscape(student.Gender) + "," +
                                CsvEscape(student.AvatarPath));
                        }
                    }

                    MessageBox.Show("Xuất CSV thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xuất file: " + ex.Message);
                }
            }
        }

        private string CsvEscape(string value)
        {
            if (value == null)
                return "\"\"";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (picAvatar.Image != null)
            {
                picAvatar.Image.Dispose();
                picAvatar.Image = null;
            }

            base.OnFormClosed(e);
        }
        private void dgvStudents_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hit = dgvStudents.HitTest(e.X, e.Y);

            // Chỉ reset ô nhập khi bấm vùng xám trống, không đụng đến danh sách.
            if (hit.Type == DataGridViewHitTestType.None)
            {
                inputSelectionActive = false;
                ClearInputBindings();

                txtStudentId.Clear();
                txtFullName.Clear();
                txtAge.Clear();
                cboGender.SelectedIndex = -1;

                Image oldImage = picAvatar.Image;
                picAvatar.Image = null;
                if (oldImage != null)
                    oldImage.Dispose();

                dgvStudents.ClearSelection();
            }
        }
    }
}
