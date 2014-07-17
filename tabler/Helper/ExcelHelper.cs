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

        public ExcelPackage CreateExcelDoc(string path, string name)
        {
            var fileName = Path.Combine(path, name + ".xlsx");

            var newFile = new FileInfo(fileName);

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
            var ws = GetWorksheetByName(pck, WORKSHEETNAME);
            if (ws == null)
            {
                return;
            }

            ws.Cells[1, 1].Value = "ID";

            int curColumn = 2;

            foreach (string header in lstLanguages)
            {
                ws.Cells[1, curColumn].Value = header;
                ws.Column(curColumn).Style.WrapText = true;
                ws.Column(curColumn).Width = 50;
                curColumn += 1;
            }

            ws.Row(1).Style.Font.Bold = true;
            ws.Column(1).Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Column(1).Width = 50;
        }

        public void WriteEntries(ExcelPackage pck, List<Dictionary<string, Dictionary<string, string>>> allTranslations, List<string> lstLanguages)
        {
            var ws = GetWorksheetByName(pck, WORKSHEETNAME);
            if (ws == null)
            {
                return;
            }

            // first row (1) contains the headers -> ID | English | lang1 | lang2, ...
            var currentRow = 2;

            // first column (1) contains the ID
            var currentColumn = 2;

            foreach (var translation in allTranslations)
            {

                foreach (var keyIdWithTranslation in translation)
                {
                    // this is the row iterator

                    var currentId = keyIdWithTranslation.Key;
                    var languages = keyIdWithTranslation.Value;

                    ws.Cells[currentRow, 1].Value = currentId;

                    foreach (var currentLanguage in lstLanguages)
                    {
                        // this is the column iterator

                        var currentTranslation = "";

                        if (languages.Keys.Contains(currentLanguage))
                        {
                            currentTranslation = languages[currentLanguage];
                        }
                        else
                        {
                            ws.Cells[currentRow, currentColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[currentRow, currentColumn].Style.Fill.BackgroundColor.SetColor(Color.Lavender);
                        }

                        ws.Cells[currentRow, currentColumn].Value = currentTranslation;

                        currentColumn += 1;
                    }
                    currentColumn = 2;
                    currentRow += 1;
                }


            }
        }

        private ExcelWorksheet GetWorksheetByName(ExcelPackage pck, string name)
        {
            return pck.Workbook.Worksheets.FirstOrDefault(w => w.Name == name);
        }
    }
}