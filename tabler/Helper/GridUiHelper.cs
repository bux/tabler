using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace tabler
{
    public class GridUiHelper
    {
        private readonly GridUI _gridUi;

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
                var currentModuleName = tabPage.Text;

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

                //modInfo.FileInfoStringTable = GetFileInfoForStringtableWithName(currentModuleName);

                allModInfo.Add(modInfo);
            }

            return allModInfo;
        }

        private FileInfo GetFileInfoForStringtableWithName(string currentModuleName)
        {
            return FileSystemHelper.GetFilesByNameInDirectory(_gridUi.ConfigHelper.GetLastPathOfDataFiles(), Path.Combine(currentModuleName , TranslationManager.STRINGTABLE_NAME), SearchOption.AllDirectories).FirstOrDefault();
        }


        private void PrepareTabControl(TranslationComponents tc)
        {
            foreach (ModInfoContainer modInfoContainer in tc.AllModInfo)
            {
                _gridUi.tabControl1.TabPages.Add(modInfoContainer.Name);
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

            ModInfoContainer modInfoContainer = tc.AllModInfo.Where(mi => mi.Name == currentModule).FirstOrDefault();

            if (modInfoContainer != null)
            {
                foreach (var translationsWithKey in modInfoContainer.Values)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(gridView);

                    int index = 1;
                    //row.Cells[0].Value = currentModule;
                    row.Cells[0].Value = translationsWithKey.Key;


                    foreach (string header in tc.Headers)
                    {
                        if (header == TranslationManager.COLUMN_IDNAME || header == TranslationManager.COLUMN_MODNAME)
                        {
                            continue;
                        }

                        if (header == "English")
                        {
                            row.Cells[index].Style.BackColor = Color.LightYellow;
                        }

                        if (!translationsWithKey.Value.ContainsKey(header))
                        {
                            row.Cells[index].Style.BackColor = Color.Lavender;
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

        private string _editedCellValue;

        void gridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
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




        void gridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (cell.Value == null)
            {
                cell.Style.BackColor = Color.Lavender;
                return;
            }

            if (cell.Value.ToString() != _editedCellValue)
            {
                cell.Style.BackColor = Color.LightSalmon;
            }
        }


        
    }
}