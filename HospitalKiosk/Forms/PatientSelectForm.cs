using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HospitalKiosk.Models;

namespace HospitalKiosk.Forms
{
    public class PatientSelectForm : Form
    {
        public Patient SelectedPatient { get; private set; }

        public PatientSelectForm(List<Patient> patients)
        {
            InitializeComponent(patients);
        }

        private void InitializeComponent(List<Patient> patients)
        {
            this.Text = "환자 선택";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var titleLabel = new Label
            {
                Text = "환자를 선택하세요",
                Font = new Font("맑은 고딕", 14, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(560, 30),
                Location = new Point(20, 20)
            };
            this.Controls.Add(titleLabel);

            var listView = new ListView
            {
                Size = new Size(560, 250),
                Location = new Point(20, 60),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("맑은 고딕", 10)
            };

            listView.Columns.Add("환자번호", 120);
            listView.Columns.Add("환자명", 120);
            listView.Columns.Add("생년월일", 120);
            listView.Columns.Add("전화번호", 150);

            foreach (var patient in patients)
            {
                var item = new ListViewItem(patient.PatientNumber);
                item.SubItems.Add(patient.PatientName);
                item.SubItems.Add(patient.BirthDate.ToString("yyyy-MM-dd"));
                item.SubItems.Add(patient.PhoneNumber);
                item.Tag = patient;
                listView.Items.Add(item);
            }

            listView.DoubleClick += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    SelectedPatient = listView.SelectedItems[0].Tag as Patient;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            this.Controls.Add(listView);

            var btnSelect = new Button
            {
                Text = "선택",
                Size = new Size(120, 40),
                Location = new Point(340, 320),
                Font = new Font("맑은 고딕", 11),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSelect.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count > 0)
                {
                    SelectedPatient = listView.SelectedItems[0].Tag as Patient;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("환자를 선택해주세요.", "선택", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            this.Controls.Add(btnSelect);

            var btnCancel = new Button
            {
                Text = "취소",
                Size = new Size(120, 40),
                Location = new Point(470, 320),
                Font = new Font("맑은 고딕", 11),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }
    }
}
