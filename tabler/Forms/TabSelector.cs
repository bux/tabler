using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace tabler.Forms
{
    public partial class TabSelector : Form
    {
        private readonly GridUI _parent;
        private readonly List<string> _lstTabNames;

        public TabSelector(GridUI parent)
        {
            InitializeComponent();

            _parent = parent;
            _lstTabNames = new List<string>();
            foreach (TabPage tabPage in _parent.tabControl1.TabPages)
            {
                _lstTabNames.Add(tabPage.Text);
            }
        }

        private void TabSelector_Load(object sender, EventArgs e)
        {
            tbSelectTab.Focus();
            tbSelectTab.AutoCompleteSource = AutoCompleteSource.CustomSource;

            var autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(_lstTabNames.ToArray());

            tbSelectTab.AutoCompleteCustomSource = autoCompleteSource;
            tbSelectTab.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        private void TabSelector_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void tbSelectTab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                _parent.SelectTabByName(tbSelectTab.Text);
            }
        }
    }
}