using System;
using System.Windows.Forms;
using DoctorScheduleApp.Forms;

namespace DoctorScheduleApp
{
    static class Program
    {
        /// <summary>
        /// 의사 일정 관리 애플리케이션의 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 로그인 폼 표시
            var loginForm = new DoctorLoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // 로그인 성공 시 메인 폼 실행
                Application.Run(new DoctorMainForm(loginForm.LoggedInDoctor));
            }
        }
    }
}
