using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Octokit;
using tabler.Classes;
using tabler.Properties;


namespace tabler {

    public partial class GridUI : Form {
        public readonly ConfigHelper ConfigHelper;
        public readonly TranslationManager TranslationManager;
        private GridUiHelper _gridUiHelper;
        private Release _newerRelease;

        public GridUI() {
            InitializeComponent();
            ConfigHelper = new ConfigHelper();
            TranslationManager = new TranslationManager();
            Logger.TextBoxToLogIn = _tbLog;
        }

        #region Events

        private void checkForNewVersionToolStripMenuItem_Click(object sender, EventArgs e) {
            CheckForNewVersion();
        }

        private void getNewVersionToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_newerRelease != null) {
                Process.Start(_newerRelease.HtmlUrl);
            }
        }

        private void GridUI_Load(object sender, EventArgs e) {
            CheckForNewVersion();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            new AboutBox().ShowDialog();
        }

        private void openModFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            var curPath = "";

            var lastPath = ConfigHelper.GetLastPathOfDataFiles();

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
                    var tc = TranslationManager.GetGridData(new DirectoryInfo(folderBrowserDialog1.SelectedPath));

                    if (tc == null) {
                        MessageBox.Show(Resources.GridUI_No_stringtable_xml_files_found);
                        return;
                    }

                    _gridUiHelper = new GridUiHelper(this);
                    _gridUiHelper.Cleanup();
                    _gridUiHelper.ShowData(tc);

                    saveToolStripMenuItem.Enabled = true;
                    addLanguageToolStripMenuItem.Enabled = true;
                    statisticsToolStripMenuItem.Enabled = true;

                    ConfigHelper.SetLastPathOfDataFiles(new DirectoryInfo(folderBrowserDialog1.SelectedPath));
                } catch (DuplicateKeyException duplicateKeyException) {
                    MessageBox.Show(string.Format(Resources.GridUI_Duplicate_key_found, duplicateKeyException.KeyName, duplicateKeyException.FileName, duplicateKeyException.EntryName), Resources.GridUI_Duplicate_key_found_title);
                } catch (GenericXmlException xmlException) {
                    MessageBox.Show(string.Format(Resources.GridUI_Generic_xml_exception, xmlException.KeyName, xmlException.FileName, xmlException.EntryName), Resources.GridUI_Generic_xml_exception_title);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            var lstModInfos = _gridUiHelper.ParseAllTables();

            var success = TranslationManager.SaveGridData(ConfigHelper.GetLastPathOfDataFiles(), lstModInfos);

            if (success) {
                Logger.Log(Resources.GridUI_saveToolStripMenuItem_Click_Successfully_saved);
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e) {
            var tabControl = (TabControl) sender;

            var tabPage = tabControl.SelectedTab;

            if (tabPage == null) {
                return;
            }

            foreach (var pb in tabPage.Controls.OfType<DataGridView>()) {
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

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e) {
            var frmStatistics = new TranslationProgress(this);
            frmStatistics.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            if (_gridUiHelper == null) {
                Close();
                return;
            }

            var canClose = _gridUiHelper.CanClose();
            if (canClose) {
                Close();
            } else {
                if (MessageBox.Show(Resources.GridUI_Discard_all_changes, Resources.GridUI_Exit, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    Close();
                }
            }
        }

        /// <summary>
        ///   Handles the FormClosing event
        ///   Used to show a message if the user has unsaved changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridUI_FormClosing(object sender, FormClosingEventArgs e) {
            if (_gridUiHelper == null) {
                return;
            }

            var canClose = _gridUiHelper.CanClose();
            if (!canClose) {
                if (MessageBox.Show(Resources.GridUI_Discard_all_changes, Resources.GridUI_Exit, MessageBoxButtons.YesNo) == DialogResult.No) {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Functions

        private void CheckForNewVersion() {
            try {
                var github = new GitHubClient(new ProductHeaderValue("tabler"));
                var releases = github.Repository.Release.GetAll("bux578", "tabler").Result;

                _newerRelease = releases.Where(x => x.PublishedAt.HasValue && new Version(x.TagName.Replace("v", "")) > new Version(ProductVersion)).OrderByDescending(x => x.PublishedAt).FirstOrDefault();

                if (_newerRelease != null) {
                    getNewVersionToolStripMenuItem.Enabled = true;
                    Logger.Log(string.Format("{0} -> {1}", Resources.GridUI_CheckForNewVersion_New_version_available, _newerRelease.Name));
                    Logger.Log(string.Format("{0}: {1}", Resources.GridUI_CheckForNewVersion_Download_the_new_version_at, _newerRelease.HtmlUrl));
                } else {
                    getNewVersionToolStripMenuItem.Enabled = false;
                    Logger.Log(string.Format("{0} {1}", Resources.GridUI_CheckForNewVersion_Current_version_is_up_to_date, ProductVersion));
                }


            } catch (Exception) {

            }
        }

        public void HandleAddLanguage(string newLanguage) {
            _gridUiHelper.AddLanguage(newLanguage);
        }

        #endregion
    }

}