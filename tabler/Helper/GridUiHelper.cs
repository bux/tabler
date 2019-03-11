using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using tabler.Classes;
using tabler.Logic.Classes;
using tabler.Logic.Enums;
using tabler.Logic.Helper;

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

        public GridUiHelper(GridUI gridUi, TranslationComponents tc)
        {
            _gridUi = gridUi;
            _tc = tc;
        }

        public void ShowData()
        {
            PrepareTabControl(_tc);
        }

        private void PrepareTabControl(TranslationComponents tc)
        {
            var lstTabPages = new List<TabPage>();

            foreach (var stringtable in tc.Stringtables)
            {
                var tabPage = new TabPage(stringtable.Name);
                tabPage.Name = stringtable.Name;
                tabPage.AutoScroll = true;

                var dataGridView = CreateGridViewAndFillWithData(tc, stringtable);
                tabPage.Controls.Add(dataGridView);

                lstTabPages.Add(tabPage);
            }
            
            _gridUi.tabControl1.TabPages.AddRange(lstTabPages.OrderBy(t => t.Name).ToArray());
        }

        private DataGridView CreateGridViewAndFillWithData(TranslationComponents tc, Stringtable stringtable)
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

            var lstDataGridViewRows = new List<DataGridViewRow>();


            foreach (var key in stringtable.AllKeys)
            {
                var row = new DataGridViewRow();
                row.CreateCells(gridView);

                var index = 1;

                row.Cells[0].Value = key.Id;

                var hasOriginal = tc.Headers.Any(s => s.Equals(Languages.Original.ToString()));

                foreach (var header in tc.Headers)
                {
                    if (header == TranslationHelper.COLUMN_IDNAME)
                    {
                        continue;
                    }

                    if (header == Languages.Original.ToString())
                    {
                        row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_BASELANGUAGE);
                    }

                    if (header == Languages.English.ToString() && !hasOriginal)
                    {
                        row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_BASELANGUAGE);
                    }

                    var value = typeof(Key).GetProperty(header)?.GetValue(key, null)?.ToString();

                    if (string.IsNullOrEmpty(value))
                    {
                        row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_EMPTYCELL);
                        AddMissingTranslationToStatistics(tc.Statistics, header, stringtable.Name);
                    }

                    //if (!translationsWithKey.Value.ContainsKey(header) || string.IsNullOrWhiteSpace(translationsWithKey.Value[header]))
                    //{
                    //    row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_EMPTYCELL);
                    //    AddMissingTranslationToStatistics(tc.Statistics, header, stringtable);
                    //}
                    else
                    {
                        row.Cells[index].Value = value;
                        row.Cells[index].Style.WrapMode = DataGridViewTriState.True;
                    }

                    index += 1;
                }

                lstDataGridViewRows.Add(row);
            }


            gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            gridView.Rows.AddRange(lstDataGridViewRows.ToArray());
            return gridView;
        }


        public IEnumerable<Stringtable> ParseAllTables()
        {
            // iterating through the modules
            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                var currentStringtableName = tabPage.Text;

                // it has to be there
                var gridView = (DataGridView)tabPage.Controls[0];

                var currentStringtable = _tc.Stringtables.FirstOrDefault(s => s.Name.Equals(currentStringtableName));
                if (currentStringtable == null)
                {
                    throw new InvalidOperationException($"stringtable with name '{currentStringtableName}' is null");
                }

                // iterating through the keys
                foreach (DataGridViewRow row in gridView.Rows)
                {
                    var keyName = string.Empty;

                    // iterating through the languages
                    foreach (DataGridViewTextBoxColumn dgvc in gridView.Columns)
                    {
                        if (dgvc.HeaderText == TranslationHelper.COLUMN_IDNAME)
                        {
                            if (row.Cells[dgvc.Index] == null || row.Cells[dgvc.Index].Value == null)
                            {
                                continue;
                            }

                            keyName = row.Cells[dgvc.Index].Value.ToString();
                            continue;
                        }

                        if (string.IsNullOrEmpty(keyName))
                        {
                            continue;
                        }

                        var name = keyName;
                        var currentKey = currentStringtable.AllKeys.SingleOrDefault(k => k.Id.Equals(name));

                        if (row.Cells[dgvc.Index] != null && row.Cells[dgvc.Index].Value != null)
                        {
                            var value = row.Cells[dgvc.Index].Value.ToString();

                            var propertyInfo = typeof(Key).GetProperty(dgvc.HeaderText);
                            if (propertyInfo == null)
                            {
                                throw new InvalidOperationException($"Language '{dgvc.HeaderText}' does not exist for Arma 3.");
                            }
                            // Existing Key that gets edited
                            if (currentKey != null)
                            {
                                var currentKeyValue = propertyInfo?.GetValue(currentKey, null);
                                if (currentKeyValue == null || currentKeyValue.Equals(value) == false)
                                {
                                    propertyInfo?.SetValue(currentKey, value);
                                    currentStringtable.HasChanges = true;
                                }
                            }
                            // New Key
                            else
                            {
                                var newKey = new Key {Id = keyName};
                                propertyInfo?.SetValue(newKey, value);
                                var packageToAddTo = currentStringtable.Project.Packages.Last();
                                packageToAddTo.Keys.Add(newKey);
                                currentStringtable.HasChanges = true;
                            }
                        }
                    }
                }
            }

            return _tc.Stringtables;
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
                OldBackColor = oldBackColor,
                Saved = false
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
            var grid = (DataGridView)tabPage.Controls[0];

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
            if (string.IsNullOrEmpty(newLanguage))
            {
                return;
            }

            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                var grid = (DataGridView)tabPage.Controls[0];

                var dgvc = new DataGridViewTextBoxColumn();
                dgvc.HeaderText = newLanguage;
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;

                grid.Columns.Add(dgvc);
            }

            _tc.Headers.Add(newLanguage);
        }

        public bool CanClose()
        {
            return _editHistory.All(eh => eh.Saved);
        }

        public void SetHistoryAsSaved()
        {
            foreach (var cellEditHistory in _editHistory)
            {
                cellEditHistory.Saved = true;
            }
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

                        found = true;
                    }

                    // set if we were in the last row
                    _lastFindWasInLastRow = rowIndex == dgv.Rows.Count - 1;
                    // set if we were in the last column
                    _lastFindWasInLastCol = colIndex == dgvRow.Cells.Count - 1;

                    if (found)
                    {
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

        public void ToggleTranslatedRows(bool show)
        {

            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                // it has to be there
                var gridView = (DataGridView) tabPage.Controls[0];

                foreach (DataGridViewRow row in gridView.Rows)
                {
                    if (show)
                    {
                        row.Visible = true;
                        continue;
                    }

                    var rowComplete = true;

                    foreach (DataGridViewTextBoxColumn dgvc in gridView.Columns)
                    {
                        if (row.Cells[dgvc.Index] == null || row.Cells[dgvc.Index].Value == null)
                        {
                            rowComplete = false;
                            break;
                        }
                    }

                    if (rowComplete)
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        #region " Events "

        private void gridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
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
                    PasteEntriesToGrid(arrEntries, (DataGridView)sender);
                }
            }
        }


        private void gridView_KeyUp(object sender, KeyEventArgs e)
        {
            var grid = (DataGridView)sender;
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

                ((DataGridView)sender).BeginEdit(false);
            }
        }

        private void gridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];

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
