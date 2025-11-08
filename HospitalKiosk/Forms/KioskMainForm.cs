using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Data;

namespace HospitalKiosk.Forms
{
    public partial class KioskMainForm : Form
    {
        public KioskMainForm()
        {
            InitializeComponent();
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                if (DatabaseHelper.TestConnection())
                {
                    // 연결 성공 - 아무것도 표시하지 않음
                }
                else
                {
                    MessageBox.Show(
                        "데이터베이스 연결에 실패했습니다.\n관리자에게 문의하세요.",
                        "연결 오류",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "데이터베이스 연결 중 오류가 발생했습니다:\n" + ex.Message,
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnAppointment_Click(object sender, EventArgs e)
        {
            var appointmentForm = new AppointmentForm();
            appointmentForm.ShowDialog();
        }

        private void BtnPayment_Click(object sender, EventArgs e)
        {
            var paymentForm = new PaymentForm();
            paymentForm.ShowDialog();
        }

        private void BtnCheckAppointment_Click(object sender, EventArgs e)
        {
            var appointmentCheckForm = new AppointmentCheckForm();
            appointmentCheckForm.ShowDialog();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new PatientRegisterForm();
            registerForm.ShowDialog();
        }

        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            var loginForm = new AdminLoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                var admin = loginForm.LoggedInAdmin;
                var adminMainForm = new AdminMainForm(admin);
                adminMainForm.ShowDialog();
            }
        }

        // 버튼 호버 효과
        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.FromArgb(0, 100, 180);
            }
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                btn.BackColor = Color.FromArgb(0, 122, 204);
            }
        }
    }
}
