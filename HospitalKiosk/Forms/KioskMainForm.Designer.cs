namespace HospitalKiosk.Forms
{
    partial class KioskMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnAppointment;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnCheckAppointment;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnAdmin;

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
            this.btnAppointment = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnCheckAppointment = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.lblTitle.Location = new System.Drawing.Point(125, 100);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(950, 100);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "병원 키오스크";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblWelcome
            //
            this.lblWelcome.Font = new System.Drawing.Font("맑은 고딕", 18F);
            this.lblWelcome.ForeColor = System.Drawing.Color.Gray;
            this.lblWelcome.Location = new System.Drawing.Point(125, 210);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(950, 50);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "이용하실 서비스를 선택해주세요";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnAppointment
            //
            this.btnAppointment.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnAppointment.FlatAppearance.BorderSize = 0;
            this.btnAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppointment.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.btnAppointment.ForeColor = System.Drawing.Color.White;
            this.btnAppointment.Location = new System.Drawing.Point(190, 300);
            this.btnAppointment.Name = "btnAppointment";
            this.btnAppointment.Size = new System.Drawing.Size(390, 155);
            this.btnAppointment.TabIndex = 2;
            this.btnAppointment.Text = "진료 예약";
            this.btnAppointment.UseVisualStyleBackColor = false;
            this.btnAppointment.Click += new System.EventHandler(this.BtnAppointment_Click);
            this.btnAppointment.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnAppointment.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            //
            // btnPayment
            //
            this.btnPayment.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.Location = new System.Drawing.Point(620, 300);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(390, 155);
            this.btnPayment.TabIndex = 3;
            this.btnPayment.Text = "진료비 수납";
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.BtnPayment_Click);
            this.btnPayment.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnPayment.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            //
            // btnCheckAppointment
            //
            this.btnCheckAppointment.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnCheckAppointment.FlatAppearance.BorderSize = 0;
            this.btnCheckAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckAppointment.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.btnCheckAppointment.ForeColor = System.Drawing.Color.White;
            this.btnCheckAppointment.Location = new System.Drawing.Point(190, 475);
            this.btnCheckAppointment.Name = "btnCheckAppointment";
            this.btnCheckAppointment.Size = new System.Drawing.Size(390, 155);
            this.btnCheckAppointment.TabIndex = 4;
            this.btnCheckAppointment.Text = "예약 조회";
            this.btnCheckAppointment.UseVisualStyleBackColor = false;
            this.btnCheckAppointment.Click += new System.EventHandler(this.BtnCheckAppointment_Click);
            this.btnCheckAppointment.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnCheckAppointment.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            //
            // btnRegister
            //
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(620, 475);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(390, 155);
            this.btnRegister.TabIndex = 5;
            this.btnRegister.Text = "신규 환자 등록";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            this.btnRegister.MouseEnter += new System.EventHandler(this.Btn_MouseEnter);
            this.btnRegister.MouseLeave += new System.EventHandler(this.Btn_MouseLeave);
            //
            // lblInfo
            //
            this.lblInfo.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.Location = new System.Drawing.Point(125, 700);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(950, 40);
            this.lblInfo.TabIndex = 6;
            this.lblInfo.Text = "문의사항이 있으시면 안내 데스크로 문의해주세요";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnAdmin
            //
            this.btnAdmin.BackColor = System.Drawing.Color.LightGray;
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.Location = new System.Drawing.Point(1060, 805);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(120, 48);
            this.btnAdmin.TabIndex = 7;
            this.btnAdmin.Text = "관리자";
            this.btnAdmin.UseVisualStyleBackColor = false;
            this.btnAdmin.Click += new System.EventHandler(this.BtnAdmin_Click);
            //
            // KioskMainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 900);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnCheckAppointment);
            this.Controls.Add(this.btnPayment);
            this.Controls.Add(this.btnAppointment);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "KioskMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "병원 키오스크 시스템";
            this.ResumeLayout(false);
        }
    }
}
