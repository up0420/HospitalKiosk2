using System;
using System.Windows.Forms;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class PatientRegisterForm : Form
    {
        private readonly PatientService _patientService;

        public PatientRegisterForm()
        {
            InitializeComponent();
            _patientService = new PatientService();

            // DateTimePicker MaxDate 설정
            dtpBirthDate.MaxDate = DateTime.Today;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // 유효성 검증
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("환자 이름을 입력해주세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("전화번호를 입력해주세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            // 환자 등록
            var gender = rbMale.Checked ? "M" : "F";
            var result = _patientService.RegisterPatient(
                txtName.Text.Trim(),
                dtpBirthDate.Value,
                gender,
                txtPhone.Text.Trim(),
                txtAddress.Text.Trim(),
                txtEmergencyContact.Text.Trim()
            );

            if (result.Success)
            {
                MessageBox.Show(
                    $"환자 등록이 완료되었습니다.\n\n환자번호: {result.Patient.PatientNumber}\n환자명: {result.Patient.PatientName}",
                    "등록 완료",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message, "등록 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtPhone_Enter(object sender, EventArgs e)
        {
            numericKeypad.TargetTextBox = txtPhone;
        }

        private void TxtEmergencyContact_Enter(object sender, EventArgs e)
        {
            numericKeypad.TargetTextBox = txtEmergencyContact;
        }
    }
}
