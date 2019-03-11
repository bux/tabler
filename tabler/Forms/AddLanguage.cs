using System;
using System.Linq;
using System.Windows.Forms;
using tabler.Logic.Enums;
using tabler.Logic.Extensions;

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
            _gridUi.HandleAddLanguage(cmbLanguage.SelectedItem?.ToString());
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AddLanguage_Load(object sender, EventArgs e)
        {
            var allPossibleLanguages = EnumUtils.GetValues<Languages>();
            var usedLanguages = _gridUi.TranslationHelper.TranslationComponents.Headers;

            var remainingLanguages = allPossibleLanguages.Select(l => l.ToString()).ToList().Except(usedLanguages).ToList();

            cmbLanguage.DataSource = remainingLanguages;
        }
    }
}
