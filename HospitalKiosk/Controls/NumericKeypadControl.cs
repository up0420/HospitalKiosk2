using System;
using System.Drawing;
using System.Windows.Forms;

namespace HospitalKiosk.Controls
{
    public partial class NumericKeypadControl : UserControl
    {
        private TextBox _targetTextBox;

        public TextBox TargetTextBox
        {
            get { return _targetTextBox; }
            set { _targetTextBox = value; }
        }

        public NumericKeypadControl()
        {
            InitializeComponent();
        }

        private void NumButton_Click(object sender, EventArgs e)
        {
            if (_targetTextBox == null) return;

            var button = sender as Button;
            if (button != null)
            {
                _targetTextBox.Text += button.Text;
            }
        }

        private void BtnBackspace_Click(object sender, EventArgs e)
        {
            if (_targetTextBox == null || _targetTextBox.Text.Length == 0) return;

            _targetTextBox.Text = _targetTextBox.Text.Substring(0, _targetTextBox.Text.Length - 1);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (_targetTextBox == null) return;

            _targetTextBox.Text = "";
        }
    }
}
