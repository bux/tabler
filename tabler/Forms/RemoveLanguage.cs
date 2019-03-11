using System;
using System.Linq;
using System.Windows.Forms;
using tabler.Logic.Enums;
using tabler.Logic.Extensions;

namespace tabler
{
    public partial class RemoveLanguage : Form
    {
        private readonly GridUI _gridUi;

        public RemoveLanguage(GridUI gridUi)
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

        private void btnRemoveThisLanguage_Click(object sender, EventArgs e)
        {
            _gridUi.HandleRemoveLanguage(cmbLanguage.SelectedItem?.ToString());
            DialogResult = DialogResult.OK;
            Close();
        }

        private void RemoveLanguage_Load(object sender, EventArgs e)
        {
            var allPossibleLanguages = EnumUtils.GetValues<Languages>().Select(l => l.ToString());
            var usedLanguages = _gridUi.TranslationHelper.TranslationComponents.Headers;

            var test = usedLanguages.Intersect(allPossibleLanguages).ToList();

            cmbLanguage.DataSource = test;
        }
    }
}
