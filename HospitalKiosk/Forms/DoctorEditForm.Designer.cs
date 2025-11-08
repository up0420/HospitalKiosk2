namespace HospitalKiosk.Forms
{
    partial class DoctorEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDoctorCode;
        private System.Windows.Forms.Label lblDoctorName;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.Label lblLicenseNumber;
        private System.Windows.Forms.Label lblPhoneNumber;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtDoctorCode;
        private System.Windows.Forms.TextBox txtDoctorName;
        private System.Windows.Forms.TextBox txtLicenseNumber;
        private System.Windows.Forms.TextBox txtPhoneNumber;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDoctorCode = new System.Windows.Forms.Label();
            this.lblDoctorName = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.lblLicenseNumber = new System.Windows.Forms.Label();
            this.lblPhoneNumber = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtDoctorCode = new System.Windows.Forms.TextBox();
            this.txtDoctorName = new System.Windows.Forms.TextBox();
            this.txtLicenseNumber = new System.Windows.Forms.TextBox();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(159, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "새 의사 추가";
            //
            // lblDoctorCode
            //
            this.lblDoctorCode.AutoSize = true;
            this.lblDoctorCode.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblDoctorCode.Location = new System.Drawing.Point(50, 80);
            this.lblDoctorCode.Name = "lblDoctorCode";
            this.lblDoctorCode.Size = new System.Drawing.Size(78, 19);
            this.lblDoctorCode.TabIndex = 1;
            this.lblDoctorCode.Text = "의사 코드:";
            //
            // txtDoctorCode
            //
            this.txtDoctorCode.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtDoctorCode.Location = new System.Drawing.Point(180, 77);
            this.txtDoctorCode.Name = "txtDoctorCode";
            this.txtDoctorCode.Size = new System.Drawing.Size(300, 25);
            this.txtDoctorCode.TabIndex = 2;
            //
            // lblDoctorName
            //
            this.lblDoctorName.AutoSize = true;
            this.lblDoctorName.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblDoctorName.Location = new System.Drawing.Point(50, 120);
            this.lblDoctorName.Name = "lblDoctorName";
            this.lblDoctorName.Size = new System.Drawing.Size(39, 19);
            this.lblDoctorName.TabIndex = 3;
            this.lblDoctorName.Text = "이름:";
            //
            // txtDoctorName
            //
            this.txtDoctorName.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtDoctorName.Location = new System.Drawing.Point(180, 117);
            this.txtDoctorName.Name = "txtDoctorName";
            this.txtDoctorName.Size = new System.Drawing.Size(300, 25);
            this.txtDoctorName.TabIndex = 4;
            //
            // lblDepartment
            //
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblDepartment.Location = new System.Drawing.Point(50, 160);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(51, 19);
            this.lblDepartment.TabIndex = 5;
            this.lblDepartment.Text = "진료과:";
            //
            // cmbDepartment
            //
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(180, 157);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(300, 25);
            this.cmbDepartment.TabIndex = 6;
            //
            // lblLicenseNumber
            //
            this.lblLicenseNumber.AutoSize = true;
            this.lblLicenseNumber.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblLicenseNumber.Location = new System.Drawing.Point(50, 200);
            this.lblLicenseNumber.Name = "lblLicenseNumber";
            this.lblLicenseNumber.Size = new System.Drawing.Size(78, 19);
            this.lblLicenseNumber.TabIndex = 7;
            this.lblLicenseNumber.Text = "면허 번호:";
            //
            // txtLicenseNumber
            //
            this.txtLicenseNumber.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtLicenseNumber.Location = new System.Drawing.Point(180, 197);
            this.txtLicenseNumber.Name = "txtLicenseNumber";
            this.txtLicenseNumber.Size = new System.Drawing.Size(300, 25);
            this.txtLicenseNumber.TabIndex = 8;
            //
            // lblPhoneNumber
            //
            this.lblPhoneNumber.AutoSize = true;
            this.lblPhoneNumber.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblPhoneNumber.Location = new System.Drawing.Point(50, 240);
            this.lblPhoneNumber.Name = "lblPhoneNumber";
            this.lblPhoneNumber.Size = new System.Drawing.Size(78, 19);
            this.lblPhoneNumber.TabIndex = 9;
            this.lblPhoneNumber.Text = "전화번호:";
            //
            // txtPhoneNumber
            //
            this.txtPhoneNumber.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtPhoneNumber.Location = new System.Drawing.Point(180, 237);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(300, 25);
            this.txtPhoneNumber.TabIndex = 10;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblEmail.Location = new System.Drawing.Point(50, 280);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(51, 19);
            this.lblEmail.TabIndex = 11;
            this.lblEmail.Text = "이메일:";
            //
            // txtEmail
            //
            this.txtEmail.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtEmail.Location = new System.Drawing.Point(180, 277);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 25);
            this.txtEmail.TabIndex = 12;
            //
            // chkIsActive
            //
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.chkIsActive.Location = new System.Drawing.Point(180, 320);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(98, 23);
            this.chkIsActive.TabIndex = 13;
            this.chkIsActive.Text = "활성 상태";
            this.chkIsActive.UseVisualStyleBackColor = true;
            //
            // btnSave
            //
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(180, 370);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 45);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(320, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 45);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // DoctorEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(534, 451);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtPhoneNumber);
            this.Controls.Add(this.lblPhoneNumber);
            this.Controls.Add(this.txtLicenseNumber);
            this.Controls.Add(this.lblLicenseNumber);
            this.Controls.Add(this.cmbDepartment);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.txtDoctorName);
            this.Controls.Add(this.lblDoctorName);
            this.Controls.Add(this.txtDoctorCode);
            this.Controls.Add(this.lblDoctorCode);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoctorEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "의사 정보";
            this.Load += new System.EventHandler(this.DoctorEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
