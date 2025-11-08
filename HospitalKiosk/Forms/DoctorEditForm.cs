using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Forms
{
    public partial class DoctorEditForm : Form
    {
        private readonly DoctorRepository _doctorRepo;
        private readonly List<Department> _departments;
        private readonly Doctor _editingDoctor;
        private readonly bool _isEditMode;

        public DoctorEditForm(List<Department> departments, Doctor doctor = null)
        {
            InitializeComponent();
            _doctorRepo = new DoctorRepository();
            _departments = departments;
            _editingDoctor = doctor;
            _isEditMode = doctor != null;
        }

        private void DoctorEditForm_Load(object sender, EventArgs e)
        {
            LoadDepartments();

            if (_isEditMode)
            {
                this.Text = "의사 편집";
                lblTitle.Text = "의사 정보 편집";
                LoadDoctorData();
            }
            else
            {
                this.Text = "의사 추가";
                lblTitle.Text = "새 의사 추가";
                chkIsActive.Checked = true;
            }
        }

        private void LoadDepartments()
        {
            cmbDepartment.Items.Clear();
            foreach (var dept in _departments)
            {
                cmbDepartment.Items.Add(dept.DepartmentName);
            }

            if (cmbDepartment.Items.Count > 0)
            {
                cmbDepartment.SelectedIndex = 0;
            }
        }

        private void LoadDoctorData()
        {
            if (_editingDoctor != null)
            {
                txtDoctorCode.Text = _editingDoctor.DoctorCode;
                txtDoctorName.Text = _editingDoctor.DoctorName;
                txtLicenseNumber.Text = _editingDoctor.LicenseNumber;
                txtPhoneNumber.Text = _editingDoctor.PhoneNumber;
                txtEmail.Text = _editingDoctor.Email;
                chkIsActive.Checked = _editingDoctor.IsActive;

                var dept = _departments.Find(d => d.DepartmentId == _editingDoctor.DepartmentId);
                if (dept != null)
                {
                    cmbDepartment.SelectedItem = dept.DepartmentName;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                var selectedDeptName = cmbDepartment.SelectedItem.ToString();
                var selectedDept = _departments.Find(d => d.DepartmentName == selectedDeptName);

                if (selectedDept == null)
                {
                    MessageBox.Show("유효한 진료과를 선택하세요.", "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_isEditMode)
                {
                    _editingDoctor.DoctorCode = txtDoctorCode.Text.Trim();
                    _editingDoctor.DoctorName = txtDoctorName.Text.Trim();
                    _editingDoctor.DepartmentId = selectedDept.DepartmentId;
                    _editingDoctor.LicenseNumber = txtLicenseNumber.Text.Trim();
                    _editingDoctor.PhoneNumber = txtPhoneNumber.Text.Trim();
                    _editingDoctor.Email = txtEmail.Text.Trim();
                    _editingDoctor.IsActive = chkIsActive.Checked;
                    _editingDoctor.UpdatedAt = DateTime.Now;

                    _doctorRepo.Update(_editingDoctor);
                    MessageBox.Show("의사 정보가 수정되었습니다.", "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var newDoctor = new Doctor
                    {
                        DoctorCode = txtDoctorCode.Text.Trim(),
                        DoctorName = txtDoctorName.Text.Trim(),
                        DepartmentId = selectedDept.DepartmentId,
                        LicenseNumber = txtLicenseNumber.Text.Trim(),
                        PhoneNumber = txtPhoneNumber.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        IsActive = chkIsActive.Checked
                    };

                    _doctorRepo.Insert(newDoctor);
                    MessageBox.Show("새 의사가 추가되었습니다.", "완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류 발생: " + ex.Message, "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtDoctorCode.Text))
            {
                MessageBox.Show("의사 코드를 입력하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDoctorCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDoctorName.Text))
            {
                MessageBox.Show("의사 이름을 입력하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDoctorName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLicenseNumber.Text))
            {
                MessageBox.Show("면허 번호를 입력하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLicenseNumber.Focus();
                return false;
            }

            if (cmbDepartment.SelectedIndex < 0)
            {
                MessageBox.Show("진료과를 선택하세요.", "입력 오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
