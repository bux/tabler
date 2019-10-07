using System;
using System.Windows.Forms;
using tabler.Logic.Classes;
using tabler.Logic.Helper;

namespace tabler
{
    public partial class SettingsForm : Form
    {
        private readonly GridUI _myParent;

        public SettingsForm(GridUI myParent)
        {
            _myParent = myParent;
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

        private void Settings_Load(object sender, EventArgs e)
        {
                if (ConfigHelper.CurrentSettings.IndentationSettings == IndentationSettings.Tabs)
                {
                    rbIndentTabs.Checked = true;
                }

                tbIndentation.Text = ConfigHelper.CurrentSettings.TabSize.ToString();
            
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rbIndentTabs_CheckedChanged(object sender, EventArgs e)
        {
            var me = (RadioButton) sender;
            tbIndentation.Enabled = !me.Checked;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (rbIndentTabs.Checked)
            {
                ConfigHelper.CurrentSettings.IndentationSettings = IndentationSettings.Tabs;
            }

            if (rbIndentSpaces.Checked)
            {
                ConfigHelper.CurrentSettings.IndentationSettings = IndentationSettings.Spaces;
                //newSettings.TabSize = 
                int tabSizeValue;
                if (int.TryParse(tbIndentation.Text, out tabSizeValue))
                {
                    ConfigHelper.CurrentSettings.TabSize = tabSizeValue;
                }
                else
                {
                    tbIndentation.Text = "4";
                    return;
                }
            }
            ConfigHelper.SaveSettings();
            Close();
        }
    }
}
