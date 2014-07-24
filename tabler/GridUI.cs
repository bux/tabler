using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace tabler
{
    public partial class GridUI : Form
    {
        public readonly ConfigHelper ConfigHelper;
        private GridUiHelper _gridUiHelper;

        public GridUI()
        {
            InitializeComponent();
            ConfigHelper = new ConfigHelper();
        }


        private void btnBrowseModFolder_Click(object sender, EventArgs e)
        {
            string curPath = "";

            var lastPath = ConfigHelper.GetLastPathOfDataFiles();

            if (lastPath != null)
            {
                curPath = lastPath.FullName;
            }

            if (string.IsNullOrEmpty(curPath) == false)
            {
                folderBrowserDialog1.SelectedPath = curPath;
            }
            folderBrowserDialog1.ShowNewFolderButton = true;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                m_tbModFolder.Text = folderBrowserDialog1.SelectedPath;
                btnLoadStringtablexmls.Enabled = true;
                ConfigHelper.SetLastPathOfDataFiles(new DirectoryInfo(folderBrowserDialog1.SelectedPath));
            }
        }


        private void GridUI_Load(object sender, EventArgs e)
        {
            DirectoryInfo lastPathToDataFiles = ConfigHelper.GetLastPathOfDataFiles();

            if (lastPathToDataFiles != null)
            {
                m_tbModFolder.Text = lastPathToDataFiles.FullName;
                btnLoadStringtablexmls.Enabled = true;
            }
        }

        private void btnLoadStringtablexmls_Click(object sender, EventArgs e)
        {
            var tm = new TranslationManager();
            TranslationComponents tc = tm.GetGridData(ConfigHelper.GetLastPathOfDataFiles());

            _gridUiHelper = new GridUiHelper(this);
            _gridUiHelper.ShowData(tc);

            btnLoadStringtablexmls.Enabled = false;
            btnSaveStringtableXmls.Enabled = true;

        }


        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            var tabControl = (TabControl)sender;

            var tabPage = tabControl.SelectedTab;

            foreach (var pb in tabPage.Controls.OfType<DataGridView>())
            {
                pb.Focus();
                pb.Select();
            }
        }

        private void btnSaveStringtableXmls_Click(object sender, EventArgs e)
        {
            List<ModInfoContainer> lstModInfos = _gridUiHelper.ParseAllTables();

            var tm = new TranslationManager();
            tm.SaveGridData(ConfigHelper.GetLastPathOfDataFiles(), lstModInfos);
        }


    }
}