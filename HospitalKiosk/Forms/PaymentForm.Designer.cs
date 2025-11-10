namespace HospitalKiosk.Forms
{
    partial class PaymentForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblPatientSearch;
        private System.Windows.Forms.TextBox txtPatientSearch;
        private System.Windows.Forms.Button btnSearchPatient;
        private System.Windows.Forms.Label lblPatientInfo;
        private System.Windows.Forms.Label lblAppointments;
        private System.Windows.Forms.ListView lvAppointments;
        private System.Windows.Forms.Label lblTotalAmountLabel;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblAmountLabel;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label lblPaymentMethodLabel;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnCancel;
        private HospitalKiosk.Controls.NumericKeypadControl numericKeypad;

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
            this.lblAppointments = new System.Windows.Forms.Label();
            this.lvAppointments = new System.Windows.Forms.ListView();
            this.lblTotalAmountLabel = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblAmountLabel = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblPaymentMethodLabel = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numericKeypad = new HospitalKiosk.Controls.NumericKeypadControl();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(760, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "진료비 수납";
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
            this.txtPatientSearch.ReadOnly = true;
            this.txtPatientSearch.Size = new System.Drawing.Size(300, 27);
            this.txtPatientSearch.TabIndex = 2;
            this.txtPatientSearch.Enter += new System.EventHandler(this.TxtPatientSearch_Enter);
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
            this.lblPatientInfo.Size = new System.Drawing.Size(760, 30);
            this.lblPatientInfo.TabIndex = 4;
            this.lblPatientInfo.Text = "환자를 검색해주세요";
            //
            // lblAppointments
            //
            this.lblAppointments.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblAppointments.Location = new System.Drawing.Point(20, 160);
            this.lblAppointments.Name = "lblAppointments";
            this.lblAppointments.Size = new System.Drawing.Size(150, 30);
            this.lblAppointments.TabIndex = 5;
            this.lblAppointments.Text = "진료 예약 목록:";
            //
            // lvAppointments
            //
            this.lvAppointments.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lvAppointments.FullRowSelect = true;
            this.lvAppointments.GridLines = true;
            this.lvAppointments.Location = new System.Drawing.Point(20, 190);
            this.lvAppointments.Name = "lvAppointments";
            this.lvAppointments.Size = new System.Drawing.Size(740, 150);
            this.lvAppointments.TabIndex = 6;
            this.lvAppointments.UseCompatibleStateImageBehavior = false;
            this.lvAppointments.View = System.Windows.Forms.View.Details;
            this.lvAppointments.SelectedIndexChanged += new System.EventHandler(this.LvAppointments_SelectedIndexChanged);
            //
            // lblTotalAmountLabel
            //
            this.lblTotalAmountLabel.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblTotalAmountLabel.Location = new System.Drawing.Point(20, 360);
            this.lblTotalAmountLabel.Name = "lblTotalAmountLabel";
            this.lblTotalAmountLabel.Size = new System.Drawing.Size(150, 30);
            this.lblTotalAmountLabel.TabIndex = 7;
            this.lblTotalAmountLabel.Text = "진료비:";
            this.lblTotalAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblTotalAmount
            //
            this.lblTotalAmount.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.lblTotalAmount.Location = new System.Drawing.Point(180, 360);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(200, 30);
            this.lblTotalAmount.TabIndex = 8;
            this.lblTotalAmount.Text = "0 원";
            //
            // lblAmountLabel
            //
            this.lblAmountLabel.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblAmountLabel.Location = new System.Drawing.Point(20, 410);
            this.lblAmountLabel.Name = "lblAmountLabel";
            this.lblAmountLabel.Size = new System.Drawing.Size(150, 30);
            this.lblAmountLabel.TabIndex = 9;
            this.lblAmountLabel.Text = "납부 금액:";
            this.lblAmountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // txtAmount
            //
            this.txtAmount.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.txtAmount.Location = new System.Drawing.Point(180, 410);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.Size = new System.Drawing.Size(200, 29);
            this.txtAmount.TabIndex = 10;
            this.txtAmount.Enter += new System.EventHandler(this.TxtAmount_Enter);
            //
            // lblPaymentMethodLabel
            //
            this.lblPaymentMethodLabel.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.lblPaymentMethodLabel.Location = new System.Drawing.Point(20, 460);
            this.lblPaymentMethodLabel.Name = "lblPaymentMethodLabel";
            this.lblPaymentMethodLabel.Size = new System.Drawing.Size(150, 30);
            this.lblPaymentMethodLabel.TabIndex = 11;
            this.lblPaymentMethodLabel.Text = "결제 방법:";
            this.lblPaymentMethodLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // cmbPaymentMethod
            //
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.Enabled = false;
            this.cmbPaymentMethod.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.Items.AddRange(new object[] { "현금", "카드", "계좌이체" });
            this.cmbPaymentMethod.Location = new System.Drawing.Point(180, 460);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(200, 28);
            this.cmbPaymentMethod.TabIndex = 12;
            //
            // btnPay
            //
            this.btnPay.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnPay.Enabled = false;
            this.btnPay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPay.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.btnPay.ForeColor = System.Drawing.Color.White;
            this.btnPay.Location = new System.Drawing.Point(480, 520);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(150, 45);
            this.btnPay.TabIndex = 13;
            this.btnPay.Text = "수납 처리";
            this.btnPay.UseVisualStyleBackColor = false;
            this.btnPay.Click += new System.EventHandler(this.BtnPay_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(640, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 45);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // numericKeypad
            //
            this.numericKeypad.BackColor = System.Drawing.Color.LightGray;
            this.numericKeypad.Location = new System.Drawing.Point(820, 80);
            this.numericKeypad.Name = "numericKeypad";
            this.numericKeypad.Size = new System.Drawing.Size(324, 440);
            this.numericKeypad.TabIndex = 15;
            this.numericKeypad.TargetTextBox = null;
            //
            // PaymentForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.numericKeypad);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.cmbPaymentMethod);
            this.Controls.Add(this.lblPaymentMethodLabel);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.lblAmountLabel);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblTotalAmountLabel);
            this.Controls.Add(this.lvAppointments);
            this.Controls.Add(this.lblAppointments);
            this.Controls.Add(this.lblPatientInfo);
            this.Controls.Add(this.btnSearchPatient);
            this.Controls.Add(this.txtPatientSearch);
            this.Controls.Add(this.lblPatientSearch);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "PaymentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "진료비 수납";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
