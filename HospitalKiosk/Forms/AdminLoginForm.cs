using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class AdminLoginForm : Form
    {
        private readonly AdminService _adminService;
        public Admin LoggedInAdmin { get; private set; }

        public AdminLoginForm()
        {
            InitializeComponent();
            _adminService = new AdminService();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("사용자명과 비밀번호를 입력하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = _adminService.Login(username, password);

            if (result.Success)
            {
                LoggedInAdmin = result.Admin;
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

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void AdminLoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }
    }
}
