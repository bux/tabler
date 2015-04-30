using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace tabler {
    public partial class GridUI : Form {
        public readonly ConfigHelper ConfigHelper;
        public readonly TranslationManager TranslationManager;
        private GridUiHelper _gridUiHelper;

        public GridUI() {
            InitializeComponent();
            ConfigHelper = new ConfigHelper();
            TranslationManager = new TranslationManager();
        }

        #region " Events "

        private void openModFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            string curPath = "";

            DirectoryInfo lastPath = ConfigHelper.GetLastPathOfDataFiles();

            if (lastPath != null) {
                curPath = lastPath.FullName;
            }

            if (string.IsNullOrEmpty(curPath) == false) {
                folderBrowserDialog1.SelectedPath = curPath;
            }
            folderBrowserDialog1.ShowNewFolderButton = true;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                m_tbModFolder.Text = folderBrowserDialog1.SelectedPath;
                try {
                    // start the process
                    TranslationComponents tc = TranslationManager.GetGridData(new DirectoryInfo(folderBrowserDialog1.SelectedPath));

                    if (tc == null) {
                        MessageBox.Show("No 'stringtable.xml' files found.");
                        return;
                    }


                    _gridUiHelper = new GridUiHelper(this);
                    _gridUiHelper.ShowData(tc);

                    openModFolderToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = true;
                    addLanguageToolStripMenuItem.Enabled = true;
                    statisticsToolStripMenuItem.Enabled = true;

                    ConfigHelper.SetLastPathOfDataFiles(new DirectoryInfo(folderBrowserDialog1.SelectedPath));
                } catch (DuplicateKeyException duplicateKeyException) {
                    MessageBox.Show(String.Format("Duplicate Key found.\nName: '{0}' \nFile: '{1}' \nEntry: '{2}'", duplicateKeyException.KeyName, duplicateKeyException.FileName, duplicateKeyException.EntryName), "Duplicate Key Error");
                } catch (GenericXmlException xmlException) {
                    MessageBox.Show(String.Format("Generic XML Exception.\nName: '{0}' \nFile: '{1}' \nEntry: '{2}'", xmlException.KeyName, xmlException.FileName, xmlException.EntryName), "XML Exception");
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            List<ModInfoContainer> lstModInfos = _gridUiHelper.ParseAllTables();

            TranslationManager.SaveGridData(ConfigHelper.GetLastPathOfDataFiles(), lstModInfos);
        }


        private void tabControl1_Selected(object sender, TabControlEventArgs e) {
            var tabControl = (TabControl)sender;

            TabPage tabPage = tabControl.SelectedTab;

            foreach (DataGridView pb in tabPage.Controls.OfType<DataGridView>()) {
                pb.Focus();
                pb.Select();
            }
        }

        private void addLanguageToolStripMenuItem_Click(object sender, EventArgs e) {
            var frmAddLanguage = new AddLanguage(this);
            frmAddLanguage.ShowDialog(this);
        }


        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
            var frmSettings = new SettingsForm(this);
            frmSettings.ShowDialog(this);
        }


        #endregion


        public void HandleAddLanguage(string newLanguage) {
            _gridUiHelper.AddLanguage(newLanguage);
        }


        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e) {
            var frmStatistics = new TranslationProgress(this);
            frmStatistics.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_gridUiHelper == null) {
                Close();
                return;
            }

            bool canClose = _gridUiHelper.CanClose();
            if (canClose) {
                Close();
            } else {
                if (MessageBox.Show("Discard all changes?", "Exit?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    Close();
                }
            }
        }



    }
}