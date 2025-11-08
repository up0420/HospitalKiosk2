using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace DoctorScheduleApp.Forms
{
    public partial class DoctorLoginForm : Form
    {
        private readonly DoctorService _doctorService;

        public Doctor LoggedInDoctor { get; private set; }

        public DoctorLoginForm()
        {
            InitializeComponent();
            _doctorService = new DoctorService();
        }

        private void DoctorLoginForm_Load(object sender, EventArgs e)
        {
            // 포커스 설정
            txtUsername.Focus();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter 키 누르면 로그인
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                PerformLogin();
            }
        }

        private void PerformLogin()
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("사용자명과 비밀번호를 입력하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 로그인 시도
            var result = _doctorService.Login(username, password);

            if (result.Success)
            {
                LoggedInDoctor = result.Doctor;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(result.Message, "로그인 실패",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
