using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using tabler.Classes;
using tabler.Forms;
using tabler.Helper;
using tabler.Logic.Classes;
using tabler.Logic.Exceptions;
using tabler.Logic.Helper;
using tabler.Properties;

namespace tabler
{
    public partial class GridUI : Form
    {
        public readonly ConfigHelper ConfigHelper;
        public readonly TranslationManager TranslationManager;
        private GridUiHelper _gridUiHelper;
        private ReleaseVersion _newerRelease;
        private bool _stringtablesLoaded;

        public GridUI()
        {
            InitializeComponent();
            ConfigHelper = new ConfigHelper();
            TranslationManager = new TranslationManager();
            Logger.TextBoxToLogIn = _tbLog;
        }

        #region Events

        private void checkForNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForNewVersion();
        }

        private void getNewVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_newerRelease != null)
            {
                Process.Start(_newerRelease.HtmlUrl);
            }
        }

        private void GridUI_Load(object sender, EventArgs e)
        {
            CheckForNewVersion();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void openModFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var curPath = "";

            var lastPath = ConfigHelper.GetLastPathOfDataFiles();

            if (lastPath != null)
            {
                curPath = lastPath.FullName;
            }

            var folderDialog = new CommonOpenFileDialog {IsFolderPicker = true};
            if (string.IsNullOrEmpty(curPath) == false)
            {
                folderDialog.DefaultDirectory = curPath;
            }


            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                m_tbModFolder.Text = folderDialog.FileName;
                try
                {
                    // start the process
                    var tc = TranslationManager.GetGridData(new DirectoryInfo(folderDialog.FileName));

                    if (tc == null)
                    {
                        MessageBox.Show(Resources.GridUI_No_stringtable_xml_files_found);
                        return;
                    }

                    _gridUiHelper = new GridUiHelper(this);
                    _gridUiHelper.Cleanup();
                    _gridUiHelper.ShowData(tc);

                    saveToolStripMenuItem.Enabled = true;
                    addLanguageToolStripMenuItem.Enabled = true;
                    statisticsToolStripMenuItem.Enabled = true;

                    ConfigHelper.SetLastPathOfDataFiles(new DirectoryInfo(folderDialog.FileName));

                    _stringtablesLoaded = true;
                }
                catch (DuplicateKeyException duplicateKeyException)
                {
                    MessageBox.Show(string.Format(Resources.GridUI_Duplicate_key_found, duplicateKeyException.KeyName, duplicateKeyException.FileName, duplicateKeyException.EntryName), Resources.GridUI_Duplicate_key_found_title);
                }
                catch (GenericXmlException xmlException)
                {
                    MessageBox.Show(string.Format(Resources.GridUI_Generic_xml_exception, xmlException.KeyName, xmlException.FileName, xmlException.EntryName), Resources.GridUI_Generic_xml_exception_title);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lstModInfos = _gridUiHelper.ParseAllTables();

            bool success;

            try
            {
                success = TranslationManager.SaveGridData(ConfigHelper.GetLastPathOfDataFiles(), lstModInfos);
            }
            catch (Exception exception)
            {
                Logger.Log(exception.Message);
                throw;
            }

            if (success)
            {
                Logger.Log(Resources.GridUI_saveToolStripMenuItem_Click_Successfully_saved);
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            var tabControl = (TabControl) sender;

            var tabPage = tabControl.SelectedTab;

            if (tabPage == null)
            {
                return;
            }

            foreach (var pb in tabPage.Controls.OfType<DataGridView>())
            {
                pb.Focus();
                pb.Select();
            }
        }

        private void addLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAddLanguage = new AddLanguage(this);
            frmAddLanguage.ShowDialog(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmSettings = new SettingsForm(this);
            frmSettings.ShowDialog(this);
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmStatistics = new TranslationProgress(this);
            frmStatistics.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_gridUiHelper == null)
            {
                Close();
                return;
            }

            var canClose = _gridUiHelper.CanClose();
            if (canClose)
            {
                Close();
            }
            else
            {
                if (MessageBox.Show(Resources.GridUI_Discard_all_changes, Resources.GridUI_Exit, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Close();
                }
            }
        }

        /// <summary>
        ///     Handles the FormClosing event
        ///     Used to show a message if the user has unsaved changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_gridUiHelper == null)
            {
                return;
            }

            var canClose = _gridUiHelper.CanClose();
            if (!canClose)
            {
                if (MessageBox.Show(Resources.GridUI_Discard_all_changes, Resources.GridUI_Exit, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void GridUI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.T)
                {
                    // Ctrl + T
                    OpenTabSelector();
                }
            }
        }

        #endregion

        #region Functions

        private void OpenTabSelector()
        {
            if (!_stringtablesLoaded)
            {
                return;
            }

            var tabSelector = new TabSelector (this);
            var tabSelectorSize = tabSelector.Size;
            if (this.ClientRectangle.Width <= tabSelectorSize.Width)
            {
                tabSelectorSize.Width = this.ClientRectangle.Width - 40;
            }

            tabSelector.Size = tabSelectorSize;
            tabSelector.StartPosition = FormStartPosition.Manual;

            var newX = this.Left + (this.ClientRectangle.Width - tabSelectorSize.Width) / 2;
            var newY = (int) Math.Floor(this.Location.Y + (this.ClientRectangle.Height) * 0.3);

            // +5 because of padding
            tabSelector.Location = new Point(newX + 5, newY);

            OverlayForm.ShowOverlay(this, tabSelector);
            tabSelector.Show(this);
        }

        private void CheckForNewVersion()
        {
            try
            {
                var gh = new GitHubVersionHelper();
                _newerRelease = gh.CheckForNewVersion(ProductVersion);

                if (_newerRelease != null)
                {
                    getNewVersionToolStripMenuItem.Enabled = true;
                    Logger.Log($"{Resources.GridUI_CheckForNewVersion_New_version_available} -> {_newerRelease.Version}");
                    Logger.Log($"{Resources.GridUI_CheckForNewVersion_Download_the_new_version_at}: {_newerRelease.HtmlUrl}");
                }
                else
                {
                    getNewVersionToolStripMenuItem.Enabled = false;
                    Logger.Log($"{Resources.GridUI_CheckForNewVersion_Current_version_is_up_to_date} {ProductVersion}");
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error: " + ex.Message);
            }
        }

        public void HandleAddLanguage(string newLanguage)
        {
            _gridUiHelper.AddLanguage(newLanguage);
        }

        public void SelectTabByName(string tabName)
        {
            _gridUiHelper.SelectTabByName(tabName);
        }

        #endregion

    }
}
