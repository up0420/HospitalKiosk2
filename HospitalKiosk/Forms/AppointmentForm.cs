using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class AppointmentForm : Form
    {
        private readonly AppointmentService _appointmentService;
        private readonly PatientService _patientService;
        private readonly DoctorScheduleService _scheduleService;

        private Patient _selectedPatient;
        private TimeSlot _selectedTimeSlot;

        public AppointmentForm()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            _patientService = new PatientService();
            _scheduleService = new DoctorScheduleService();

            dtpAppointmentDate.MinDate = DateTime.Today;
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            var departments = _scheduleService.GetAllDepartments();
            cmbDepartment.Items.Clear();
            cmbDepartment.Items.Add("진료과를 선택하세요");

            foreach (var dept in departments)
            {
                cmbDepartment.Items.Add(dept);
            }

            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.SelectedIndex = 0;
        }

        private void BtnSearchPatient_Click(object sender, EventArgs e)
        {
            var searchTerm = txtPatientSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("환자 번호 또는 이름을 입력해주세요.", "검색", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var patients = _patientService.SearchPatients(searchTerm);

            if (patients.Count == 0)
            {
                MessageBox.Show("검색된 환자가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (patients.Count == 1)
            {
                SelectPatient(patients[0]);
            }
            else
            {
                var patientSelectForm = new PatientSelectForm(patients);
                if (patientSelectForm.ShowDialog() == DialogResult.OK)
                {
                    SelectPatient(patientSelectForm.SelectedPatient);
                }
            }
        }

        private void SelectPatient(Patient patient)
        {
            _selectedPatient = patient;
            lblPatientInfo.Text = $"환자: {patient.PatientName} ({patient.PatientNumber}) - 생년월일: {patient.BirthDate:yyyy-MM-dd}";
            lblPatientInfo.ForeColor = Color.Black;
            CheckCanConfirm();
        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDepartment.SelectedIndex <= 0) return;

            var department = cmbDepartment.SelectedItem as Department;
            if (department == null) return;

            var doctors = _scheduleService.GetDoctorsByDepartment(department.DepartmentId);
            cmbDoctor.Items.Clear();
            cmbDoctor.Items.Add("의사를 선택하세요");

            foreach (var doctor in doctors)
            {
                cmbDoctor.Items.Add(doctor);
            }

            cmbDoctor.DisplayMember = "DoctorName";
            cmbDoctor.SelectedIndex = 0;
            cmbDoctor.Enabled = true;
        }

        private void CmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDoctor.SelectedIndex <= 0) return;
            LoadTimeSlots();
        }

        private void DtpAppointmentDate_ValueChanged(object sender, EventArgs e)
        {
            if (cmbDoctor.SelectedIndex > 0)
            {
                LoadTimeSlots();
            }
        }

        private void LoadTimeSlots()
        {
            pnlTimeSlots.Controls.Clear();
            _selectedTimeSlot = null;

            var doctor = cmbDoctor.SelectedItem as Doctor;
            if (doctor == null) return;

            var timeSlots = _appointmentService.GetAvailableTimeSlots(doctor.DoctorId, dtpAppointmentDate.Value);

            foreach (var slot in timeSlots)
            {
                var btn = new Button
                {
                    Text = slot.DisplayTime,
                    Size = new Size(100, 35),
                    Margin = new Padding(5),
                    Font = new Font("맑은 고딕", 10F),
                    BackColor = slot.IsAvailable ? Color.LightGreen : Color.LightGray,
                    ForeColor = Color.Black,
                    Enabled = slot.IsAvailable,
                    Tag = slot
                };

                btn.Click += TimeSlotButton_Click;
                pnlTimeSlots.Controls.Add(btn);
            }

            if (timeSlots.Count == 0)
            {
                var lblNoSlots = new Label
                {
                    Text = "예약 가능한 시간이 없습니다",
                    AutoSize = true,
                    Font = new Font("맑은 고딕", 11F),
                    ForeColor = Color.Red
                };
                pnlTimeSlots.Controls.Add(lblNoSlots);
            }

            CheckCanConfirm();
        }

        private void TimeSlotButton_Click(object sender, EventArgs e)
        {
            var clickedBtn = sender as Button;
            var slot = clickedBtn.Tag as TimeSlot;

            foreach (Control ctrl in pnlTimeSlots.Controls)
            {
                if (ctrl is Button button && button.Enabled)
                {
                    var buttonSlot = button.Tag as TimeSlot;
                    button.BackColor = buttonSlot.IsAvailable ? Color.LightGreen : Color.LightGray;
                    button.ForeColor = Color.Black;
                }
            }

            clickedBtn.BackColor = Color.FromArgb(0, 122, 204);
            clickedBtn.ForeColor = Color.White;
            _selectedTimeSlot = slot;
            CheckCanConfirm();
        }

        private void CheckCanConfirm()
        {
            btnConfirm.Enabled = _selectedPatient != null && _selectedTimeSlot != null;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            var doctor = cmbDoctor.SelectedItem as Doctor;
            if (doctor == null) return;

            var result = _appointmentService.CreateAppointment(
                _selectedPatient.PatientId,
                doctor.DoctorId,
                _selectedTimeSlot.StartTime,
                30,
                txtReason.Text.Trim(),
                "Patient"
            );

            if (result.Success)
            {
                MessageBox.Show(
                    $"예약이 완료되었습니다.\n\n예약번호: {result.Appointment.AppointmentNumber}\n예약일시: {result.Appointment.AppointmentDateTime:yyyy-MM-dd HH:mm}",
                    "예약 완료",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "예약 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
