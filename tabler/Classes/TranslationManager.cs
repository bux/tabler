using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace tabler
{
    public class TranslationManager
    {
        public const string COLUMN_MODNAME = "Mod";
        public const string COLUMN_IDNAME = "ID";
        public const string STRINGTABLE_NAME = "stringtable.xml";

        private readonly FileInfo _fiExcelFile;

        public TranslationManager()
        {
        }

        public TranslationManager(FileInfo fiExcelFile)
        {
            _fiExcelFile = fiExcelFile;
        }


        private List<string> PrepareHeaders(List<string> headers, bool insertMod)
        {
            headers = headers.OrderBy(l => l).ToList();

            if (headers.Any(x => x.ToLowerInvariant() == "english"))
            {
                headers.Remove("English");
                headers.Insert(0, "English");
            }

            headers.Insert(0, COLUMN_IDNAME);
            if (insertMod)
            {
                headers.Insert(0, COLUMN_MODNAME);
            }


            //remove duplicates
            headers = headers.Distinct().ToList();

            return headers;
        }


        public void ConvertXmlToExcel(DirectoryInfo lastPathToDataFiles, bool insertMod)
        {
            TranslationComponents transComp = GetTranslationComponents(lastPathToDataFiles, insertMod);

            var eh = new ExcelHelper();
            ExcelPackage pck = eh.CreateExcelDoc(_fiExcelFile);

            // structure of headers (columns) -> MOD | ID | English | lang1 | lang2, ...
            eh.CreateHeaderRow(pck, transComp.Headers);

            eh.WriteEntries(pck, transComp.AllModInfo);
            eh.SaveExcelDoc(pck);
        }

        private TranslationComponents GetTranslationComponents(DirectoryInfo lastPathToDataFiles, bool insertMod)
        {
            List<FileInfo> allStringtableFiles = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories).ToList();

            if (allStringtableFiles.Any() == false)
            {
                return null;
            }

            var xh = new XmlHelper();
            TranslationComponents transComp = xh.ParseXmlFiles(allStringtableFiles);

            transComp.Headers = PrepareHeaders(transComp.Headers, insertMod);

            return transComp;
        }


        public void ConvertExcelToXml(DirectoryInfo lastPathToDataFiles)
        {
            var eh = new ExcelHelper();
            ExcelWorksheet ws = eh.LoadExcelDoc(_fiExcelFile);

            //load all mod infos
            List<ModInfoContainer> lstModInfos = eh.LoadModInfos(ws);

            SaveModInfosToXml(lastPathToDataFiles, lstModInfos);
        }

        private static void SaveModInfosToXml(DirectoryInfo lastPathToDataFiles, List<ModInfoContainer> lstModInfos)
        {
            //if going through mods instead of files, 
            // we could create files
            // too tired :D ->  TODO
            List<FileInfo> filesByNameInDirectory = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories);

            var xh = new XmlHelper();
            xh.UpdateXmlFiles(filesByNameInDirectory, lstModInfos);
        }


        public TranslationComponents GetGridData(DirectoryInfo lastPathToDataFiles)
        {
            return GetTranslationComponents(lastPathToDataFiles, false);
        }

        public void SaveGridData(DirectoryInfo lastPathToDataFiles, List<ModInfoContainer> lstModInfos)
        {
            SaveModInfosToXml(lastPathToDataFiles, lstModInfos);
        }
    }
}