namespace HospitalKiosk.Forms
{
    partial class AppointmentCheckForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpPatientSearch = new System.Windows.Forms.GroupBox();
            this.btnSearchPatient = new System.Windows.Forms.Button();
            this.txtPatientSearch = new System.Windows.Forms.TextBox();
            this.lblSearchHint = new System.Windows.Forms.Label();
            this.lblPatientInfo = new System.Windows.Forms.Label();
            this.grpAppointmentList = new System.Windows.Forms.GroupBox();
            this.lvAppointments = new System.Windows.Forms.ListView();
            this.grpAppointmentDetails = new System.Windows.Forms.GroupBox();
            this.lblDetailNotes = new System.Windows.Forms.Label();
            this.lblDetailNotesTitle = new System.Windows.Forms.Label();
            this.lblDetailReason = new System.Windows.Forms.Label();
            this.lblDetailReasonTitle = new System.Windows.Forms.Label();
            this.lblDetailStatus = new System.Windows.Forms.Label();
            this.lblDetailStatusTitle = new System.Windows.Forms.Label();
            this.lblDetailDoctor = new System.Windows.Forms.Label();
            this.lblDetailDoctorTitle = new System.Windows.Forms.Label();
            this.lblDetailDateTime = new System.Windows.Forms.Label();
            this.lblDetailDateTimeTitle = new System.Windows.Forms.Label();
            this.lblDetailAppointmentNumber = new System.Windows.Forms.Label();
            this.lblDetailAppointmentNumberTitle = new System.Windows.Forms.Label();
            this.btnCancelAppointment = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.numericKeypad = new HospitalKiosk.Controls.NumericKeypadControl();
            this.grpPatientSearch.SuspendLayout();
            this.grpAppointmentList.SuspendLayout();
            this.grpAppointmentDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(118, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "예약 조회";
            // 
            // grpPatientSearch
            //
            this.grpPatientSearch.Controls.Add(this.lblSearchHint);
            this.grpPatientSearch.Controls.Add(this.txtPatientSearch);
            this.grpPatientSearch.Controls.Add(this.btnSearchPatient);
            this.grpPatientSearch.Controls.Add(this.numericKeypad);
            this.grpPatientSearch.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpPatientSearch.Location = new System.Drawing.Point(36, 80);
            this.grpPatientSearch.Name = "grpPatientSearch";
            this.grpPatientSearch.Size = new System.Drawing.Size(928, 550);
            this.grpPatientSearch.TabIndex = 1;
            this.grpPatientSearch.TabStop = false;
            this.grpPatientSearch.Text = "환자 검색";
            // 
            // btnSearchPatient
            // 
            this.btnSearchPatient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSearchPatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchPatient.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchPatient.ForeColor = System.Drawing.Color.White;
            this.btnSearchPatient.Location = new System.Drawing.Point(750, 35);
            this.btnSearchPatient.Name = "btnSearchPatient";
            this.btnSearchPatient.Size = new System.Drawing.Size(150, 40);
            this.btnSearchPatient.TabIndex = 2;
            this.btnSearchPatient.Text = "검색";
            this.btnSearchPatient.UseVisualStyleBackColor = false;
            this.btnSearchPatient.Click += new System.EventHandler(this.BtnSearchPatient_Click);
            // 
            // txtPatientSearch
            // 
            this.txtPatientSearch.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPatientSearch.Location = new System.Drawing.Point(330, 40);
            this.txtPatientSearch.Name = "txtPatientSearch";
            this.txtPatientSearch.ReadOnly = true;
            this.txtPatientSearch.Size = new System.Drawing.Size(400, 29);
            this.txtPatientSearch.TabIndex = 1;
            this.txtPatientSearch.Enter += new System.EventHandler(this.TxtPatientSearch_Enter);
            this.txtPatientSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPatientSearch_KeyPress);
            // 
            // lblSearchHint
            // 
            this.lblSearchHint.AutoSize = true;
            this.lblSearchHint.Location = new System.Drawing.Point(30, 43);
            this.lblSearchHint.Name = "lblSearchHint";
            this.lblSearchHint.Size = new System.Drawing.Size(272, 20);
            this.lblSearchHint.TabIndex = 0;
            this.lblSearchHint.Text = "환자 이름 또는 전화번호를 입력하세요:";
            //
            // lblPatientInfo
            //
            this.lblPatientInfo.AutoSize = true;
            this.lblPatientInfo.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPatientInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblPatientInfo.Location = new System.Drawing.Point(50, 640);
            this.lblPatientInfo.Name = "lblPatientInfo";
            this.lblPatientInfo.Size = new System.Drawing.Size(161, 20);
            this.lblPatientInfo.TabIndex = 2;
            this.lblPatientInfo.Text = "환자를 검색해주세요...";
            // 
            // grpAppointmentList
            // 
            this.grpAppointmentList.Controls.Add(this.lvAppointments);
            this.grpAppointmentList.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpAppointmentList.Location = new System.Drawing.Point(36, 670);
            this.grpAppointmentList.Name = "grpAppointmentList";
            this.grpAppointmentList.Size = new System.Drawing.Size(928, 170);
            this.grpAppointmentList.TabIndex = 3;
            this.grpAppointmentList.TabStop = false;
            this.grpAppointmentList.Text = "예약 목록";
            // 
            // lvAppointments
            // 
            this.lvAppointments.FullRowSelect = true;
            this.lvAppointments.GridLines = true;
            this.lvAppointments.HideSelection = false;
            this.lvAppointments.Location = new System.Drawing.Point(20, 26);
            this.lvAppointments.MultiSelect = false;
            this.lvAppointments.Name = "lvAppointments";
            this.lvAppointments.Size = new System.Drawing.Size(888, 135);
            this.lvAppointments.TabIndex = 0;
            this.lvAppointments.UseCompatibleStateImageBehavior = false;
            this.lvAppointments.View = System.Windows.Forms.View.Details;
            this.lvAppointments.SelectedIndexChanged += new System.EventHandler(this.LvAppointments_SelectedIndexChanged);
            // 
            // grpAppointmentDetails
            // 
            this.grpAppointmentDetails.Controls.Add(this.lblDetailNotes);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailNotesTitle);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailReason);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailReasonTitle);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailStatus);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailStatusTitle);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailDoctor);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailDoctorTitle);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailDateTime);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailDateTimeTitle);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailAppointmentNumber);
            this.grpAppointmentDetails.Controls.Add(this.lblDetailAppointmentNumberTitle);
            this.grpAppointmentDetails.Controls.Add(this.btnCancelAppointment);
            this.grpAppointmentDetails.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grpAppointmentDetails.Location = new System.Drawing.Point(36, 850);
            this.grpAppointmentDetails.Name = "grpAppointmentDetails";
            this.grpAppointmentDetails.Size = new System.Drawing.Size(928, 145);
            this.grpAppointmentDetails.TabIndex = 4;
            this.grpAppointmentDetails.TabStop = false;
            this.grpAppointmentDetails.Text = "예약 상세";
            this.grpAppointmentDetails.Visible = false;
            // 
            // lblDetailNotes
            // 
            this.lblDetailNotes.AutoSize = true;
            this.lblDetailNotes.Location = new System.Drawing.Point(580, 110);
            this.lblDetailNotes.Name = "lblDetailNotes";
            this.lblDetailNotes.Size = new System.Drawing.Size(15, 20);
            this.lblDetailNotes.TabIndex = 11;
            this.lblDetailNotes.Text = "-";
            // 
            // lblDetailNotesTitle
            // 
            this.lblDetailNotesTitle.AutoSize = true;
            this.lblDetailNotesTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailNotesTitle.Location = new System.Drawing.Point(480, 110);
            this.lblDetailNotesTitle.Name = "lblDetailNotesTitle";
            this.lblDetailNotesTitle.Size = new System.Drawing.Size(43, 20);
            this.lblDetailNotesTitle.TabIndex = 10;
            this.lblDetailNotesTitle.Text = "메모:";
            // 
            // lblDetailReason
            // 
            this.lblDetailReason.AutoSize = true;
            this.lblDetailReason.Location = new System.Drawing.Point(580, 70);
            this.lblDetailReason.Name = "lblDetailReason";
            this.lblDetailReason.Size = new System.Drawing.Size(15, 20);
            this.lblDetailReason.TabIndex = 9;
            this.lblDetailReason.Text = "-";
            // 
            // lblDetailReasonTitle
            // 
            this.lblDetailReasonTitle.AutoSize = true;
            this.lblDetailReasonTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailReasonTitle.Location = new System.Drawing.Point(480, 70);
            this.lblDetailReasonTitle.Name = "lblDetailReasonTitle";
            this.lblDetailReasonTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailReasonTitle.TabIndex = 8;
            this.lblDetailReasonTitle.Text = "진료사유:";
            // 
            // lblDetailStatus
            // 
            this.lblDetailStatus.AutoSize = true;
            this.lblDetailStatus.Location = new System.Drawing.Point(580, 30);
            this.lblDetailStatus.Name = "lblDetailStatus";
            this.lblDetailStatus.Size = new System.Drawing.Size(15, 20);
            this.lblDetailStatus.TabIndex = 7;
            this.lblDetailStatus.Text = "-";
            // 
            // lblDetailStatusTitle
            // 
            this.lblDetailStatusTitle.AutoSize = true;
            this.lblDetailStatusTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailStatusTitle.Location = new System.Drawing.Point(480, 30);
            this.lblDetailStatusTitle.Name = "lblDetailStatusTitle";
            this.lblDetailStatusTitle.Size = new System.Drawing.Size(43, 20);
            this.lblDetailStatusTitle.TabIndex = 6;
            this.lblDetailStatusTitle.Text = "상태:";
            // 
            // lblDetailDoctor
            // 
            this.lblDetailDoctor.AutoSize = true;
            this.lblDetailDoctor.Location = new System.Drawing.Point(160, 110);
            this.lblDetailDoctor.Name = "lblDetailDoctor";
            this.lblDetailDoctor.Size = new System.Drawing.Size(15, 20);
            this.lblDetailDoctor.TabIndex = 5;
            this.lblDetailDoctor.Text = "-";
            // 
            // lblDetailDoctorTitle
            // 
            this.lblDetailDoctorTitle.AutoSize = true;
            this.lblDetailDoctorTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDoctorTitle.Location = new System.Drawing.Point(30, 110);
            this.lblDetailDoctorTitle.Name = "lblDetailDoctorTitle";
            this.lblDetailDoctorTitle.Size = new System.Drawing.Size(110, 20);
            this.lblDetailDoctorTitle.TabIndex = 4;
            this.lblDetailDoctorTitle.Text = "담당의/진료과:";
            // 
            // lblDetailDateTime
            // 
            this.lblDetailDateTime.AutoSize = true;
            this.lblDetailDateTime.Location = new System.Drawing.Point(160, 70);
            this.lblDetailDateTime.Name = "lblDetailDateTime";
            this.lblDetailDateTime.Size = new System.Drawing.Size(15, 20);
            this.lblDetailDateTime.TabIndex = 3;
            this.lblDetailDateTime.Text = "-";
            // 
            // lblDetailDateTimeTitle
            // 
            this.lblDetailDateTimeTitle.AutoSize = true;
            this.lblDetailDateTimeTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailDateTimeTitle.Location = new System.Drawing.Point(30, 70);
            this.lblDetailDateTimeTitle.Name = "lblDetailDateTimeTitle";
            this.lblDetailDateTimeTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailDateTimeTitle.TabIndex = 2;
            this.lblDetailDateTimeTitle.Text = "예약일시:";
            // 
            // lblDetailAppointmentNumber
            // 
            this.lblDetailAppointmentNumber.AutoSize = true;
            this.lblDetailAppointmentNumber.Location = new System.Drawing.Point(160, 30);
            this.lblDetailAppointmentNumber.Name = "lblDetailAppointmentNumber";
            this.lblDetailAppointmentNumber.Size = new System.Drawing.Size(15, 20);
            this.lblDetailAppointmentNumber.TabIndex = 1;
            this.lblDetailAppointmentNumber.Text = "-";
            // 
            // lblDetailAppointmentNumberTitle
            // 
            this.lblDetailAppointmentNumberTitle.AutoSize = true;
            this.lblDetailAppointmentNumberTitle.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDetailAppointmentNumberTitle.Location = new System.Drawing.Point(30, 30);
            this.lblDetailAppointmentNumberTitle.Name = "lblDetailAppointmentNumberTitle";
            this.lblDetailAppointmentNumberTitle.Size = new System.Drawing.Size(73, 20);
            this.lblDetailAppointmentNumberTitle.TabIndex = 0;
            this.lblDetailAppointmentNumberTitle.Text = "예약번호:";
            // 
            // btnCancelAppointment
            // 
            this.btnCancelAppointment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnCancelAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelAppointment.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancelAppointment.ForeColor = System.Drawing.Color.White;
            this.btnCancelAppointment.Location = new System.Drawing.Point(750, 100);
            this.btnCancelAppointment.Name = "btnCancelAppointment";
            this.btnCancelAppointment.Size = new System.Drawing.Size(150, 40);
            this.btnCancelAppointment.TabIndex = 12;
            this.btnCancelAppointment.Text = "예약 취소";
            this.btnCancelAppointment.UseVisualStyleBackColor = false;
            this.btnCancelAppointment.Click += new System.EventHandler(this.BtnCancelAppointment_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(814, 1005);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 50);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // numericKeypad
            // 
            this.numericKeypad.BackColor = System.Drawing.Color.LightGray;
            this.numericKeypad.Location = new System.Drawing.Point(330, 80);
            this.numericKeypad.Name = "numericKeypad";
            this.numericKeypad.Size = new System.Drawing.Size(324, 440);
            this.numericKeypad.TabIndex = 3;
            this.numericKeypad.TargetTextBox = null;
            // 
            // AppointmentCheckForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1000, 1020);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpAppointmentDetails);
            this.Controls.Add(this.grpAppointmentList);
            this.Controls.Add(this.lblPatientInfo);
            this.Controls.Add(this.grpPatientSearch);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AppointmentCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "예약 조회";
            this.grpPatientSearch.ResumeLayout(false);
            this.grpPatientSearch.PerformLayout();
            this.grpAppointmentList.ResumeLayout(false);
            this.grpAppointmentDetails.ResumeLayout(false);
            this.grpAppointmentDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpPatientSearch;
        private System.Windows.Forms.Button btnSearchPatient;
        private System.Windows.Forms.TextBox txtPatientSearch;
        private System.Windows.Forms.Label lblSearchHint;
        private System.Windows.Forms.Label lblPatientInfo;
        private System.Windows.Forms.GroupBox grpAppointmentList;
        private System.Windows.Forms.ListView lvAppointments;
        private System.Windows.Forms.GroupBox grpAppointmentDetails;
        private System.Windows.Forms.Label lblDetailAppointmentNumber;
        private System.Windows.Forms.Label lblDetailAppointmentNumberTitle;
        private System.Windows.Forms.Label lblDetailDateTime;
        private System.Windows.Forms.Label lblDetailDateTimeTitle;
        private System.Windows.Forms.Label lblDetailDoctor;
        private System.Windows.Forms.Label lblDetailDoctorTitle;
        private System.Windows.Forms.Label lblDetailStatus;
        private System.Windows.Forms.Label lblDetailStatusTitle;
        private System.Windows.Forms.Label lblDetailReason;
        private System.Windows.Forms.Label lblDetailReasonTitle;
        private System.Windows.Forms.Label lblDetailNotes;
        private System.Windows.Forms.Label lblDetailNotesTitle;
        private System.Windows.Forms.Button btnCancelAppointment;
        private System.Windows.Forms.Button btnClose;
        private HospitalKiosk.Controls.NumericKeypadControl numericKeypad;
    }
}
