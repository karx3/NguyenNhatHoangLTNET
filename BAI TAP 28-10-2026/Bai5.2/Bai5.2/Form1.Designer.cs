namespace Bai5._2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblMaSV;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblLop;
        private System.Windows.Forms.Label lblDiemTB;

        private System.Windows.Forms.TextBox txtMaSV;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.RadioButton rdoMale;
        private System.Windows.Forms.RadioButton rdoFemale;
        private System.Windows.Forms.TextBox txtLop;
        private System.Windows.Forms.TextBox txtDiemTB;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvStudents;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblMaSV = new System.Windows.Forms.Label();
            txtMaSV = new System.Windows.Forms.TextBox();
            lblFullName = new System.Windows.Forms.Label();
            txtFullName = new System.Windows.Forms.TextBox();
            lblBirthDate = new System.Windows.Forms.Label();
            dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            lblGender = new System.Windows.Forms.Label();
            rdoMale = new System.Windows.Forms.RadioButton();
            rdoFemale = new System.Windows.Forms.RadioButton();
            lblLop = new System.Windows.Forms.Label();
            txtLop = new System.Windows.Forms.TextBox();
            lblDiemTB = new System.Windows.Forms.Label();
            txtDiemTB = new System.Windows.Forms.TextBox();
            btnSave = new System.Windows.Forms.Button();
            dgvStudents = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(dgvStudents)).BeginInit();
            SuspendLayout();

            // lblMaSV
            lblMaSV.AutoSize = true;
            lblMaSV.Location = new System.Drawing.Point(30, 30);
            lblMaSV.Name = "lblMaSV";
            lblMaSV.Size = new Size(95, 20);
            lblMaSV.Text = "Mã sinh viên:";

            // txtMaSV
            txtMaSV.Location = new System.Drawing.Point(150, 27);
            txtMaSV.Name = "txtMaSV";
            txtMaSV.Size = new Size(200, 27);

            // lblFullName
            lblFullName.AutoSize = true;
            lblFullName.Location = new System.Drawing.Point(30, 70);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(76, 20);
            lblFullName.Text = "Họ và tên:";

            // txtFullName
            txtFullName.Location = new System.Drawing.Point(150, 67);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new System.Drawing.Size(200, 27);

            // lblBirthDate
            lblBirthDate.AutoSize = true;
            lblBirthDate.Location = new System.Drawing.Point(30, 110);
            lblBirthDate.Name = "lblBirthDate";
            lblBirthDate.Size = new Size(77, 20);
            lblBirthDate.Text = "Ngày sinh:";

            // dtpBirthDate
            dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpBirthDate.Location = new System.Drawing.Point(150, 107);
            dtpBirthDate.Name = "dtpBirthDate";
            dtpBirthDate.Size = new System.Drawing.Size(200, 27);

            // lblGender
            lblGender.AutoSize = true;
            lblGender.Location = new System.Drawing.Point(30, 150);
            lblGender.Name = "lblGender";
            lblGender.Size = new Size(68, 20);
            lblGender.Text = "Giới tính:";

            // rdoMale
            rdoMale.AutoSize = true;
            rdoMale.Checked = true;
            rdoMale.Location = new System.Drawing.Point(150, 148);
            rdoMale.Name = "rdoMale";
            rdoMale.Size = new Size(62, 24);
            rdoMale.Text = "Nam";

            // rdoFemale
            rdoFemale.AutoSize = true;
            rdoFemale.Location = new System.Drawing.Point(225, 148);
            rdoFemale.Name = "rdoFemale";
            rdoFemale.Size = new Size(50, 24);
            rdoFemale.Text = "Nữ";

            // lblLop
            lblLop.AutoSize = true;
            lblLop.Location = new System.Drawing.Point(30, 190);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(37, 20);
            lblLop.Text = "Lớp:";

            // txtLop
            txtLop.Location = new System.Drawing.Point(150, 187);
            txtLop.Name = "txtLop";
            txtLop.Size = new System.Drawing.Size(200, 27);

            // lblDiemTB
            lblDiemTB.AutoSize = true;
            lblDiemTB.Location = new System.Drawing.Point(30, 230);
            lblDiemTB.Name = "lblDiemTB";
            lblDiemTB.Size = new Size(69, 20);
            lblDiemTB.Text = "Điểm TB:";

            // txtDiemTB
            txtDiemTB.Location = new System.Drawing.Point(150, 227);
            txtDiemTB.Name = "txtDiemTB";
            txtDiemTB.Size = new System.Drawing.Size(200, 27);

            // btnSave
            btnSave.Location = new System.Drawing.Point(150, 270);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 35);
            btnSave.Text = "Thêm Sinh Viên";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // dgvStudents
            dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Location = new System.Drawing.Point(380, 27);
            dgvStudents.Name = "dgvStudents";
            dgvStudents.RowHeadersWidth = 51;
            dgvStudents.Size = new System.Drawing.Size(550, 278);
            dgvStudents.TabIndex = 13;

            // Form1
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(950, 335);
            Controls.Add(dgvStudents);
            Controls.Add(btnSave);
            Controls.Add(txtDiemTB);
            Controls.Add(lblDiemTB);
            Controls.Add(txtLop);
            Controls.Add(lblLop);
            Controls.Add(rdoFemale);
            Controls.Add(rdoMale);
            Controls.Add(lblGender);
            Controls.Add(dtpBirthDate);
            Controls.Add(lblBirthDate);
            Controls.Add(txtFullName);
            Controls.Add(lblFullName);
            Controls.Add(txtMaSV);
            Controls.Add(lblMaSV);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Quản lý Sinh viên - MySQL";
            ((System.ComponentModel.ISupportInitialize)(dgvStudents)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}