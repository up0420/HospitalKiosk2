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

        private void BtnSetDefaultSchedule_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "기본 평일 근무 스케줄(월~금, 9:00-18:00)을 생성하시겠습니까?\n" +
                "이미 동일한 반복 일정이 있으면 중복될 수 있습니다.",
                "확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var createResult = _scheduleService.CreateDefaultWeekdaySchedule(_loggedInDoctor.DoctorId);

                if (createResult.Success)
                {
                    LoadSchedules();
                    MessageBox.Show(createResult.Message, "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(createResult.Message, "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteMonth_Click(object sender, EventArgs e)
        {
            var year = _currentDate.Year;
            var month = _currentDate.Month;

            var result = MessageBox.Show(
                $"{year}년 {month}월의 모든 일정을 삭제하시겠습니까?\n" +
                "이 작업은 되돌릴 수 없습니다.\n\n" +
                "※ 반복 일정도 함께 삭제됩니다.",
                "경고",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var deleteResult = _scheduleService.DeleteSchedulesByMonth(_loggedInDoctor.DoctorId, year, month);

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

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "모든 일정을 삭제하시겠습니까?\n" +
                "이 작업은 되돌릴 수 없습니다.\n\n" +
                "※ 반복 일정을 포함한 모든 일정이 삭제됩니다.",
                "경고",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // 한 번 더 확인
                var confirmResult = MessageBox.Show(
                    "정말로 모든 일정을 삭제하시겠습니까?\n" +
                    "이 작업은 되돌릴 수 없습니다!",
                    "최종 확인",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation);

                if (confirmResult == DialogResult.Yes)
                {
                    var deleteResult = _scheduleService.DeleteAllSchedules(_loggedInDoctor.DoctorId);

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
        }
    }
}
