using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OfficeOpenXml;

namespace tabler
{
    public class TranslationManager
    {

        private const string STRINGTABLE_NAME = "stringtable.xml";
        private readonly FileInfo _fiExcelFile;

        public TranslationManager(FileInfo fiExcelFile)
        {
            _fiExcelFile = fiExcelFile;
        }



        private List<string> PrepareHeaders(List<string> headers)
        {
            headers = headers.OrderBy(l => l).ToList();

            if (headers.Any(x => x.ToLowerInvariant() == "english"))
            {
                headers.Remove("English");
                headers.Insert(0, "English");
            }

            headers.Insert(0, ExcelHelper.COLUMN_IDNAME);
            headers.Insert(0, ExcelHelper.COLUMN_MODNAME);

            //remove duplicates
            headers = headers.Distinct().ToList();

            return headers;
        }













        public void ConvertXmlToExcel(DirectoryInfo lastPathToDataFiles)
        {
            List<FileInfo> allStringTablePaths = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories).ToList();

            var xh = new XmlHelper();
            TranslationComponents transComp = xh.ParseXmlFiles(allStringTablePaths);

            transComp.Headers = PrepareHeaders(transComp.Headers);

            var eh = new ExcelHelper();
            ExcelPackage pck = eh.CreateExcelDoc(_fiExcelFile);

            //  structure of headers (columns) -> MOD | ID | English | lang1 | lang2, ...
            eh.CreateHeaderRow(pck, transComp.Headers);

            eh.WriteEntries(pck, transComp.AllModInfo);
            eh.SaveExcelDoc(pck);
        }



        public void ConvertExcelToXml(DirectoryInfo lastPathToDataFiles)
        {


            var eh = new ExcelHelper();
            ExcelWorksheet ws = eh.LoadExcelDoc(_fiExcelFile);

            //load all mod infos
            List<ModInfoContainer> lstModInfos = eh.LoadModInfos(ws);

            //if going through mods instead of files, 
            // we could create files
            // too tired :D ->  TODO
            var filesByNameInDirectory = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories);

            var xh = new XmlHelper();
            xh.UpdateXmlFiles(filesByNameInDirectory, lstModInfos);


        }

    }
}
