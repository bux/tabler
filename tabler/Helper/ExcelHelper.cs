using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

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


        public void CreateHeaderRow(ExcelPackage pck, List<string> headers)
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.FirstOrDefault(w => w.Name == WORKSHEETNAME);
            if (ws == null)
            {
                return;
            }

            ws.Cells[1, 1].Value = "ID";

            int curColumn = 2;

            foreach (string header in headers)
            {
                ws.Cells[1, curColumn].Value = header;
                curColumn += 1;
            }
        }
    }
}