namespace HospitalKiosk.Forms
{
    partial class DoctorScheduleManagementForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDoctor;
        private System.Windows.Forms.ComboBox cmbDoctor;
        private System.Windows.Forms.MonthCalendar monthCalendar;
        private System.Windows.Forms.Label lblCurrentMonth;
        private System.Windows.Forms.Button btnPrevMonth;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.DataGridView dgvSchedules;
        private System.Windows.Forms.Label lblScheduleCount;
        private System.Windows.Forms.Button btnAddSchedule;
        private System.Windows.Forms.Button btnEditSchedule;
        private System.Windows.Forms.Button btnDeleteSchedule;
        private System.Windows.Forms.Button btnClose;

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
            this.lblDoctor = new System.Windows.Forms.Label();
            this.cmbDoctor = new System.Windows.Forms.ComboBox();
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.lblCurrentMonth = new System.Windows.Forms.Label();
            this.btnPrevMonth = new System.Windows.Forms.Button();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.dgvSchedules = new System.Windows.Forms.DataGridView();
            this.lblScheduleCount = new System.Windows.Forms.Label();
            this.btnAddSchedule = new System.Windows.Forms.Button();
            this.btnEditSchedule = new System.Windows.Forms.Button();
            this.btnDeleteSchedule = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(201, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "의사 일정 관리";
            //
            // lblDoctor
            //
            this.lblDoctor.AutoSize = true;
            this.lblDoctor.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lblDoctor.Location = new System.Drawing.Point(30, 80);
            this.lblDoctor.Name = "lblDoctor";
            this.lblDoctor.Size = new System.Drawing.Size(39, 19);
            this.lblDoctor.TabIndex = 1;
            this.lblDoctor.Text = "의사:";
            //
            // cmbDoctor
            //
            this.cmbDoctor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDoctor.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.cmbDoctor.FormattingEnabled = true;
            this.cmbDoctor.Location = new System.Drawing.Point(80, 77);
            this.cmbDoctor.Name = "cmbDoctor";
            this.cmbDoctor.Size = new System.Drawing.Size(250, 25);
            this.cmbDoctor.TabIndex = 2;
            this.cmbDoctor.SelectedIndexChanged += new System.EventHandler(this.CmbDoctor_SelectedIndexChanged);
            //
            // lblCurrentMonth
            //
            this.lblCurrentMonth.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrentMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.lblCurrentMonth.Location = new System.Drawing.Point(30, 130);
            this.lblCurrentMonth.Name = "lblCurrentMonth";
            this.lblCurrentMonth.Size = new System.Drawing.Size(200, 30);
            this.lblCurrentMonth.TabIndex = 3;
            this.lblCurrentMonth.Text = "2025년 01월";
            this.lblCurrentMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnPrevMonth
            //
            this.btnPrevMonth.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrevMonth.Location = new System.Drawing.Point(240, 130);
            this.btnPrevMonth.Name = "btnPrevMonth";
            this.btnPrevMonth.Size = new System.Drawing.Size(70, 30);
            this.btnPrevMonth.TabIndex = 4;
            this.btnPrevMonth.Text = "◀ 이전";
            this.btnPrevMonth.UseVisualStyleBackColor = true;
            this.btnPrevMonth.Click += new System.EventHandler(this.BtnPrevMonth_Click);
            //
            // btnNextMonth
            //
            this.btnNextMonth.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnNextMonth.Location = new System.Drawing.Point(320, 130);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(70, 30);
            this.btnNextMonth.TabIndex = 5;
            this.btnNextMonth.Text = "다음 ▶";
            this.btnNextMonth.UseVisualStyleBackColor = true;
            this.btnNextMonth.Click += new System.EventHandler(this.BtnNextMonth_Click);
            //
            // btnToday
            //
            this.btnToday.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnToday.Location = new System.Drawing.Point(400, 130);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(70, 30);
            this.btnToday.TabIndex = 6;
            this.btnToday.Text = "오늘";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.BtnToday_Click);
            //
            // monthCalendar
            //
            this.monthCalendar.Location = new System.Drawing.Point(30, 180);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 7;
            this.monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.MonthCalendar_DateChanged);
            //
            // dgvSchedules
            //
            this.dgvSchedules.AllowUserToAddRows = false;
            this.dgvSchedules.AllowUserToDeleteRows = false;
            this.dgvSchedules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSchedules.BackgroundColor = System.Drawing.Color.White;
            this.dgvSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedules.Location = new System.Drawing.Point(320, 180);
            this.dgvSchedules.MultiSelect = false;
            this.dgvSchedules.Name = "dgvSchedules";
            this.dgvSchedules.ReadOnly = true;
            this.dgvSchedules.RowHeadersWidth = 51;
            this.dgvSchedules.RowTemplate.Height = 23;
            this.dgvSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSchedules.Size = new System.Drawing.Size(650, 350);
            this.dgvSchedules.TabIndex = 8;
            this.dgvSchedules.Columns.Add("ScheduleId", "ID");
            this.dgvSchedules.Columns.Add("Date", "날짜");
            this.dgvSchedules.Columns.Add("StartTime", "시작");
            this.dgvSchedules.Columns.Add("EndTime", "종료");
            this.dgvSchedules.Columns.Add("Type", "유형");
            this.dgvSchedules.Columns.Add("Title", "제목");
            this.dgvSchedules.Columns.Add("Description", "설명");
            this.dgvSchedules.Columns[0].Visible = false;
            this.dgvSchedules.Columns[1].Width = 100;
            this.dgvSchedules.Columns[2].Width = 60;
            this.dgvSchedules.Columns[3].Width = 60;
            this.dgvSchedules.Columns[4].Width = 70;
            //
            // lblScheduleCount
            //
            this.lblScheduleCount.AutoSize = true;
            this.lblScheduleCount.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblScheduleCount.Location = new System.Drawing.Point(320, 540);
            this.lblScheduleCount.Name = "lblScheduleCount";
            this.lblScheduleCount.Size = new System.Drawing.Size(56, 19);
            this.lblScheduleCount.TabIndex = 9;
            this.lblScheduleCount.Text = "총 0건";
            //
            // btnAddSchedule
            //
            this.btnAddSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAddSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSchedule.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddSchedule.ForeColor = System.Drawing.Color.White;
            this.btnAddSchedule.Location = new System.Drawing.Point(580, 560);
            this.btnAddSchedule.Name = "btnAddSchedule";
            this.btnAddSchedule.Size = new System.Drawing.Size(90, 40);
            this.btnAddSchedule.TabIndex = 10;
            this.btnAddSchedule.Text = "추가";
            this.btnAddSchedule.UseVisualStyleBackColor = false;
            this.btnAddSchedule.Click += new System.EventHandler(this.BtnAddSchedule_Click);
            //
            // btnEditSchedule
            //
            this.btnEditSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnEditSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSchedule.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnEditSchedule.ForeColor = System.Drawing.Color.White;
            this.btnEditSchedule.Location = new System.Drawing.Point(690, 560);
            this.btnEditSchedule.Name = "btnEditSchedule";
            this.btnEditSchedule.Size = new System.Drawing.Size(90, 40);
            this.btnEditSchedule.TabIndex = 11;
            this.btnEditSchedule.Text = "편집";
            this.btnEditSchedule.UseVisualStyleBackColor = false;
            this.btnEditSchedule.Click += new System.EventHandler(this.BtnEditSchedule_Click);
            //
            // btnDeleteSchedule
            //
            this.btnDeleteSchedule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDeleteSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSchedule.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnDeleteSchedule.ForeColor = System.Drawing.Color.White;
            this.btnDeleteSchedule.Location = new System.Drawing.Point(800, 560);
            this.btnDeleteSchedule.Name = "btnDeleteSchedule";
            this.btnDeleteSchedule.Size = new System.Drawing.Size(80, 40);
            this.btnDeleteSchedule.TabIndex = 12;
            this.btnDeleteSchedule.Text = "삭제";
            this.btnDeleteSchedule.UseVisualStyleBackColor = false;
            this.btnDeleteSchedule.Click += new System.EventHandler(this.BtnDeleteSchedule_Click);
            //
            // btnClose
            //
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(890, 560);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 40);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            //
            // DoctorScheduleManagementForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDeleteSchedule);
            this.Controls.Add(this.btnEditSchedule);
            this.Controls.Add(this.btnAddSchedule);
            this.Controls.Add(this.lblScheduleCount);
            this.Controls.Add(this.dgvSchedules);
            this.Controls.Add(this.monthCalendar);
            this.Controls.Add(this.btnToday);
            this.Controls.Add(this.btnNextMonth);
            this.Controls.Add(this.btnPrevMonth);
            this.Controls.Add(this.lblCurrentMonth);
            this.Controls.Add(this.cmbDoctor);
            this.Controls.Add(this.lblDoctor);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DoctorScheduleManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "의사 일정 관리";
            this.Load += new System.EventHandler(this.DoctorScheduleManagementForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
