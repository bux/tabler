using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using tabler.Classes;
using tabler.Logic.Classes;

namespace tabler.Helper
{
    public class GridUiHelper
    {
        private const KnownColor COLOR_EMPTYCELL = KnownColor.Lavender;
        private const KnownColor COLOR_EDITEDCELL = KnownColor.LightGreen;
        private const KnownColor COLOR_BASELANGUAGE = KnownColor.LightYellow;
        private const int HISTORY_COUNT = 99;

        private readonly GridUI _gridUi;
        private string _editedCellValue;
        private bool _ignoreForHistory;
        private bool _rowDeleted;
        private TranslationComponents _tc;

        private int _lastFindRowIndex = -1;
        private int _lastFindColIndex = -1;
        private string _lastFindTerm;
        private bool _lastFindWasInLastRow;
        private bool _lastFindWasInLastCol;

        private List<CellEditHistory> _editHistory = new List<CellEditHistory>();

        public GridUiHelper(GridUI gridUi)
        {
            _gridUi = gridUi;
        }

        public void ShowData(TranslationComponents tc)
        {
            _tc = tc;
            PrepareTabControl(tc);
        }


        public List<ModInfoContainer> ParseAllTables()
        {
            var allModInfo = new List<ModInfoContainer>();

            // iterating through the modules
            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                var currentModuleName = tabPage.Text;

                // it has to be there
                var gridView = (DataGridView) tabPage.Controls[0];

                var modInfo = new ModInfoContainer();

                var translationsWithKeys = new Dictionary<string, Dictionary<string, string>>();

                // iterating through the keys
                foreach (DataGridViewRow row in gridView.Rows)
                {
                    var keyName = string.Empty;
                    var dicTranslations = new Dictionary<string, string>();

                    // iterating through the languages
                    foreach (DataGridViewTextBoxColumn dgvc in gridView.Columns)
                    {
                        if (dgvc.HeaderText == TranslationManager.COLUMN_IDNAME)
                        {
                            if (row.Cells[dgvc.Index] == null || row.Cells[dgvc.Index].Value == null)
                            {
                                continue;
                            }

                            keyName = row.Cells[dgvc.Index].Value.ToString();
                            continue;
                        }

                        var value = string.Empty;

                        if (row.Cells[dgvc.Index] != null && row.Cells[dgvc.Index].Value != null)
                        {
                            value = row.Cells[dgvc.Index].Value.ToString();
                        }

                        dicTranslations.Add(dgvc.HeaderText, value);
                    }

                    if (string.IsNullOrEmpty(keyName))
                    {
                        continue;
                    }

                    translationsWithKeys.Add(keyName, dicTranslations);
                }

                modInfo.Values = translationsWithKeys;
                modInfo.Name = currentModuleName;


                allModInfo.Add(modInfo);
            }

            return allModInfo;
        }


        private void PrepareTabControl(TranslationComponents tc)
        {
            foreach (var modInfoContainer in tc.AllModInfo)
            {
                var tabPage = new TabPage(modInfoContainer.Name);
                tabPage.Name = modInfoContainer.Name;
                tabPage.AutoScroll = true;

                _gridUi.tabControl1.TabPages.Add(tabPage);
            }

            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                var gridView = CreateGridViewAndFillWithData(tc, tabPage.Text);
                tabPage.Controls.Add(gridView);
            }
        }


        private DataGridView CreateGridViewAndFillWithData(TranslationComponents tc, string currentModule)
        {
            var gridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                EditMode = DataGridViewEditMode.EditOnKeystroke
            };

            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = gridView.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(gridView, true, null);
            }

            gridView.CellValueChanged += gridView_CellValueChanged;
            gridView.CellBeginEdit += gridView_CellBeginEdit;
            gridView.KeyUp += gridView_KeyUp;
            gridView.KeyDown += gridView_KeyDown;
            gridView.UserDeletedRow += gridView_UserDeletedRow;
            gridView.ColumnHeaderMouseClick += gridView_ColumnHeaderMouseClick;

            var lstGridViewColumns = new List<DataGridViewTextBoxColumn>();

            foreach (var header in tc.Headers)
            {
                var dgvc = new DataGridViewTextBoxColumn
                {
                    HeaderText = header,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    Resizable = DataGridViewTriState.True,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                lstGridViewColumns.Add(dgvc);
            }

            gridView.Columns.AddRange(lstGridViewColumns.ToArray());

            var modInfoContainer = tc.AllModInfo.FirstOrDefault(mi => mi.Name == currentModule);

            var lstDataGridViewRows = new List<DataGridViewRow>();

            if (modInfoContainer != null)
            {

                foreach (var translationsWithKey in modInfoContainer.Values)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(gridView);

                    var index = 1;

                    row.Cells[0].Value = translationsWithKey.Key;


                    foreach (var header in tc.Headers)
                    {
                        if (header == TranslationManager.COLUMN_IDNAME || header == TranslationManager.COLUMN_MODNAME)
                        {
                            continue;
                        }

                        if (header == "English")
                        {
                            row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_BASELANGUAGE);
                        }

                        if (!translationsWithKey.Value.ContainsKey(header) || string.IsNullOrWhiteSpace(translationsWithKey.Value[header]))
                        {
                            row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_EMPTYCELL);
                            AddMissingTranslationToStatistics(tc.Statistics, header, currentModule);
                        }
                        else
                        {
                            var trans = translationsWithKey.Value[header];
                            row.Cells[index].Value = trans;
                            row.Cells[index].Style.WrapMode = DataGridViewTriState.True;
                        }

                        index += 1;
                    }

                    lstDataGridViewRows.Add(row);
                }
            }

            gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            gridView.Rows.AddRange(lstDataGridViewRows.ToArray());
            return gridView;
        }


        private void AddNewEditHistory(string currentMod, DataGridViewCell cell, string oldValue, string newValue, Color oldBackColor)
        {
            if (_editHistory.Count >= HISTORY_COUNT)
            {
                _editHistory = _editHistory.OrderBy(ceh => ceh.ModifiedDate).ToList();
                _editHistory.RemoveAt(0);
            }

            _editHistory.Add(new CellEditHistory
            {
                Mod = currentMod,
                CellColumnIndex = cell.ColumnIndex,
                CellRowIndex = cell.RowIndex,
                OldValue = oldValue,
                NewValue = newValue,
                ModifiedDate = DateTime.Now,
                OldBackColor = oldBackColor
            });
        }


        private void Undo()
        {
            if (_editHistory.Any() == false)
            {
                return;
            }

            _ignoreForHistory = true;

            var lastEdit = _editHistory.Last();

            var tabPage = _gridUi.tabControl1.TabPages[lastEdit.Mod];
            _gridUi.tabControl1.SelectTab(tabPage);

            // it has to be there
            var grid = (DataGridView) tabPage.Controls[0];

            var cell = grid.Rows[lastEdit.CellRowIndex].Cells[lastEdit.CellColumnIndex];

            if (cell == null)
            {
                // cell has been deleted -> probably the whole row or column (not yet)
            }
            else
            {
                cell.Value = lastEdit.OldValue;
                cell.Style.BackColor = lastEdit.OldBackColor;
            }

            // TODO
            // bux: yeah, but what?


            _editHistory.Remove(lastEdit);
        }

        public void AddLanguage(string newLanguage)
        {
            if (_tc.Headers.Any(l => l.ToLowerInvariant() == newLanguage.ToLowerInvariant()))
            {
                return;
            }

            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                var grid = (DataGridView) tabPage.Controls[0];

                var dgvc = new DataGridViewTextBoxColumn();
                dgvc.HeaderText = newLanguage;
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

                grid.Columns.Add(dgvc);
            }

            _tc.Headers.Add(newLanguage);
        }

        public bool CanClose()
        {
            return !_editHistory.Any();
        }

        private void PasteEntriesToGrid(string[] arrEntries, DataGridView grid)
        {
            var selCells = grid.SelectedCells;
            if (selCells.Count != arrEntries.Length)
            {
                return;
            }

            arrEntries = arrEntries.Reverse().ToArray();

            var i = 0;
            foreach (DataGridViewCell selCell in selCells)
            {
                selCell.Value = arrEntries[i];
                i += 1;
            }
        }


        private void AddMissingTranslationToStatistics(List<LanguageStatistics> statistics, string language, string modName)
        {
            if (statistics.Any(ls => ls.LanguageName == language))
            {
                var languageStatistics = statistics.First(ls => ls.LanguageName == language);

                if (languageStatistics.MissingModStrings.ContainsKey(modName))
                {
                    // increment
                    languageStatistics.MissingModStrings[modName] = languageStatistics.MissingModStrings[modName] + 1;
                }
                else
                {
                    languageStatistics.MissingModStrings.Add(modName, 1);
                }
            }
            else
            {
                var languageStatistics = new LanguageStatistics();
                languageStatistics.LanguageName = language;
                languageStatistics.MissingModStrings = new Dictionary<string, int>();
                languageStatistics.MissingModStrings.Add(modName, 1);

                statistics.Add(languageStatistics);
            }
        }

        public void Cleanup()
        {
            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                foreach (var dataGridView in tabPage.Controls.OfType<DataGridView>())
                {
                    tabPage.Controls.Remove(dataGridView);
                }

                _gridUi.tabControl1.TabPages.Remove(tabPage);
            }
        }

        public void SelectTabByName(string tabName)
        {
            if (_gridUi.tabControl1.TabPages.ContainsKey(tabName))
            {
                _gridUi.tabControl1.SelectTab(tabName);
            }
        }

        public void PerformFind(string findTerm)
        {
            if (_lastFindTerm != findTerm)
            {
                _lastFindRowIndex = -1;
                _lastFindColIndex = -1;
            }

            _lastFindTerm = findTerm;

            var currentTabPage = _gridUi.tabControl1.SelectedTab;
            var dgv = currentTabPage.Controls.OfType<DataGridView>().FirstOrDefault();

            if (dgv == null)
            {
                return;
            }

            var findRowStartIndex = 0;
            if (_lastFindRowIndex > -1)
            {
                findRowStartIndex = _lastFindRowIndex;
                // if last search was in the last cell we jump down one row
                if (_lastFindWasInLastCol)
                {
                    findRowStartIndex++;
                }
            }

            var findColStartIndex = 0;
            if (_lastFindColIndex > -1)
            {
                findColStartIndex = _lastFindColIndex + 1;
                // if last search was in the last cell we jump down one row
                if (_lastFindWasInLastCol)
                {
                    findColStartIndex = 0;
                }
            }

            // if we were in the last cell reset to 0
            if (_lastFindWasInLastRow && _lastFindWasInLastCol)
            {
                findRowStartIndex = 0;
                findColStartIndex = 0;
            }

            for (var rowIndex = findRowStartIndex; rowIndex < dgv.Rows.Count; rowIndex++)
            {
                DataGridViewRow dgvRow = dgv.Rows[rowIndex];
                var found = false;

                for (var colIndex = findColStartIndex; colIndex < dgvRow.Cells.Count; colIndex++)
                {
                    DataGridViewCell cell = dgvRow.Cells[colIndex];
                    if (!string.IsNullOrEmpty(cell.Value?.ToString()) && cell.Value.ToString().ToLowerInvariant().Contains(findTerm.ToLowerInvariant()))
                    {
                        _lastFindColIndex = cell.ColumnIndex;
                        _lastFindRowIndex = cell.RowIndex;

                        // set if we were in the last row
                        // substract an additional row because of the "new row" feature
                        _lastFindWasInLastRow = rowIndex == dgv.Rows.Count - 2;
                        // set if we were in the last column
                        _lastFindWasInLastCol = colIndex == dgvRow.Cells.Count - 1;

                        found = true;
                        break;
                    }
                }

                // reset for next row
                findColStartIndex = 0;

                if (found)
                {
                    break;
                }
            }

            if (_lastFindRowIndex != -1 && _lastFindColIndex != -1)
            {
                dgv.CurrentCell = dgv.Rows[_lastFindRowIndex].Cells[_lastFindColIndex];
            }
        }

        #region " Events "

        private void gridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView) sender;
            var selectedColumn = grid.Columns[e.ColumnIndex];

            grid.ClearSelection();

            foreach (DataGridViewRow row in grid.Rows)
            {
                var cell = row.Cells[selectedColumn.Index];
                cell.Selected = true;
            }
        }


        private void gridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            _rowDeleted = true;
        }

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    // Ctrl + Z
                    Undo();
                }

                if (e.KeyCode == Keys.V)
                {
                    // Ctrl + V
                    var clipboard = Clipboard.GetText();
                    clipboard = clipboard.Replace("\r\n", "Ѡ");
                    var arrEntries = clipboard.Split('Ѡ');
                    PasteEntriesToGrid(arrEntries, (DataGridView) sender);
                }
            }
        }


        private void gridView_KeyUp(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView) sender;
            var activeCells = grid.SelectedCells;

            if (_rowDeleted)
            {
                _rowDeleted = false;
                return;
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                foreach (DataGridViewCell activeCell in activeCells)
                {
                    if (activeCell == null || activeCell.Value == null)
                    {
                        continue;
                    }

                    var oldValue = activeCell.Value.ToString();
                    var oldColor = activeCell.Style.BackColor;

                    _ignoreForHistory = true;

                    activeCell.Value = "";

                    AddNewEditHistory(_gridUi.tabControl1.SelectedTab.Text, activeCell, oldValue, activeCell.Value.ToString(), oldColor);
                }

                ((DataGridView) sender).BeginEdit(false);
            }
        }

        private void gridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var cell = ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell.Value == null)
            {
                _editedCellValue = string.Empty;
            }
            else
            {
                _editedCellValue = cell.Value.ToString();
            }
        }

        private void gridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var cell = ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell.Value == null)
            {
                cell.Value = "";
            }

            if (_ignoreForHistory != true)
            {
                AddNewEditHistory(_gridUi.tabControl1.SelectedTab.Text, cell, _editedCellValue, cell.Value.ToString(), cell.Style.BackColor);
            }
            else
            {
                _ignoreForHistory = false;
            }

            if (cell.Value.ToString() != _editedCellValue && cell.Value.ToString() != "")
            {
                cell.Style.BackColor = Color.FromKnownColor(COLOR_EDITEDCELL);
            }

            if (cell.Value.ToString() == "")
            {
                cell.Style.BackColor = Color.FromKnownColor(COLOR_EMPTYCELL);
            }
        }

        #endregion

    }
}
