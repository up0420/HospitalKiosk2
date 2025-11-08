namespace HospitalKiosk.Forms
{
    partial class PatientRegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Panel pnlGender;
        private System.Windows.Forms.RadioButton rbMale;
        private System.Windows.Forms.RadioButton rbFemale;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblEmergency;
        private System.Windows.Forms.TextBox txtEmergencyContact;
        private System.Windows.Forms.Button btnRegister;
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.lblGender = new System.Windows.Forms.Label();
            this.pnlGender = new System.Windows.Forms.Panel();
            this.rbMale = new System.Windows.Forms.RadioButton();
            this.rbFemale = new System.Windows.Forms.RadioButton();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblEmergency = new System.Windows.Forms.Label();
            this.txtEmergencyContact = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlGender.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(560, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "환자 정보 등록";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblName
            //
            this.lblName.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblName.Location = new System.Drawing.Point(20, 80);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(120, 30);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "환자 이름:";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtName
            //
            this.txtName.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.txtName.Location = new System.Drawing.Point(160, 80);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(380, 29);
            this.txtName.TabIndex = 2;
            //
            // lblBirthDate
            //
            this.lblBirthDate.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblBirthDate.Location = new System.Drawing.Point(20, 130);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(120, 30);
            this.lblBirthDate.TabIndex = 3;
            this.lblBirthDate.Text = "생년월일:";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // dtpBirthDate
            //
            this.dtpBirthDate.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthDate.Location = new System.Drawing.Point(160, 130);
            this.dtpBirthDate.MaxDate = new System.DateTime(2025, 12, 31, 0, 0, 0, 0);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new System.Drawing.Size(380, 29);
            this.dtpBirthDate.TabIndex = 4;
            //
            // lblGender
            //
            this.lblGender.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblGender.Location = new System.Drawing.Point(20, 180);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(120, 30);
            this.lblGender.TabIndex = 5;
            this.lblGender.Text = "성별:";
            this.lblGender.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlGender
            //
            this.pnlGender.Controls.Add(this.rbMale);
            this.pnlGender.Controls.Add(this.rbFemale);
            this.pnlGender.Location = new System.Drawing.Point(160, 180);
            this.pnlGender.Name = "pnlGender";
            this.pnlGender.Size = new System.Drawing.Size(380, 30);
            this.pnlGender.TabIndex = 6;
            //
            // rbMale
            //
            this.rbMale.AutoSize = true;
            this.rbMale.Checked = true;
            this.rbMale.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.rbMale.Location = new System.Drawing.Point(0, 5);
            this.rbMale.Name = "rbMale";
            this.rbMale.Size = new System.Drawing.Size(58, 25);
            this.rbMale.TabIndex = 0;
            this.rbMale.TabStop = true;
            this.rbMale.Text = "남성";
            this.rbMale.UseVisualStyleBackColor = true;
            //
            // rbFemale
            //
            this.rbFemale.AutoSize = true;
            this.rbFemale.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.rbFemale.Location = new System.Drawing.Point(100, 5);
            this.rbFemale.Name = "rbFemale";
            this.rbFemale.Size = new System.Drawing.Size(58, 25);
            this.rbFemale.TabIndex = 1;
            this.rbFemale.Text = "여성";
            this.rbFemale.UseVisualStyleBackColor = true;
            //
            // lblPhone
            //
            this.lblPhone.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblPhone.Location = new System.Drawing.Point(20, 230);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(120, 30);
            this.lblPhone.TabIndex = 7;
            this.lblPhone.Text = "전화번호:";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtPhone
            //
            this.txtPhone.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.txtPhone.Location = new System.Drawing.Point(160, 230);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(380, 29);
            this.txtPhone.TabIndex = 8;
            //
            // lblAddress
            //
            this.lblAddress.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblAddress.Location = new System.Drawing.Point(20, 280);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(120, 30);
            this.lblAddress.TabIndex = 9;
            this.lblAddress.Text = "주소:";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtAddress
            //
            this.txtAddress.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.txtAddress.Location = new System.Drawing.Point(160, 280);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(380, 29);
            this.txtAddress.TabIndex = 10;
            //
            // lblEmergency
            //
            this.lblEmergency.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblEmergency.Location = new System.Drawing.Point(20, 330);
            this.lblEmergency.Name = "lblEmergency";
            this.lblEmergency.Size = new System.Drawing.Size(120, 30);
            this.lblEmergency.TabIndex = 11;
            this.lblEmergency.Text = "비상연락처:";
            this.lblEmergency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtEmergencyContact
            //
            this.txtEmergencyContact.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.txtEmergencyContact.Location = new System.Drawing.Point(160, 330);
            this.txtEmergencyContact.Name = "txtEmergencyContact";
            this.txtEmergencyContact.Size = new System.Drawing.Size(380, 29);
            this.txtEmergencyContact.TabIndex = 12;
            //
            // btnRegister
            //
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(200, 390);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(150, 45);
            this.btnRegister.TabIndex = 13;
            this.btnRegister.Text = "등록";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(360, 390);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 45);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // PatientRegisterForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 480);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtEmergencyContact);
            this.Controls.Add(this.lblEmergency);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.pnlGender);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.dtpBirthDate);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatientRegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "신규 환자 등록";
            this.pnlGender.ResumeLayout(false);
            this.pnlGender.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
