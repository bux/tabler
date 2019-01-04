using System;
using System.Windows.Forms;

namespace tabler
{
    public partial class AddLanguage : Form
    {
        private readonly GridUI _gridUi;

        public AddLanguage(GridUI gridUi)
        {
            _gridUi = gridUi;
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAddThisLanguage_Click(object sender, EventArgs e)
        {
            _gridUi.HandleAddLanguage(textBox1.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
