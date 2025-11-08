using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;
using HospitalKiosk.Data;

namespace HospitalKiosk.Forms
{
    public partial class AdminMainForm : Form
    {
        private readonly Admin _loggedInAdmin;

        public AdminMainForm(Admin admin)
        {
            InitializeComponent();
            _loggedInAdmin = admin;
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"환영합니다, {_loggedInAdmin.AdminName}님";
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                var appointmentRepo = new AppointmentRepository();
                var doctorRepo = new DoctorRepository();
                var patientRepo = new PatientRepository();

                var today = DateTime.Today;
                var allAppointments = appointmentRepo.GetAll();
                var allDoctors = doctorRepo.GetAll();
                var allPatients = patientRepo.GetAll();

                int todayCount = 0;
                int scheduledCount = 0;
                foreach (var apt in allAppointments)
                {
                    if (apt.AppointmentDateTime.Date == today)
                    {
                        todayCount++;
                        if (apt.Status == "Scheduled")
                            scheduledCount++;
                    }
                }

                int doctorCount = 0;
                foreach (var doc in allDoctors)
                {
                    if (doc.IsActive)
                        doctorCount++;
                }

                int patientCount = 0;
                foreach (var patient in allPatients)
                {
                    if (patient.IsActive)
                        patientCount++;
                }

                lblTodayAppointments.Text = $"오늘 예약: {todayCount}건 (대기: {scheduledCount}건)";
                lblDoctorCount.Text = $"활성 의사: {doctorCount}명";
                lblPatientCount.Text = $"등록 환자: {patientCount}명";
            }
            catch (Exception ex)
            {
                MessageBox.Show("통계 로드 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDoctorManagement_Click(object sender, EventArgs e)
        {
            var form = new DoctorManagementForm();
            form.ShowDialog();
            LoadStatistics(); // 통계 새로고침
        }

        private void BtnScheduleManagement_Click(object sender, EventArgs e)
        {
            var form = new DoctorScheduleManagementForm();
            form.ShowDialog();
        }

        private void BtnAppointmentManagement_Click(object sender, EventArgs e)
        {
            var form = new AppointmentManagementForm();
            form.ShowDialog();
            LoadStatistics(); // 통계 새로고침
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            MessageBox.Show("통계가 새로고침되었습니다.", "완료",
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
    }
}
