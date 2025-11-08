namespace HospitalKiosk.Forms
{
    partial class AppointmentForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPatientSearch;
        private System.Windows.Forms.TextBox txtPatientSearch;
        private System.Windows.Forms.Button btnSearchPatient;
        private System.Windows.Forms.Label lblPatientInfo;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.Label lblAppointmentDate;
        private System.Windows.Forms.DateTimePicker dtpAppointmentDate;
        private System.Windows.Forms.Label lblTimeSlots;
        private System.Windows.Forms.FlowLayoutPanel pnlTimeSlots;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Button btnConfirm;
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
            this.lblPatientSearch = new System.Windows.Forms.Label();
            this.txtPatientSearch = new System.Windows.Forms.TextBox();
            this.btnSearchPatient = new System.Windows.Forms.Button();
            this.lblPatientInfo = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.lblDoctor = new System.Windows.Forms.Label();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.lblAppointmentDate = new System.Windows.Forms.Label();
            this.dtpAppointmentDate = new System.Windows.Forms.DateTimePicker();
            this.lblTimeSlots = new System.Windows.Forms.Label();
            this.pnlTimeSlots = new System.Windows.Forms.FlowLayoutPanel();
            this.lblReason = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(860, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "진료 예약";
            //
            // lblPatientSearch
            //
            this.lblPatientSearch.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblPatientSearch.Location = new System.Drawing.Point(20, 70);
            this.lblPatientSearch.Name = "lblPatientSearch";
            this.lblPatientSearch.Size = new System.Drawing.Size(150, 30);
            this.lblPatientSearch.TabIndex = 1;
            this.lblPatientSearch.Text = "환자 번호 / 이름:";
            this.lblPatientSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtPatientSearch
            //
            this.txtPatientSearch.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtPatientSearch.Location = new System.Drawing.Point(180, 70);
            this.txtPatientSearch.Name = "txtPatientSearch";
            this.txtPatientSearch.Size = new System.Drawing.Size(300, 27);
            this.txtPatientSearch.TabIndex = 2;
            //
            // btnSearchPatient
            //
            this.btnSearchPatient.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnSearchPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchPatient.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.btnSearchPatient.ForeColor = System.Drawing.Color.White;
            this.btnSearchPatient.Location = new System.Drawing.Point(490, 70);
            this.btnSearchPatient.Name = "btnSearchPatient";
            this.btnSearchPatient.Size = new System.Drawing.Size(100, 30);
            this.btnSearchPatient.TabIndex = 3;
            this.btnSearchPatient.Text = "검색";
            this.btnSearchPatient.UseVisualStyleBackColor = false;
            this.btnSearchPatient.Click += new System.EventHandler(this.BtnSearchPatient_Click);
            //
            // lblPatientInfo
            //
            this.lblPatientInfo.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblPatientInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblPatientInfo.Location = new System.Drawing.Point(20, 110);
            this.lblPatientInfo.Name = "lblPatientInfo";
            this.lblPatientInfo.Size = new System.Drawing.Size(860, 30);
            this.lblPatientInfo.TabIndex = 4;
            this.lblPatientInfo.Text = "환자를 검색해주세요";
            //
            // lblDepartment
            //
            this.lblDepartment.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblDepartment.Location = new System.Drawing.Point(20, 160);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(150, 30);
            this.lblDepartment.TabIndex = 5;
            this.lblDepartment.Text = "진료과:";
            this.lblDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbDepartment
            //
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(180, 160);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(400, 28);
            this.cmbDepartment.TabIndex = 6;
            this.cmbDepartment.SelectedIndexChanged += new System.EventHandler(this.CmbDepartment_SelectedIndexChanged);
            //
            // lblDoctor
            //
            this.lblDoctor.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblDoctor.Location = new System.Drawing.Point(20, 210);
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Size = new System.Drawing.Size(150, 30);
            this.lblDoctor.TabIndex = 7;
            this.lblDoctor.Text = "담당 의사:";
            this.lblDoctor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbDoctor
            //
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.Enabled = false;
            this.cmbDoctor.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.cmbDoctor.FormattingEnabled = true;
            this.cmbDoctor.Location = new System.Drawing.Point(180, 210);
            this.cmbDoctor.Name = "cmbDoctor";
            this.cmbDoctor.Size = new System.Drawing.Size(400, 28);
            this.cmbDoctor.TabIndex = 8;
            this.cmbDoctor.SelectedIndexChanged += new System.EventHandler(this.CmbDoctor_SelectedIndexChanged);
            //
            // lblAppointmentDate
            //
            this.lblAppointmentDate.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblAppointmentDate.Location = new System.Drawing.Point(20, 260);
            this.lblAppointmentDate.Name = "lblAppointmentDate";
            this.lblAppointmentDate.Size = new System.Drawing.Size(150, 30);
            this.lblAppointmentDate.TabIndex = 9;
            this.lblAppointmentDate.Text = "예약 날짜:";
            this.lblAppointmentDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // dtpAppointmentDate
            //
            this.dtpAppointmentDate.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.dtpAppointmentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAppointmentDate.Location = new System.Drawing.Point(180, 260);
            this.dtpAppointmentDate.Name = "dtpAppointmentDate";
            this.dtpAppointmentDate.Size = new System.Drawing.Size(400, 27);
            this.dtpAppointmentDate.TabIndex = 10;
            this.dtpAppointmentDate.ValueChanged += new System.EventHandler(this.DtpAppointmentDate_ValueChanged);
            //
            // lblTimeSlots
            //
            this.lblTimeSlots.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblTimeSlots.Location = new System.Drawing.Point(20, 310);
            this.lblTimeSlots.Name = "lblTimeSlots";
            this.lblTimeSlots.Size = new System.Drawing.Size(150, 30);
            this.lblTimeSlots.TabIndex = 11;
            this.lblTimeSlots.Text = "예약 시간:";
            this.lblTimeSlots.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlTimeSlots
            //
            this.pnlTimeSlots.AutoScroll = true;
            this.pnlTimeSlots.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTimeSlots.Location = new System.Drawing.Point(180, 310);
            this.pnlTimeSlots.Name = "pnlTimeSlots";
            this.pnlTimeSlots.Size = new System.Drawing.Size(640, 150);
            this.pnlTimeSlots.TabIndex = 12;
            //
            // lblReason
            //
            this.lblReason.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblReason.Location = new System.Drawing.Point(20, 470);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(150, 30);
            this.lblReason.TabIndex = 13;
            this.lblReason.Text = "진료 사유:";
            this.lblReason.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtReason
            //
            this.txtReason.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.txtReason.Location = new System.Drawing.Point(180, 470);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(640, 60);
            this.txtReason.TabIndex = 14;
            //
            // btnConfirm
            //
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnConfirm.Enabled = false;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(520, 550);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(150, 45);
            this.btnConfirm.TabIndex = 15;
            this.btnConfirm.Text = "예약 확인";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(680, 550);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 45);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // AppointmentForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.pnlTimeSlots);
            this.Controls.Add(this.lblTimeSlots);
            this.Controls.Add(this.dtpAppointmentDate);
            this.Controls.Add(this.lblAppointmentDate);
            this.Controls.Add(this.cmbDoctor);
            this.Controls.Add(this.lblDoctor);
            this.Controls.Add(this.cmbDepartment);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.lblPatientInfo);
            this.Controls.Add(this.btnSearchPatient);
            this.Controls.Add(this.txtPatientSearch);
            this.Controls.Add(this.lblPatientSearch);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AppointmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "진료 예약";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
