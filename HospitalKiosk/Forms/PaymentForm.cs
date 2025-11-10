using System;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class PaymentForm : Form
    {
        private readonly PaymentService _paymentService;
        private readonly PatientService _patientService;
        private readonly AppointmentService _appointmentService;

        private Patient _selectedPatient;
        private Appointment _selectedAppointment;

        public PaymentForm()
        {
            InitializeComponent();
            _paymentService = new PaymentService();
            _patientService = new PatientService();
            _appointmentService = new AppointmentService();

            InitializeListView();
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void InitializeListView()
        {
            lvAppointments.Columns.Add("예약번호", 120);
            lvAppointments.Columns.Add("예약일시", 150);
            lvAppointments.Columns.Add("의사", 120);
            lvAppointments.Columns.Add("진료과", 120);
            lvAppointments.Columns.Add("상태", 80);
            lvAppointments.Columns.Add("수납상태", 100);
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
            lblPatientInfo.Text = $"환자: {patient.PatientName} ({patient.PatientNumber})";
            lblPatientInfo.ForeColor = System.Drawing.Color.Black;

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            lvAppointments.Items.Clear();

            var appointments = _appointmentService.GetPatientAppointments(_selectedPatient.PatientId);

            foreach (var detail in appointments)
            {
                if (detail.Appointment.Status != "Completed")
                    continue;

                var payment = _paymentService.GetPaymentByAppointmentId(detail.Appointment.AppointmentId);
                var paymentStatus = payment == null ? "미수납" : payment.PaymentStatus;

                var item = new ListViewItem(detail.Appointment.AppointmentNumber);
                item.SubItems.Add(detail.Appointment.AppointmentDateTime.ToString("yyyy-MM-dd HH:mm"));
                item.SubItems.Add(detail.DoctorName ?? "");
                item.SubItems.Add(detail.DepartmentName ?? "");
                item.SubItems.Add(detail.Appointment.Status);
                item.SubItems.Add(paymentStatus);
                item.Tag = detail.Appointment;

                lvAppointments.Items.Add(item);
            }

            if (lvAppointments.Items.Count == 0)
            {
                MessageBox.Show("수납 가능한 진료 내역이 없습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LvAppointments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAppointments.SelectedItems.Count == 0)
            {
                _selectedAppointment = null;
                lblTotalAmount.Text = "0 원";
                txtAmount.Text = "";
                txtAmount.Enabled = false;
                cmbPaymentMethod.Enabled = false;
                btnPay.Enabled = false;
                return;
            }

            _selectedAppointment = lvAppointments.SelectedItems[0].Tag as Appointment;

            var payment = _paymentService.GetPaymentByAppointmentId(_selectedAppointment.AppointmentId);

            if (payment != null && payment.IsFullyPaid())
            {
                MessageBox.Show("이미 수납 완료된 진료입니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal amount = 30000; // 실제로는 진료비 계산 로직 필요

            lblTotalAmount.Text = $"{amount:N0} 원";
            txtAmount.Text = amount.ToString();
            txtAmount.Enabled = true;
            cmbPaymentMethod.Enabled = true;
            btnPay.Enabled = true;
        }

        private void BtnPay_Click(object sender, EventArgs e)
        {
            if (_selectedAppointment == null)
            {
                MessageBox.Show("진료를 선택해주세요.", "수납", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("올바른 금액을 입력해주세요.", "수납", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            var dbPaymentMethod = paymentMethod == "현금" ? "Cash" : (paymentMethod == "카드" ? "Card" : "Transfer");

            var result = _paymentService.CreatePayment(_selectedAppointment.AppointmentId, amount, dbPaymentMethod, amount);

            if (result.Success)
            {
                MessageBox.Show(
                    $"수납이 완료되었습니다.\n\n수납번호: {result.Payment.PaymentNumber}\n금액: {result.Payment.PaidAmount:N0}원\n영수증번호: {result.Payment.ReceiptNumber}",
                    "수납 완료",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadAppointments();
            }
            else
            {
                MessageBox.Show(result.Message, "수납 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtPatientSearch_Enter(object sender, EventArgs e)
        {
            numericKeypad.TargetTextBox = txtPatientSearch;
        }

        private void TxtAmount_Enter(object sender, EventArgs e)
        {
            numericKeypad.TargetTextBox = txtAmount;
        }
    }
}
