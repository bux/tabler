using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace tabler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _selectedPath = "C:\\Users\\dajo\\Documents\\GitHub\\AGM" ;

        private void btnBrowseModFolder_Click(object sender, EventArgs e)
        {

            if (_selectedPath != "" && Directory.Exists(_selectedPath))
            {
                folderBrowserDialog1.SelectedPath = _selectedPath;
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _selectedPath = folderBrowserDialog1.SelectedPath;
                tbModFolder.Text = _selectedPath;
                btnConvertToExcel.Enabled = true;
            }
        }

        private void btnConvertToExcel_Click(object sender, EventArgs e)
        {
            var fsh = new FileSystemHelper();
            List<String> allStringTablePaths = fsh.GetAllStringTablePaths(_selectedPath);

            var test = new List<XDocument>();

            foreach (var stringTablePath in allStringTablePaths)
            {
                XDocument xdoc = XDocument.Load(stringTablePath);

                var Project = xdoc.Element("Project").Value;
                var Package = xdoc.Element("Package").Value;

            }

        }
    }
}
