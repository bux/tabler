using System;
using System.Windows.Forms;

namespace tabler.Forms
{
    public partial class FindForm : Form
    {
        private readonly GridUI _gridUi;

        public FindForm(GridUI gridUi)
        {
            _gridUi = gridUi;
            InitializeComponent();
        }


        private void CloseForm(DialogResult result)
        {
            DialogResult = result;
            _gridUi.FindFormOpen = false;
            Close();
        }


        #region Events

        private void FindForm_Load(object sender, EventArgs e)
        {
            _gridUi.FindFormOpen = true;
            cbSearchTerm.Select();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                _gridUi.FindFormOpen = false;
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseForm(DialogResult.None);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbSearchTerm.Text))
            {
                _gridUi.PerformFind(cbSearchTerm.Text);
            }
        }

        private void cbSearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(cbSearchTerm.Text))
                {
                    _gridUi.PerformFind(cbSearchTerm.Text);
                }
            }
        }


        #endregion


    }
}
