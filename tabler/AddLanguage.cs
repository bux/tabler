using System;
using System.Windows.Forms;

namespace tabler
{
    public partial class AddLanguage : Form
    {
        private readonly GridUI _myParent;

        public AddLanguage(GridUI parent)
        {
            _myParent = parent;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddThisLanguage_Click(object sender, EventArgs e)
        {
            _myParent.HandleAddLanguage(textBox1.Text);
            Close();
        }
    }
}