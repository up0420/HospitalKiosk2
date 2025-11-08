using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace DoctorScheduleApp.Forms
{
    public partial class DoctorMainForm : Form
    {
        private readonly Doctor _loggedInDoctor;
        private readonly DoctorScheduleService _scheduleService;
        private DateTime _currentDate;

        public DoctorMainForm(Doctor doctor)
        {
            InitializeComponent();
            _loggedInDoctor = doctor;
            _scheduleService = new DoctorScheduleService();
            _currentDate = DateTime.Today;
        }

        private void DoctorMainForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"환영합니다, {_loggedInDoctor.DoctorName} 선생님";
            lblDepartment.Text = $"진료과: {_loggedInDoctor.Department?.DepartmentName ?? "N/A"}";

            // 캘린더 초기 설정
            monthCalendar.SelectionStart = _currentDate;
            monthCalendar.SelectionEnd = _currentDate;

            LoadSchedules();
        }

        private void LoadSchedules()
        {
            try
            {
                // 현재 월의 첫날과 마지막 날
                var startDate = new DateTime(_currentDate.Year, _currentDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var schedules = _scheduleService.GetDoctorSchedules(_loggedInDoctor.DoctorId, startDate, endDate);

                dgvSchedules.Rows.Clear();
                foreach (var scheduleDetail in schedules)
                {
                    var schedule = scheduleDetail.Schedule;
                    var typeText = GetScheduleTypeText(schedule.ScheduleType);
                    var statusText = schedule.IsAvailableSchedule() ? "근무가능" : "근무불가";

                    dgvSchedules.Rows.Add(
                        schedule.ScheduleId,
                        schedule.StartDateTime.ToString("yyyy-MM-dd"),
                        schedule.StartDateTime.ToString("HH:mm"),
                        schedule.EndDateTime.ToString("HH:mm"),
                        typeText,
                        statusText,
                        schedule.Description ?? ""
                    );
                }

                lblScheduleCount.Text = $"이번 달 일정: {dgvSchedules.Rows.Count}건";
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

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            _currentDate = e.Start;
            LoadSchedules();
        }

        private void BtnAddSchedule_Click(object sender, EventArgs e)
        {
            var form = new MyScheduleEditForm(_loggedInDoctor.DoctorId, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSchedules();
            }
        }

        private void BtnEditSchedule_Click(object sender, EventArgs e)
        {
            if (dgvSchedules.SelectedRows.Count == 0)
            {
                MessageBox.Show("수정할 일정을 선택하세요.", "안내",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = dgvSchedules.SelectedRows[0];
            var scheduleId = Convert.ToInt32(selectedRow.Cells["colScheduleId"].Value);

            var form = new MyScheduleEditForm(_loggedInDoctor.DoctorId, scheduleId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSchedules();
            }
        }

        private void BtnDeleteSchedule_Click(object sender, EventArgs e)
        {
            if (dgvSchedules.SelectedRows.Count == 0)
            {
                MessageBox.Show("삭제할 일정을 선택하세요.", "안내",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("선택한 일정을 삭제하시겠습니까?", "확인",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var selectedRow = dgvSchedules.SelectedRows[0];
                var scheduleId = Convert.ToInt32(selectedRow.Cells["colScheduleId"].Value);

                var deleteResult = _scheduleService.DeleteSchedule(scheduleId);

                if (deleteResult.Success)
                {
                    LoadSchedules();
                    MessageBox.Show(deleteResult.Message, "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(deleteResult.Message, "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadSchedules();
            MessageBox.Show("일정이 새로고침되었습니다.", "완료",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("로그아웃 하시겠습니까?", "확인",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void DgvSchedules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                BtnEditSchedule_Click(sender, e);
            }
        }
    }
}
