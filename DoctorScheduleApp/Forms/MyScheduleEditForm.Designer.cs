namespace DoctorScheduleApp.Forms
{
    partial class MyScheduleEditForm
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
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.lblScheduleType = new System.Windows.Forms.Label();
            this.cmbScheduleType = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.chkRecurring = new System.Windows.Forms.CheckBox();
            this.lblRecurringDays = new System.Windows.Forms.Label();
            this.clbDaysOfWeek = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            //
            // lblDate
            //
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDate.Location = new System.Drawing.Point(30, 30);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(42, 19);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "날짜:";
            //
            // dtpDate
            //
            this.dtpDate.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(150, 27);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(250, 25);
            this.dtpDate.TabIndex = 0;
            //
            // lblStartTime
            //
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStartTime.Location = new System.Drawing.Point(30, 70);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(74, 19);
            this.lblStartTime.TabIndex = 2;
            this.lblStartTime.Text = "시작 시간:";
            //
            // dtpStartTime
            //
            this.dtpStartTime.CustomFormat = "HH:mm";
            this.dtpStartTime.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(150, 67);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(250, 25);
            this.dtpStartTime.TabIndex = 1;
            //
            // lblEndTime
            //
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblEndTime.Location = new System.Drawing.Point(30, 110);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(74, 19);
            this.lblEndTime.TabIndex = 4;
            this.lblEndTime.Text = "종료 시간:";
            //
            // dtpEndTime
            //
            this.dtpEndTime.CustomFormat = "HH:mm";
            this.dtpEndTime.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(150, 107);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(250, 25);
            this.dtpEndTime.TabIndex = 2;
            //
            // lblScheduleType
            //
            this.lblScheduleType.AutoSize = true;
            this.lblScheduleType.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblScheduleType.Location = new System.Drawing.Point(30, 150);
            this.lblScheduleType.Name = "lblScheduleType";
            this.lblScheduleType.Size = new System.Drawing.Size(74, 19);
            this.lblScheduleType.TabIndex = 6;
            this.lblScheduleType.Text = "일정 유형:";
            //
            // cmbScheduleType
            //
            this.cmbScheduleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScheduleType.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbScheduleType.FormattingEnabled = true;
            this.cmbScheduleType.Location = new System.Drawing.Point(150, 147);
            this.cmbScheduleType.Name = "cmbScheduleType";
            this.cmbScheduleType.Size = new System.Drawing.Size(250, 25);
            this.cmbScheduleType.TabIndex = 3;
            //
            // lblNotes
            //
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNotes.Location = new System.Drawing.Point(30, 190);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(42, 19);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Text = "메모:";
            //
            // txtNotes
            //
            this.txtNotes.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtNotes.Location = new System.Drawing.Point(150, 187);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNotes.Size = new System.Drawing.Size(250, 60);
            this.txtNotes.TabIndex = 4;
            //
            // chkRecurring
            //
            this.chkRecurring.AutoSize = true;
            this.chkRecurring.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkRecurring.Location = new System.Drawing.Point(150, 260);
            this.chkRecurring.Name = "chkRecurring";
            this.chkRecurring.Size = new System.Drawing.Size(122, 23);
            this.chkRecurring.TabIndex = 5;
            this.chkRecurring.Text = "반복 일정 설정";
            this.chkRecurring.UseVisualStyleBackColor = true;
            this.chkRecurring.CheckedChanged += new System.EventHandler(this.ChkRecurring_CheckedChanged);
            //
            // lblRecurringDays
            //
            this.lblRecurringDays.AutoSize = true;
            this.lblRecurringDays.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRecurringDays.Location = new System.Drawing.Point(30, 295);
            this.lblRecurringDays.Name = "lblRecurringDays";
            this.lblRecurringDays.Size = new System.Drawing.Size(74, 19);
            this.lblRecurringDays.TabIndex = 10;
            this.lblRecurringDays.Text = "반복 요일:";
            this.lblRecurringDays.Visible = false;
            //
            // clbDaysOfWeek
            //
            this.clbDaysOfWeek.CheckOnClick = true;
            this.clbDaysOfWeek.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.clbDaysOfWeek.FormattingEnabled = true;
            this.clbDaysOfWeek.Items.AddRange(new object[] {
            "일요일",
            "월요일",
            "화요일",
            "수요일",
            "목요일",
            "금요일",
            "토요일"});
            this.clbDaysOfWeek.Location = new System.Drawing.Point(150, 295);
            this.clbDaysOfWeek.MultiColumn = true;
            this.clbDaysOfWeek.Name = "clbDaysOfWeek";
            this.clbDaysOfWeek.Size = new System.Drawing.Size(250, 40);
            this.clbDaysOfWeek.TabIndex = 6;
            this.clbDaysOfWeek.Visible = false;
            //
            // btnSave
            //
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(150, 355);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(280, 355);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // panelMain
            //
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.lblDate);
            this.panelMain.Controls.Add(this.btnCancel);
            this.panelMain.Controls.Add(this.dtpDate);
            this.panelMain.Controls.Add(this.btnSave);
            this.panelMain.Controls.Add(this.lblStartTime);
            this.panelMain.Controls.Add(this.txtNotes);
            this.panelMain.Controls.Add(this.dtpStartTime);
            this.panelMain.Controls.Add(this.lblNotes);
            this.panelMain.Controls.Add(this.lblEndTime);
            this.panelMain.Controls.Add(this.cmbScheduleType);
            this.panelMain.Controls.Add(this.dtpEndTime);
            this.panelMain.Controls.Add(this.lblScheduleType);
            this.panelMain.Controls.Add(this.chkRecurring);
            this.panelMain.Controls.Add(this.lblRecurringDays);
            this.panelMain.Controls.Add(this.clbDaysOfWeek);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(450, 420);
            this.panelMain.TabIndex = 12;
            //
            // MyScheduleEditForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 420);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MyScheduleEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "일정 편집";
            this.Load += new System.EventHandler(this.MyScheduleEditForm_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label lblScheduleType;
        private System.Windows.Forms.ComboBox cmbScheduleType;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.CheckBox chkRecurring;
        private System.Windows.Forms.Label lblRecurringDays;
        private System.Windows.Forms.CheckedListBox clbDaysOfWeek;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panelMain;
    }
}
