using System;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Data;
using HospitalKiosk.Models;
using System.Collections.Generic;

namespace HospitalKiosk.Forms
{
    public partial class DoctorManagementForm : Form
    {
        private readonly DoctorRepository _doctorRepo;
        private readonly DepartmentRepository _departmentRepo;
        private List<Department> _departments;

        public DoctorManagementForm()
        {
            InitializeComponent();
            _doctorRepo = new DoctorRepository();
            _departmentRepo = new DepartmentRepository();
        }

        private void DoctorManagementForm_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadDoctors();
        }

        private void LoadDepartments()
        {
            try
            {
                _departments = new List<Department>(_departmentRepo.GetAll());
                cmbDepartment.Items.Clear();
                cmbDepartment.Items.Add("전체");

                foreach (var dept in _departments)
                {
                    cmbDepartment.Items.Add(dept.DepartmentName);
                }

                cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("진료과 로드 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDoctors()
        {
            try
            {
                var doctors = _doctorRepo.GetAll();
                dgvDoctors.Rows.Clear();

                foreach (var doctor in doctors)
                {
                    var dept = _departmentRepo.GetById(doctor.DepartmentId);
                    dgvDoctors.Rows.Add(
                        doctor.DoctorId,
                        doctor.DoctorCode,
                        doctor.DoctorName,
                        dept?.DepartmentName ?? "",
                        doctor.LicenseNumber,
                        doctor.PhoneNumber,
                        doctor.Email,
                        doctor.IsActive ? "활성" : "비활성"
                    );
                }

                lblTotalCount.Text = $"총 {dgvDoctors.Rows.Count}명";
            }
            catch (Exception ex)
            {
                MessageBox.Show("의사 목록 로드 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvDoctors.Rows.Clear();

                if (cmbDepartment.SelectedIndex == 0) // 전체
                {
                    LoadDoctors();
                    return;
                }

                var selectedDeptName = cmbDepartment.SelectedItem.ToString();
                var selectedDept = _departments.Find(d => d.DepartmentName == selectedDeptName);

                if (selectedDept != null)
                {
                    var doctors = _doctorRepo.GetByDepartmentId(selectedDept.DepartmentId);

                    foreach (var doctor in doctors)
                    {
                        var dept = _departmentRepo.GetById(doctor.DepartmentId);
                        dgvDoctors.Rows.Add(
                            doctor.DoctorId,
                            doctor.DoctorCode,
                            doctor.DoctorName,
                            dept?.DepartmentName ?? "",
                            doctor.LicenseNumber,
                            doctor.PhoneNumber,
                            doctor.Email,
                            doctor.IsActive ? "활성" : "비활성"
                        );
                    }
                }

                lblTotalCount.Text = $"총 {dgvDoctors.Rows.Count}명";
            }
            catch (Exception ex)
            {
                MessageBox.Show("필터링 중 오류: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAddDoctor_Click(object sender, EventArgs e)
        {
            var form = new DoctorEditForm(_departments);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadDoctors();
            }
        }

        private void BtnEditDoctor_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.SelectedRows.Count == 0)
            {
                MessageBox.Show("편집할 의사를 선택하세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var doctorId = (int)dgvDoctors.SelectedRows[0].Cells[0].Value;
            var doctor = _doctorRepo.GetById(doctorId);

            if (doctor != null)
            {
                var form = new DoctorEditForm(_departments, doctor);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadDoctors();
                }
            }
        }

        private void BtnDeleteDoctor_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.SelectedRows.Count == 0)
            {
                MessageBox.Show("삭제할 의사를 선택하세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("선택한 의사를 삭제하시겠습니까?\n\n참고: 예약 이력이 있는 의사는 삭제가 제한될 수 있습니다.",
                "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var doctorId = (int)dgvDoctors.SelectedRows[0].Cells[0].Value;
                    _doctorRepo.Delete(doctorId);
                    MessageBox.Show("의사가 삭제되었습니다.", "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDoctors();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("삭제 중 오류 발생: " + ex.Message, "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadDoctors();
            cmbDepartment.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
