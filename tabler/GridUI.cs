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

        public GridUI()
        {
            InitializeComponent();
            ConfigHelper = new ConfigHelper();
        }


        private void btnBrowseModFolder_Click(object sender, EventArgs e)
        {
            // TODO - remove this for release
            string preDefSelectedPath = "C:\\Users\\dajo\\Documents\\GitHub\\AGM";
            string preDefSelectedPath2 = "Z:\\git\\AGM";
            string preDefSelectedPath3 = @" J:\arbeit\githubrepo\AGM";

            string curPath = ConfigHelper.GetLastPathOfDataFiles().FullName;

            if (string.IsNullOrEmpty(curPath))
            {
                if (preDefSelectedPath != "" && Directory.Exists(preDefSelectedPath))
                {
                    curPath = preDefSelectedPath;
                }

                if (preDefSelectedPath2 != "" && Directory.Exists(preDefSelectedPath2))
                {
                    curPath = preDefSelectedPath2;
                }

                if (preDefSelectedPath3 != "" && Directory.Exists(preDefSelectedPath3))
                {
                    curPath = preDefSelectedPath3;
                }
            }

            if (string.IsNullOrEmpty(curPath) == false)
            {
                folderBrowserDialog1.SelectedPath = curPath;
            }
            folderBrowserDialog1.ShowNewFolderButton = true;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                preDefSelectedPath = folderBrowserDialog1.SelectedPath;

                m_tbModFolder.Text = preDefSelectedPath;

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

            var guiH = new GridUiHelper(this);
            guiH.ShowData(tc);

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
            var gh = new GridUiHelper(this);
            List<ModInfoContainer> lstModInfos = gh.ParseAllTables();

            var tm = new TranslationManager();
            tm.SaveGridData(ConfigHelper.GetLastPathOfDataFiles(), lstModInfos);
        }


    }
}