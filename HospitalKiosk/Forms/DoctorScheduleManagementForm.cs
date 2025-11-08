using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Data;
using HospitalKiosk.Models;
using HospitalKiosk.Services;
using System.Collections.Generic;

namespace HospitalKiosk.Forms
{
    public partial class DoctorScheduleManagementForm : Form
    {
        private readonly DoctorScheduleService _scheduleService;
        private readonly DoctorRepository _doctorRepo;
        private DateTime _currentDate;
        private Doctor _selectedDoctor;

        public DoctorScheduleManagementForm()
        {
            InitializeComponent();
            _scheduleService = new DoctorScheduleService();
            _doctorRepo = new DoctorRepository();
            _currentDate = DateTime.Today;
        }

        private void DoctorScheduleManagementForm_Load(object sender, EventArgs e)
        {
            LoadDoctors();
            UpdateCalendar();
        }

        private void LoadDoctors()
        {
            try
            {
                var doctors = _doctorRepo.GetAll();
                cmbDoctor.Items.Clear();

                foreach (var doctor in doctors)
                {
                    if (doctor.IsActive)
                    {
                        cmbDoctor.Items.Add($"{doctor.DoctorName} ({doctor.DoctorCode})");
                        cmbDoctor.Tag = cmbDoctor.Tag ?? new List<Doctor>();
                        ((List<Doctor>)cmbDoctor.Tag).Add(doctor);
                    }
                }

                if (cmbDoctor.Items.Count > 0)
                {
                    cmbDoctor.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("의사 목록 로드 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDoctor.SelectedIndex >= 0 && cmbDoctor.Tag != null)
            {
                var doctors = (List<Doctor>)cmbDoctor.Tag;
                _selectedDoctor = doctors[cmbDoctor.SelectedIndex];
                LoadSchedules();
            }
        }

        private void UpdateCalendar()
        {
            lblCurrentMonth.Text = _currentDate.ToString("yyyy년 MM월");
            monthCalendar.SetDate(_currentDate);
        }

        private void LoadSchedules()
        {
            if (_selectedDoctor == null)
                return;

            try
            {
                var startDate = new DateTime(_currentDate.Year, _currentDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var schedules = _scheduleService.GetDoctorSchedules(_selectedDoctor.DoctorId, startDate, endDate);
                dgvSchedules.Rows.Clear();

                foreach (var scheduleDetail in schedules)
                {
                    var schedule = scheduleDetail.Schedule;
                    dgvSchedules.Rows.Add(
                        schedule.ScheduleId,
                        schedule.StartDateTime.ToString("yyyy-MM-dd"),
                        schedule.StartDateTime.ToString("HH:mm"),
                        schedule.EndDateTime.ToString("HH:mm"),
                        GetScheduleTypeText(schedule.ScheduleType),
                        schedule.Title ?? "",
                        schedule.Description ?? ""
                    );
                }

                lblScheduleCount.Text = $"총 {dgvSchedules.Rows.Count}건";
            }
            catch (Exception ex)
            {
                MessageBox.Show("일정 로드 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetScheduleTypeText(string scheduleType)
        {
            switch (scheduleType)
            {
                case "Available": return "근무";
                case "Vacation": return "휴가";
                case "Meeting": return "회의";
                case "ClosedDay": return "휴진";
                default: return scheduleType;
            }
        }

        private void BtnPrevMonth_Click(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(-1);
            UpdateCalendar();
            LoadSchedules();
        }

        private void BtnNextMonth_Click(object sender, EventArgs e)
        {
            _currentDate = _currentDate.AddMonths(1);
            UpdateCalendar();
            LoadSchedules();
        }

        private void BtnToday_Click(object sender, EventArgs e)
        {
            _currentDate = DateTime.Today;
            UpdateCalendar();
            LoadSchedules();
        }

        private void BtnAddSchedule_Click(object sender, EventArgs e)
        {
            if (_selectedDoctor == null)
            {
                MessageBox.Show("의사를 먼저 선택하세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var form = new ScheduleEditForm(_selectedDoctor);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSchedules();
            }
        }

        private void BtnEditSchedule_Click(object sender, EventArgs e)
        {
            if (dgvSchedules.SelectedRows.Count == 0)
            {
                MessageBox.Show("편집할 일정을 선택하세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var scheduleId = (int)dgvSchedules.SelectedRows[0].Cells[0].Value;
            var scheduleRepo = new DoctorScheduleRepository();
            var schedule = scheduleRepo.GetById(scheduleId);

            if (schedule != null && _selectedDoctor != null)
            {
                var form = new ScheduleEditForm(_selectedDoctor, schedule);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSchedules();
                }
            }
        }

        private void BtnDeleteSchedule_Click(object sender, EventArgs e)
        {
            if (dgvSchedules.SelectedRows.Count == 0)
            {
                MessageBox.Show("삭제할 일정을 선택하세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("선택한 일정을 삭제하시겠습니까?",
                "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var scheduleId = (int)dgvSchedules.SelectedRows[0].Cells[0].Value;
                    var deleteResult = _scheduleService.DeleteSchedule(scheduleId);

                    if (deleteResult.Success)
                    {
                        MessageBox.Show(deleteResult.Message, "완료",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSchedules();
                    }
                    else
                    {
                        MessageBox.Show(deleteResult.Message, "오류",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("삭제 중 오류: " + ex.Message, "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            _currentDate = e.Start;
            UpdateCalendar();
            LoadSchedules();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
