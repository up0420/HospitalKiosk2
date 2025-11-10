using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class AppointmentCheckForm : Form
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;

        private Patient _selectedPatient;
        private Appointment _selectedAppointment;

        public AppointmentCheckForm()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            _patientService = new PatientService();

            InitializeListView();
        }

        private void InitializeListView()
        {
            lvAppointments.Columns.Add("예약번호", 120);
            lvAppointments.Columns.Add("예약일시", 150);
            lvAppointments.Columns.Add("의사", 120);
            lvAppointments.Columns.Add("진료과", 120);
            lvAppointments.Columns.Add("상태", 100);
            lvAppointments.Columns.Add("사유", 150);
        }

        private void BtnSearchPatient_Click(object sender, EventArgs e)
        {
            var searchTerm = txtPatientSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("환자 이름 또는 전화번호를 입력해주세요.", "검색", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var patients = _patientService.SearchPatients(searchTerm);

            if (patients.Count == 0)
            {
                MessageBox.Show("검색된 환자가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Patient patient;
            if (patients.Count == 1)
            {
                patient = patients[0];
            }
            else
            {
                var patientSelectForm = new PatientSelectForm(patients);
                if (patientSelectForm.ShowDialog() != DialogResult.OK)
                    return;
                patient = patientSelectForm.SelectedPatient;
            }

            _selectedPatient = patient;
            lblPatientInfo.Text = $"환자: {patient.PatientName} ({patient.PatientNumber}) | 전화번호: {patient.PhoneNumber}";
            lblPatientInfo.ForeColor = Color.Black;

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            lvAppointments.Items.Clear();
            ClearAppointmentDetails();

            var appointments = _appointmentService.GetPatientAppointments(_selectedPatient.PatientId);

            foreach (var detail in appointments)
            {
                var statusText = GetStatusText(detail.Appointment.Status);
                var statusColor = GetStatusColor(detail.Appointment.Status);

                var item = new ListViewItem(detail.Appointment.AppointmentNumber);
                item.SubItems.Add(detail.Appointment.AppointmentDateTime.ToString("yyyy-MM-dd HH:mm"));
                item.SubItems.Add(detail.DoctorName ?? "");
                item.SubItems.Add(detail.DepartmentName ?? "");
                item.SubItems.Add(statusText);
                item.SubItems.Add(detail.Appointment.Reason ?? "");
                item.Tag = detail;
                item.ForeColor = statusColor;

                lvAppointments.Items.Add(item);
            }

            if (lvAppointments.Items.Count == 0)
            {
                MessageBox.Show("예약 내역이 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetStatusText(string status)
        {
            switch (status)
            {
                case "Scheduled": return "예약됨";
                case "Completed": return "진료완료";
                case "Cancelled": return "취소됨";
                case "NoShow": return "부재";
                default: return status;
            }
        }

        private Color GetStatusColor(string status)
        {
            switch (status)
            {
                case "Scheduled": return Color.Blue;
                case "Completed": return Color.Green;
                case "Cancelled": return Color.Red;
                case "NoShow": return Color.DarkOrange;
                default: return Color.Black;
            }
        }

        private void LvAppointments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAppointments.SelectedItems.Count == 0)
            {
                ClearAppointmentDetails();
                return;
            }

            var selectedDetail = lvAppointments.SelectedItems[0].Tag as AppointmentDetail;
            _selectedAppointment = selectedDetail.Appointment;

            DisplayAppointmentDetails(selectedDetail);
        }

        private void DisplayAppointmentDetails(AppointmentDetail detail)
        {
            grpAppointmentDetails.Visible = true;

            lblDetailAppointmentNumber.Text = detail.Appointment.AppointmentNumber;
            lblDetailDateTime.Text = detail.Appointment.AppointmentDateTime.ToString("yyyy년 MM월 dd일 HH:mm");
            lblDetailDoctor.Text = $"{detail.DoctorName} ({detail.DepartmentName})";
            lblDetailStatus.Text = GetStatusText(detail.Appointment.Status);
            lblDetailStatus.ForeColor = GetStatusColor(detail.Appointment.Status);
            lblDetailReason.Text = detail.Appointment.Reason ?? "-";
            lblDetailNotes.Text = detail.Appointment.Notes ?? "-";

            // 예약 취소 버튼 활성화 조건: 예약됨 상태이고 예약 시간이 아직 지나지 않은 경우
            bool canCancel = detail.Appointment.Status == "Scheduled" &&
                           detail.Appointment.AppointmentDateTime > DateTime.Now;

            btnCancelAppointment.Enabled = canCancel;

            // 예약 상세가 보이도록 스크롤
            this.ScrollControlIntoView(grpAppointmentDetails);
        }

        private void ClearAppointmentDetails()
        {
            grpAppointmentDetails.Visible = false;
            _selectedAppointment = null;
            btnCancelAppointment.Enabled = false;
        }

        private void BtnCancelAppointment_Click(object sender, EventArgs e)
        {
            if (_selectedAppointment == null)
            {
                MessageBox.Show("예약을 선택해주세요.", "예약 취소", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"예약을 취소하시겠습니까?\n\n예약번호: {_selectedAppointment.AppointmentNumber}\n예약일시: {_selectedAppointment.AppointmentDateTime:yyyy-MM-dd HH:mm}",
                "예약 취소 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            var result = _appointmentService.CancelAppointment(_selectedAppointment.AppointmentId, "환자 요청");

            if (result.Success)
            {
                MessageBox.Show("예약이 취소되었습니다.", "취소 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAppointments();
            }
            else
            {
                MessageBox.Show($"예약 취소 실패: {result.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtPatientSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnSearchPatient_Click(sender, e);
            }
        }

        private void TxtPatientSearch_Enter(object sender, EventArgs e)
        {
            numericKeypad.TargetTextBox = txtPatientSearch;
        }
    }
}
