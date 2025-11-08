namespace HospitalKiosk.Forms
{
    partial class AdminMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label lblTodayAppointments;
        private System.Windows.Forms.Label lblDoctorCount;
        private System.Windows.Forms.Label lblPatientCount;
        private System.Windows.Forms.Button btnDoctorManagement;
        private System.Windows.Forms.Button btnScheduleManagement;
        private System.Windows.Forms.Button btnAppointmentManagement;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnLogout;

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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelStats = new System.Windows.Forms.Panel();
            this.lblTodayAppointments = new System.Windows.Forms.Label();
            this.lblDoctorCount = new System.Windows.Forms.Label();
            this.lblPatientCount = new System.Windows.Forms.Label();
            this.btnDoctorManagement = new System.Windows.Forms.Button();
            this.btnScheduleManagement = new System.Windows.Forms.Button();
            this.btnAppointmentManagement = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.SuspendLayout();
            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblWelcome);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1024, 100);
            this.panelHeader.TabIndex = 0;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(313, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "관리자 대시보드";
            //
            // lblWelcome
            //
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(35, 70);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(129, 20);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "환영합니다, 님";
            //
            // panelStats
            //
            this.panelStats.BackColor = System.Drawing.Color.White;
            this.panelStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStats.Controls.Add(this.lblTodayAppointments);
            this.panelStats.Controls.Add(this.lblDoctorCount);
            this.panelStats.Controls.Add(this.lblPatientCount);
            this.panelStats.Location = new System.Drawing.Point(30, 130);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(964, 100);
            this.panelStats.TabIndex = 1;
            //
            // lblTodayAppointments
            //
            this.lblTodayAppointments.AutoSize = true;
            this.lblTodayAppointments.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTodayAppointments.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTodayAppointments.Location = new System.Drawing.Point(30, 25);
            this.lblTodayAppointments.Name = "lblTodayAppointments";
            this.lblTodayAppointments.Size = new System.Drawing.Size(258, 21);
            this.lblTodayAppointments.TabIndex = 0;
            this.lblTodayAppointments.Text = "오늘 예약: 0건 (대기: 0건)";
            //
            // lblDoctorCount
            //
            this.lblDoctorCount.AutoSize = true;
            this.lblDoctorCount.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblDoctorCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lblDoctorCount.Location = new System.Drawing.Point(380, 25);
            this.lblDoctorCount.Name = "lblDoctorCount";
            this.lblDoctorCount.Size = new System.Drawing.Size(126, 21);
            this.lblDoctorCount.TabIndex = 1;
            this.lblDoctorCount.Text = "활성 의사: 0명";
            //
            // lblPatientCount
            //
            this.lblPatientCount.AutoSize = true;
            this.lblPatientCount.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblPatientCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.lblPatientCount.Location = new System.Drawing.Point(680, 25);
            this.lblPatientCount.Name = "lblPatientCount";
            this.lblPatientCount.Size = new System.Drawing.Size(126, 21);
            this.lblPatientCount.TabIndex = 2;
            this.lblPatientCount.Text = "등록 환자: 0명";
            //
            // btnDoctorManagement
            //
            this.btnDoctorManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnDoctorManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoctorManagement.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btnDoctorManagement.ForeColor = System.Drawing.Color.White;
            this.btnDoctorManagement.Location = new System.Drawing.Point(80, 280);
            this.btnDoctorManagement.Name = "btnDoctorManagement";
            this.btnDoctorManagement.Size = new System.Drawing.Size(200, 100);
            this.btnDoctorManagement.TabIndex = 2;
            this.btnDoctorManagement.Text = "의사 관리";
            this.btnDoctorManagement.UseVisualStyleBackColor = false;
            this.btnDoctorManagement.Click += new System.EventHandler(this.BtnDoctorManagement_Click);
            //
            // btnScheduleManagement
            //
            this.btnScheduleManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnScheduleManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleManagement.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btnScheduleManagement.ForeColor = System.Drawing.Color.White;
            this.btnScheduleManagement.Location = new System.Drawing.Point(320, 280);
            this.btnScheduleManagement.Name = "btnScheduleManagement";
            this.btnScheduleManagement.Size = new System.Drawing.Size(200, 100);
            this.btnScheduleManagement.TabIndex = 3;
            this.btnScheduleManagement.Text = "의사 일정 관리\r\n(캘린더)";
            this.btnScheduleManagement.UseVisualStyleBackColor = false;
            this.btnScheduleManagement.Click += new System.EventHandler(this.BtnScheduleManagement_Click);
            //
            // btnAppointmentManagement
            //
            this.btnAppointmentManagement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnAppointmentManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppointmentManagement.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.btnAppointmentManagement.ForeColor = System.Drawing.Color.White;
            this.btnAppointmentManagement.Location = new System.Drawing.Point(560, 280);
            this.btnAppointmentManagement.Name = "btnAppointmentManagement";
            this.btnAppointmentManagement.Size = new System.Drawing.Size(200, 100);
            this.btnAppointmentManagement.TabIndex = 4;
            this.btnAppointmentManagement.Text = "예약 관리";
            this.btnAppointmentManagement.UseVisualStyleBackColor = false;
            this.btnAppointmentManagement.Click += new System.EventHandler(this.BtnAppointmentManagement_Click);
            //
            // btnRefresh
            //
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(320, 430);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 50);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "통계 새로고침";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            //
            // btnLogout
            //
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(560, 430);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(150, 50);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "로그아웃";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            //
            // AdminMainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1024, 550);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnAppointmentManagement);
            this.Controls.Add(this.btnScheduleManagement);
            this.Controls.Add(this.btnDoctorManagement);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AdminMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "병원 키오스크 - 관리자";
            this.Load += new System.EventHandler(this.AdminMainForm_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelStats.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
