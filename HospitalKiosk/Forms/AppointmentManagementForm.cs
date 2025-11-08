using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HospitalKiosk.Data;
using HospitalKiosk.Models;
using HospitalKiosk.Services;

namespace HospitalKiosk.Forms
{
    public partial class AppointmentManagementForm : Form
    {
        private readonly AppointmentService _appointmentService;
        private readonly DepartmentRepository _departmentRepo;
        private readonly DoctorRepository _doctorRepo;
        private DateTime _selectedDate;

        public AppointmentManagementForm()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            _departmentRepo = new DepartmentRepository();
            _doctorRepo = new DoctorRepository();
            _selectedDate = DateTime.Today;
        }

        private void AppointmentManagementForm_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadDoctors();
            LoadStatusFilter();
            dtpSearchDate.Value = _selectedDate;
            LoadAppointments();
        }

        private void LoadDepartments()
        {
            cmbDepartment.Items.Clear();
            cmbDepartment.Items.Add("전체");

            var departments = _departmentRepo.GetAll();
            foreach (var dept in departments)
            {
                cmbDepartment.Items.Add(dept);
            }
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.SelectedIndex = 0;
        }

        private void LoadDoctors()
        {
            cmbDoctor.Items.Clear();
            cmbDoctor.Items.Add("전체");

            var doctors = _doctorRepo.GetAll();
            foreach (var doctor in doctors)
            {
                cmbDoctor.Items.Add(doctor);
            }
            cmbDoctor.DisplayMember = "DoctorName";
            cmbDoctor.SelectedIndex = 0;
        }

        private void LoadStatusFilter()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("전체");
            cmbStatus.Items.Add("예약됨");
            cmbStatus.Items.Add("진료완료");
            cmbStatus.Items.Add("취소됨");
            cmbStatus.Items.Add("부재");
            cmbStatus.SelectedIndex = 0;
        }

        private void LoadAppointments()
        {
            try
            {
                dgvAppointments.Rows.Clear();

                var startDate = dtpSearchDate.Value.Date;
                var endDate = startDate.AddDays(1);

                // 모든 예약 조회
                var allAppointments = new List<AppointmentDetail>();
                var appointmentRepo = new AppointmentRepository();
                var appointments = appointmentRepo.GetAll();

                foreach (var apt in appointments)
                {
                    if (apt.AppointmentDateTime >= startDate && apt.AppointmentDateTime < endDate)
                    {
                        var detail = _appointmentService.GetAppointmentDetail(apt.AppointmentId);
                        if (detail != null)
                        {
                            allAppointments.Add(detail);
                        }
                    }
                }

                // 필터 적용
                var filteredAppointments = ApplyFilters(allAppointments);

                // 그리드에 표시
                foreach (var detail in filteredAppointments.OrderBy(a => a.Appointment.AppointmentDateTime))
                {
                    var statusText = GetStatusText(detail.Appointment.Status);
                    var statusColor = GetStatusColor(detail.Appointment.Status);

                    var index = dgvAppointments.Rows.Add(
                        detail.Appointment.AppointmentId,
                        detail.Appointment.AppointmentNumber,
                        detail.Appointment.AppointmentDateTime.ToString("HH:mm"),
                        detail.PatientName ?? "",
                        detail.DoctorName ?? "",
                        detail.DepartmentName ?? "",
                        statusText,
                        detail.Appointment.Reason ?? ""
                    );

                    dgvAppointments.Rows[index].Tag = detail;
                    dgvAppointments.Rows[index].DefaultCellStyle.ForeColor = statusColor;
                }

                lblTotalCount.Text = $"총 {dgvAppointments.Rows.Count}건";
            }
            catch (Exception ex)
            {
                MessageBox.Show("예약 목록 로드 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<AppointmentDetail> ApplyFilters(List<AppointmentDetail> appointments)
        {
            var result = appointments.AsEnumerable();

            // 진료과 필터
            if (cmbDepartment.SelectedIndex > 0)
            {
                var selectedDept = cmbDepartment.SelectedItem as Department;
                result = result.Where(a => a.Appointment.DoctorId == selectedDept.DepartmentId);
            }

            // 의사 필터
            if (cmbDoctor.SelectedIndex > 0)
            {
                var selectedDoctor = cmbDoctor.SelectedItem as Doctor;
                result = result.Where(a => a.Appointment.DoctorId == selectedDoctor.DoctorId);
            }

            // 상태 필터
            if (cmbStatus.SelectedIndex > 0)
            {
                var status = GetStatusValue(cmbStatus.SelectedItem.ToString());
                result = result.Where(a => a.Appointment.Status == status);
            }

            // 환자 검색
            if (!string.IsNullOrWhiteSpace(txtPatientSearch.Text))
            {
                var searchTerm = txtPatientSearch.Text.ToLower();
                result = result.Where(a =>
                    (a.PatientName ?? "").ToLower().Contains(searchTerm) ||
                    (a.Appointment.AppointmentNumber ?? "").ToLower().Contains(searchTerm));
            }

            return result.ToList();
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

        private string GetStatusValue(string statusText)
        {
            switch (statusText)
            {
                case "예약됨": return "Scheduled";
                case "진료완료": return "Completed";
                case "취소됨": return "Cancelled";
                case "부재": return "NoShow";
                default: return statusText;
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void DtpSearchDate_ValueChanged(object sender, EventArgs e)
        {
            _selectedDate = dtpSearchDate.Value.Date;
            LoadAppointments();
        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void CmbDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void BtnChangeStatus_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("예약을 선택하세요.", "안내",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedDetail = dgvAppointments.SelectedRows[0].Tag as AppointmentDetail;
            var appointment = selectedDetail.Appointment;

            var statusForm = new Form();
            statusForm.Text = "상태 변경";
            statusForm.Size = new Size(400, 200);
            statusForm.StartPosition = FormStartPosition.CenterParent;
            statusForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            statusForm.MaximizeBox = false;
            statusForm.MinimizeBox = false;

            var lblStatus = new Label();
            lblStatus.Text = "변경할 상태:";
            lblStatus.Location = new Point(30, 30);
            lblStatus.AutoSize = true;
            statusForm.Controls.Add(lblStatus);

            var cmbNewStatus = new ComboBox();
            cmbNewStatus.Items.AddRange(new string[] { "Scheduled", "Completed", "Cancelled", "NoShow" });
            cmbNewStatus.SelectedItem = appointment.Status;
            cmbNewStatus.Location = new Point(30, 60);
            cmbNewStatus.Size = new Size(320, 25);
            cmbNewStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            statusForm.Controls.Add(cmbNewStatus);

            var btnOk = new Button();
            btnOk.Text = "확인";
            btnOk.Location = new Point(150, 110);
            btnOk.Size = new Size(100, 30);
            btnOk.DialogResult = DialogResult.OK;
            statusForm.Controls.Add(btnOk);

            var btnCancel = new Button();
            btnCancel.Text = "취소";
            btnCancel.Location = new Point(260, 110);
            btnCancel.Size = new Size(100, 30);
            btnCancel.DialogResult = DialogResult.Cancel;
            statusForm.Controls.Add(btnCancel);

            if (statusForm.ShowDialog() == DialogResult.OK)
            {
                var newStatus = cmbNewStatus.SelectedItem.ToString();
                var result = _appointmentService.UpdateAppointmentStatus(appointment.AppointmentId, newStatus);

                if (result.Success)
                {
                    MessageBox.Show("상태가 변경되었습니다.", "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments();
                }
                else
                {
                    MessageBox.Show(result.Message, "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancelAppointment_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("예약을 선택하세요.", "안내",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedDetail = dgvAppointments.SelectedRows[0].Tag as AppointmentDetail;
            var appointment = selectedDetail.Appointment;

            if (appointment.Status == "Cancelled")
            {
                MessageBox.Show("이미 취소된 예약입니다.", "안내",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"예약을 취소하시겠습니까?\n\n예약번호: {appointment.AppointmentNumber}\n환자: {selectedDetail.PatientName}\n예약일시: {appointment.AppointmentDateTime:yyyy-MM-dd HH:mm}",
                "예약 취소 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            var result = _appointmentService.CancelAppointment(appointment.AppointmentId, "관리자 취소", "Admin");

            if (result.Success)
            {
                MessageBox.Show("예약이 취소되었습니다.", "취소 완료",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAppointments();
            }
            else
            {
                MessageBox.Show($"예약 취소 실패: {result.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DgvAppointments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedDetail = dgvAppointments.Rows[e.RowIndex].Tag as AppointmentDetail;
                ShowAppointmentDetails(selectedDetail);
            }
        }

        private void ShowAppointmentDetails(AppointmentDetail detail)
        {
            var message = $"예약 상세 정보\n\n" +
                         $"예약번호: {detail.Appointment.AppointmentNumber}\n" +
                         $"환자: {detail.PatientName}\n" +
                         $"의사: {detail.DoctorName}\n" +
                         $"진료과: {detail.DepartmentName}\n" +
                         $"예약일시: {detail.Appointment.AppointmentDateTime:yyyy-MM-dd HH:mm}\n" +
                         $"소요시간: {detail.Appointment.DurationMinutes}분\n" +
                         $"상태: {GetStatusText(detail.Appointment.Status)}\n" +
                         $"사유: {detail.Appointment.Reason ?? "-"}\n" +
                         $"메모: {detail.Appointment.Notes ?? "-"}";

            MessageBox.Show(message, "예약 상세", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
