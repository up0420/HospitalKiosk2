using System;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class ScheduleEditForm : Form
    {
        private readonly DoctorScheduleService _scheduleService;
        private readonly Doctor _doctor;
        private readonly DoctorSchedule _editingSchedule;
        private readonly bool _isEditMode;

        public ScheduleEditForm(Doctor doctor, DoctorSchedule schedule = null)
        {
            InitializeComponent();
            _scheduleService = new DoctorScheduleService();
            _doctor = doctor;
            _editingSchedule = schedule;
            _isEditMode = schedule != null;
        }

        private void ScheduleEditForm_Load(object sender, EventArgs e)
        {
            lblDoctorName.Text = $"의사: {_doctor.DoctorName}";

            // 일정 유형 초기화
            cmbScheduleType.Items.AddRange(new string[] { "근무", "휴가", "회의", "휴진" });
            cmbScheduleType.SelectedIndex = 0;

            if (_isEditMode)
            {
                this.Text = "일정 편집";
                lblTitle.Text = "일정 정보 편집";
                LoadScheduleData();
            }
            else
            {
                this.Text = "일정 추가";
                lblTitle.Text = "새 일정 추가";
                dtpDate.Value = DateTime.Today;
                dtpStartTime.Value = DateTime.Today.AddHours(9); // 09:00
                dtpEndTime.Value = DateTime.Today.AddHours(18);  // 18:00
            }
        }

        private void LoadScheduleData()
        {
            if (_editingSchedule != null)
            {
                dtpDate.Value = _editingSchedule.StartDateTime.Date;
                dtpStartTime.Value = _editingSchedule.StartDateTime;
                dtpEndTime.Value = _editingSchedule.EndDateTime;

                switch (_editingSchedule.ScheduleType)
                {
                    case "Available": cmbScheduleType.SelectedIndex = 0; break;
                    case "Vacation": cmbScheduleType.SelectedIndex = 1; break;
                    case "Meeting": cmbScheduleType.SelectedIndex = 2; break;
                    case "ClosedDay": cmbScheduleType.SelectedIndex = 3; break;
                }

                txtTitle.Text = _editingSchedule.Title;
                txtDescription.Text = _editingSchedule.Description;
            }
        }

        private string GetScheduleTypeValue(int index)
        {
            switch (index)
            {
                case 0: return "Available";
                case 1: return "Vacation";
                case 2: return "Meeting";
                case 3: return "ClosedDay";
                default: return "Available";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                var startDateTime = dtpDate.Value.Date.Add(dtpStartTime.Value.TimeOfDay);
                var endDateTime = dtpDate.Value.Date.Add(dtpEndTime.Value.TimeOfDay);
                var scheduleType = GetScheduleTypeValue(cmbScheduleType.SelectedIndex);
                var title = txtTitle.Text.Trim();
                var description = txtDescription.Text.Trim();

                ScheduleResult result;

                if (_isEditMode)
                {
                    result = _scheduleService.UpdateSchedule(
                        _editingSchedule.ScheduleId,
                        scheduleType,
                        startDateTime,
                        endDateTime,
                        title,
                        description
                    );
                }
                else
                {
                    result = _scheduleService.CreateSchedule(
                        _doctor.DoctorId,
                        scheduleType,
                        startDateTime,
                        endDateTime,
                        title,
                        description
                    );
                }

                if (result.Success)
                {
                    MessageBox.Show(result.Message, "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Message, "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류 발생: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (cmbScheduleType.SelectedIndex < 0)
            {
                MessageBox.Show("일정 유형을 선택하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var startDateTime = dtpDate.Value.Date.Add(dtpStartTime.Value.TimeOfDay);
            var endDateTime = dtpDate.Value.Date.Add(dtpEndTime.Value.TimeOfDay);

            if (startDateTime >= endDateTime)
            {
                MessageBox.Show("종료 시간은 시작 시간보다 늦어야 합니다.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
