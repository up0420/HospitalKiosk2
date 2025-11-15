using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;
using HospitalKiosk.Data;

namespace DoctorScheduleApp.Forms
{
    public partial class MyScheduleEditForm : Form
    {
        private readonly DoctorScheduleService _scheduleService;
        private readonly DoctorScheduleRepository _scheduleRepo;
        private readonly int _doctorId;
        private readonly int? _scheduleId;
        private DoctorSchedule _schedule;
        private readonly bool _isEditMode;

        public MyScheduleEditForm(int doctorId, int? scheduleId)
        {
            InitializeComponent();
            _scheduleService = new DoctorScheduleService();
            _scheduleRepo = new DoctorScheduleRepository();
            _doctorId = doctorId;
            _scheduleId = scheduleId;
            _isEditMode = scheduleId.HasValue;
        }

        private void MyScheduleEditForm_Load(object sender, EventArgs e)
        {
            // 일정 유형 콤보박스 설정
            cmbScheduleType.Items.Clear();
            cmbScheduleType.Items.Add(new ComboBoxItem("근무", "Available"));
            cmbScheduleType.Items.Add(new ComboBoxItem("휴가", "Vacation"));
            cmbScheduleType.Items.Add(new ComboBoxItem("회의", "Meeting"));
            cmbScheduleType.Items.Add(new ComboBoxItem("휴진", "ClosedDay"));
            cmbScheduleType.DisplayMember = "Text";
            cmbScheduleType.ValueMember = "Value";

            if (_isEditMode)
            {
                // 수정 모드
                this.Text = "일정 수정";
                _schedule = _scheduleRepo.GetById(_scheduleId.Value);
                if (_schedule != null)
                {
                    LoadScheduleData();
                }
            }
            else
            {
                // 추가 모드
                this.Text = "일정 추가";
                dtpDate.Value = DateTime.Today;
                dtpStartTime.Value = DateTime.Today.AddHours(9);
                dtpEndTime.Value = DateTime.Today.AddHours(18);
                cmbScheduleType.SelectedIndex = 0;
            }
        }

        private void LoadScheduleData()
        {
            dtpDate.Value = _schedule.StartDateTime.Date;
            dtpStartTime.Value = DateTime.Today.Add(_schedule.StartDateTime.TimeOfDay);
            dtpEndTime.Value = DateTime.Today.Add(_schedule.EndDateTime.TimeOfDay);
            txtNotes.Text = _schedule.Description ?? "";

            // 일정 유형 선택
            for (int i = 0; i < cmbScheduleType.Items.Count; i++)
            {
                var item = (ComboBoxItem)cmbScheduleType.Items[i];
                if (item.Value == _schedule.ScheduleType)
                {
                    cmbScheduleType.SelectedIndex = i;
                    break;
                }
            }

            // 반복 일정 설정
            if (_schedule.IsRecurring)
            {
                chkRecurring.Checked = true;
                var recurringDays = _schedule.GetRecurringDaysOfWeek();
                foreach (var day in recurringDays)
                {
                    int dayIndex = (int)day;
                    clbDaysOfWeek.SetItemChecked(dayIndex, true);
                }
            }
        }

        private void ChkRecurring_CheckedChanged(object sender, EventArgs e)
        {
            bool isRecurring = chkRecurring.Checked;
            lblRecurringDays.Visible = isRecurring;
            clbDaysOfWeek.Visible = isRecurring;

            // 반복 일정일 때는 날짜 선택 비활성화
            dtpDate.Enabled = !isRecurring;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 유효성 검사
                if (cmbScheduleType.SelectedItem == null)
                {
                    MessageBox.Show("일정 유형을 선택하세요.", "입력 오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 반복 일정 검증
                bool isRecurring = chkRecurring.Checked;
                List<DayOfWeek> selectedDays = null;

                if (isRecurring)
                {
                    selectedDays = new List<DayOfWeek>();
                    for (int i = 0; i < clbDaysOfWeek.Items.Count; i++)
                    {
                        if (clbDaysOfWeek.GetItemChecked(i))
                        {
                            selectedDays.Add((DayOfWeek)i);
                        }
                    }

                    if (selectedDays.Count == 0)
                    {
                        MessageBox.Show("반복 요일을 최소 1개 이상 선택하세요.", "입력 오류",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var selectedDate = dtpDate.Value.Date;
                var startDateTime = selectedDate.Add(dtpStartTime.Value.TimeOfDay);
                var endDateTime = selectedDate.Add(dtpEndTime.Value.TimeOfDay);

                if (startDateTime >= endDateTime)
                {
                    MessageBox.Show("종료 시간은 시작 시간보다 늦어야 합니다.", "입력 오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedType = ((ComboBoxItem)cmbScheduleType.SelectedItem).Value;
                var title = selectedType == "Available" ? "근무" :
                           selectedType == "ClosedDay" ? "휴진" :
                           cmbScheduleType.Text;
                var description = txtNotes.Text.Trim();

                // 일정 객체 생성 또는 수정
                DoctorSchedule schedule;
                if (_isEditMode)
                {
                    schedule = _schedule;
                }
                else
                {
                    schedule = new DoctorSchedule
                    {
                        DoctorId = _doctorId
                    };
                }

                schedule.ScheduleType = selectedType;
                schedule.StartDateTime = startDateTime;
                schedule.EndDateTime = endDateTime;
                schedule.Title = title;
                schedule.Description = description;
                schedule.IsRecurring = isRecurring;
                schedule.RecurrencePattern = isRecurring ?
                    DoctorSchedule.CreateWeeklyPattern(selectedDays) : null;

                ScheduleResult result;

                if (_isEditMode)
                {
                    _scheduleRepo.Update(schedule);
                    result = new ScheduleResult
                    {
                        Success = true,
                        Message = "일정이 수정되었습니다.",
                        Schedule = schedule
                    };
                }
                else
                {
                    var scheduleId = _scheduleRepo.Insert(schedule);
                    schedule.ScheduleId = scheduleId;
                    result = new ScheduleResult
                    {
                        Success = true,
                        Message = "일정이 등록되었습니다.",
                        Schedule = schedule
                    };
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
                MessageBox.Show("일정 저장 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // 콤보박스 아이템용 헬퍼 클래스
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public ComboBoxItem(string text, string value)
            {
                Text = text;
                Value = value;
            }
        }
    }
}
