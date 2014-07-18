using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace tabler
{
    public class ExcelHelper
    {
        private const string WORKSHEETNAME = "Translation";
        public const string COLUMN_MODNAME = "Mod";
        public const string COLUMN_IDNAME = "ID";

        public ExcelWorksheet LoadExcelDoc(FileInfo file)
        {
            if (file.Exists == false)
            {
                return null;
            }

            var pck = new ExcelPackage(file);

            //todo check if exists
            ExcelWorksheet ws = pck.Workbook.Worksheets[WORKSHEETNAME];
            return ws;
        }

        public ExcelPackage CreateExcelDoc(FileInfo newFile)
        {
            if (newFile.Exists)
            {
                newFile.Delete();
            }

            var pck = new ExcelPackage(newFile);

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(WORKSHEETNAME);
            ws.View.ShowGridLines = true;

            return pck;
        }

        public void SaveExcelDoc(ExcelPackage pck)
        {
            pck.Save();
        }


        public void CreateHeaderRow(ExcelPackage pck, List<string> lstLanguages)
        {
            ExcelWorksheet ws = GetWorksheetByName(pck, WORKSHEETNAME);
            if (ws == null)
            {
                return;
            }


            lstLanguages = lstLanguages.Distinct().ToList();


            int curColumn = 1;

            foreach (string header in lstLanguages)
            {
                ws.Cells[1, curColumn].Value = header;
                ws.Column(curColumn).Style.WrapText = true;
                ws.Column(curColumn).Width = 50;
                curColumn += 1;
            }

            //set headerstyle
            ws.Row(1).Style.Font.Bold = true;
            ws.Column(2).Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Column(2).Width = 50;
        }


        //private int GetHeaderPosition(string name, ExcelWorksheet ws)
        //{
        //    for (int currentColumn = 1; currentColumn < ws.Dimension.End.Column + 1; currentColumn++)
        //    {
        //        if (ws.GetValue(1, currentColumn).ToString().ToUpperInvariant() == name.ToUpperInvariant())
        //        {
        //            return currentColumn;
        //        }
                
        //    }

        //    return -1;
        //}


        private Dictionary<string, int> GetHeaderIndexes(ExcelWorksheet ws)
        {
            var dicHeader = new Dictionary<string, int>();

            for (int currentColumn = 1; currentColumn < ws.Dimension.End.Column + 1; currentColumn++)
            {
                dicHeader.Add(ws.GetValue(1, currentColumn).ToString(), currentColumn);
            }

            return dicHeader;
        }

        public void WriteEntries(ExcelPackage pck, List<ModInfoContainer> lstModInfos)
        {
            ExcelWorksheet ws = GetWorksheetByName(pck, WORKSHEETNAME);
            if (ws == null)
            {
                return;
            }

            //  structure of headers (columns) -> MOD | ID | English | lang1 | lang2, ...
            int currentRow = 2;

            Dictionary<string, int> dicHeader = GetHeaderIndexes(ws);

            int modColumn = dicHeader[COLUMN_MODNAME];
            int idColumn = dicHeader[COLUMN_IDNAME];


            //for each mod
            foreach (ModInfoContainer currentModInfo in lstModInfos)
            {
                //create mod entry once
                ws.Cells[currentRow, 1].Value = currentModInfo.Name;

                // for each ID
                foreach (var currentID in currentModInfo.Values)
                {
                    //is the id
                    ws.Cells[currentRow, idColumn].Value = currentID.Key;


                    //for each language in xlsx
                    foreach (var currentColumn in dicHeader)
                    {
                        //dont handle id and mod column
                        if (currentColumn.Value == modColumn)
                        {
                            continue;
                        }
                        if (currentColumn.Value == idColumn)
                        {
                            continue;
                        }

                        ExcelRange cell = ws.Cells[currentRow, currentColumn.Value];

                        if (currentID.Value.ContainsKey(currentColumn.Key) == false)
                        {
                            //this mod has no entrry for this language
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.Lavender);

                            continue;
                        }

                        //set value
                        cell.Value = currentID.Value[currentColumn.Key];
                    }

                    currentRow += 1;
                }
            }
        }

        private ExcelWorksheet GetWorksheetByName(ExcelPackage pck, string name)
        {
            return pck.Workbook.Worksheets.FirstOrDefault(w => w.Name == name);
        }

        public List<ModInfoContainer> LoadModInfos(ExcelWorksheet ws)
        {
            var lstModInfos = new List<ModInfoContainer>();


            Dictionary<string, int> dicHeader = GetHeaderIndexes(ws);

            int modColumn = dicHeader[COLUMN_MODNAME];
            int idColumn = dicHeader[COLUMN_IDNAME];

            ModInfoContainer currentMod = null;


            List<KeyValuePair<string, int>> allHeaderOrdered = dicHeader.OrderBy(x => x.Value).ToList();

            //each row
            for (int currentRow = 2; currentRow < ws.Dimension.End.Row + 1; currentRow++)
            {
                //<language,value>
                Dictionary<string, string> dicLanguagesOfCurrentID = null;


                //each col
                //for each language in xlsx
                foreach (var currentColumn in allHeaderOrdered)
                {
                    bool isEmpty = ws.Cells[currentRow, currentColumn.Value].IsEmpty();
                    object value = ws.Cells[currentRow, currentColumn.Value].Value;


                    //if column mod
                    if (currentColumn.Value == modColumn)
                    {
                        //check if new mod starts
                        if (isEmpty == false)
                        {
                            currentMod = new ModInfoContainer();

                            currentMod.Name = value.ToString();

                            lstModInfos.Add(currentMod);
                        }
                        continue;
                    }

                    //if column id
                    if (currentColumn.Value == idColumn)
                    {
                        //ids can never be empty
                        if (isEmpty)
                        {
                            break;
                        }

                        dicLanguagesOfCurrentID = new Dictionary<string, string>();

                        currentMod.Values.Add(value.ToString(), dicLanguagesOfCurrentID);
                        continue;
                    }


                    if (isEmpty)
                    {
                        continue;
                    }


                    dicLanguagesOfCurrentID.Add(currentColumn.Key, value.ToString());
                }
            }
            return lstModInfos;
        }
    }
}