using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace tabler
{
    public class GridUiHelper
    {
        private const KnownColor COLOR_EMPTYCELL = KnownColor.Lavender;
        private const KnownColor COLOR_EDITEDCELL = KnownColor.LightGreen;
        private const KnownColor COLOR_BASELANGUAGE = KnownColor.LightYellow;
        private const int HISTORY_COUNT = 99;

        private readonly GridUI _gridUi;

        public List<CellEditHistory> EditHistory = new List<CellEditHistory>();
        private string _editedCellValue;


        public GridUiHelper(GridUI gridUi)
        {
            _gridUi = gridUi;
        }

        public void ShowData(TranslationComponents tc)
        {
            PrepareTabControl(tc);
        }


        public List<ModInfoContainer> ParseAllTables()
        {
            var allModInfo = new List<ModInfoContainer>();

            // iterating through the modules
            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                string currentModuleName = tabPage.Text;

                // it has to be there
                var gridView = (DataGridView) tabPage.Controls[0];

                var modInfo = new ModInfoContainer();

                var translationsWithKeys = new Dictionary<string, Dictionary<string, string>>();

                // iterating through the keys
                foreach (DataGridViewRow row in gridView.Rows)
                {
                    string keyName = string.Empty;
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

                        string value = string.Empty;

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
            foreach (ModInfoContainer modInfoContainer in tc.AllModInfo)
            {
                var tabPage = new TabPage(modInfoContainer.Name);
                tabPage.Name = modInfoContainer.Name;
                _gridUi.tabControl1.TabPages.Add(tabPage);
            }

            foreach (TabPage tabPage in _gridUi.tabControl1.TabPages)
            {
                DataGridView gridView = CreateGridViewAndFillWithData(tc, tabPage.Text);

                tabPage.Controls.Add(gridView);
            }
        }


        private DataGridView CreateGridViewAndFillWithData(TranslationComponents tc, string currentModule)
        {
            var gridView = new DataGridView();
            gridView.Dock = DockStyle.Fill;

            gridView.EditMode = DataGridViewEditMode.EditOnKeystroke;


            gridView.CellValueChanged += gridView_CellValueChanged;
            gridView.CellBeginEdit += gridView_CellBeginEdit;
            gridView.KeyUp += gridView_KeyUp;
            gridView.KeyDown += gridView_KeyDown;

            foreach (string header in tc.Headers)
            {
                var dgvc = new DataGridViewTextBoxColumn();
                dgvc.HeaderText = header;
                dgvc.DataPropertyName = "Value";

                if (header != TranslationManager.COLUMN_IDNAME)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                gridView.Columns.Add(dgvc);
            }

            ModInfoContainer modInfoContainer = tc.AllModInfo.FirstOrDefault(mi => mi.Name == currentModule);

            if (modInfoContainer != null)
            {
                foreach (var translationsWithKey in modInfoContainer.Values)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(gridView);

                    int index = 1;

                    row.Cells[0].Value = translationsWithKey.Key;


                    foreach (string header in tc.Headers)
                    {
                        if (header == TranslationManager.COLUMN_IDNAME || header == TranslationManager.COLUMN_MODNAME)
                        {
                            continue;
                        }

                        if (header == "English")
                        {
                            row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_BASELANGUAGE);
                        }

                        if (!translationsWithKey.Value.ContainsKey(header))
                        {
                            row.Cells[index].Style.BackColor = Color.FromKnownColor(COLOR_EMPTYCELL);
                        }
                        else
                        {
                            string trans = translationsWithKey.Value[header];
                            row.Cells[index].Value = trans;
                            row.Cells[index].Style.WrapMode = DataGridViewTriState.True;
                        }

                        index += 1;
                    }

                    gridView.Rows.Add(row);
                }
            }


            gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            return gridView;
        }

        void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                // Ctrl + Z
                Undo();
            }
        }


        private void gridView_KeyUp(object sender, KeyEventArgs e)
        {
            var grid = ((DataGridView) sender);
            DataGridViewSelectedCellCollection activeCells = grid.SelectedCells;

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                foreach (DataGridViewCell activeCell in activeCells)
                {
                    //_editedCellValue = activeCell.Value.ToString();

                    var oldValue = activeCell.Value.ToString();
                    var oldColor = activeCell.Style.BackColor;

                    _ignoreForHistory = true;

                    activeCell.Value = "";
                   // gridView_CellValueChanged(sender, new DataGridViewCellEventArgs(activeCell.ColumnIndex, activeCell.RowIndex));

                    AddNewEditHistory(_gridUi.tabControl1.SelectedTab.Text, activeCell, oldValue, activeCell.Value.ToString(), oldColor);
                    
                }
                ((DataGridView) sender).BeginEdit(false);
            }
        }


        private void gridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
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
            DataGridViewCell cell = ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex];

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

        private void AddNewEditHistory(string currentMod, DataGridViewCell cell, string oldValue, string newValue, Color oldBackColor)
        {
            if (EditHistory.Count >= HISTORY_COUNT)
            {
                EditHistory = EditHistory.OrderBy(ceh => ceh.ModifiedDate).ToList();
                EditHistory.RemoveAt(0);
            }

            EditHistory.Add(new CellEditHistory {
                Mod = currentMod,
                Cell = cell,
                OldValue = oldValue,
                NewValue = newValue,
                ModifiedDate = DateTime.Now,
                OldBackColor = oldBackColor
            });
        }

        private bool _ignoreForHistory = false;

        
        public void Undo()
        {
            if (EditHistory.Any() == false)
            {
                return;
            }

            _ignoreForHistory = true;

            var lastEdit = EditHistory.Last();

            var tabPage = _gridUi.tabControl1.TabPages[lastEdit.Mod];
            _gridUi.tabControl1.SelectTab(tabPage);

            // it has to be there
            //var grid = (DataGridView)tabPage.Controls[0];

//            var cell = 
            var currentColor = lastEdit.Cell.Style.BackColor;
            lastEdit.Cell.Value = lastEdit.OldValue;
            lastEdit.Cell.Style.BackColor = lastEdit.OldBackColor;

            //AddNewEditHistory(lastEdit.Mod, lastEdit.Cell, lastEdit.NewValue, lastEdit.OldValue, currentColor);

            EditHistory.Remove(lastEdit);
        }
    }
}