using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using OfficeOpenXml;

namespace tabler
{
    public partial class Form1 : Form
    {
        private const string STRINGTABLE_NAME = "stringtable.xml";


        private const string LASTPATHTODATAFILES = "LastPathToDataFiles";
        private readonly FileInfo m_fiConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\config.xml"));
        private readonly FileInfo m_fiExcelFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ExcelFile\Translations.xlsx"));
        private Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private DirectoryInfo m_lastPathToDataFiles;
        private XDocument m_xDocConfig;
        private ConfigurationSection section;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseModFolder_Click(object sender, EventArgs e)
        {
            string preDefSelectedPath = "C:\\Users\\dajo\\Documents\\GitHub\\AGM";
            string preDefSelectedPath2 = "Z:\\git\\AGM";
            string preDefSelectedPath3 = @" J:\arbeit\githubrepo\AGM";

            string curPath = null;

            if (m_lastPathToDataFiles != null)
            {
                curPath = m_lastPathToDataFiles.FullName;
            }

            if (string.IsNullOrEmpty(curPath) && preDefSelectedPath != "" && Directory.Exists(preDefSelectedPath))
            {
                curPath = preDefSelectedPath;
            }

            if (string.IsNullOrEmpty(curPath) && preDefSelectedPath2 != "" && Directory.Exists(preDefSelectedPath2))
            {
                curPath = preDefSelectedPath2;
            }

            if (string.IsNullOrEmpty(curPath) && preDefSelectedPath3 != "" && Directory.Exists(preDefSelectedPath3))
            {
                curPath = preDefSelectedPath3;
            }

            if (string.IsNullOrEmpty(curPath) == false)
            {
                folderBrowserDialog1.SelectedPath = curPath;
            }
            folderBrowserDialog1.ShowNewFolderButton = true;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                preDefSelectedPath = folderBrowserDialog1.SelectedPath;

                m_lastPathToDataFiles = new DirectoryInfo(folderBrowserDialog1.SelectedPath);

                m_tbModFolder.Text = preDefSelectedPath;

                btnConvertToExcel.Enabled = true;
                m_btnExcelToXml.Enabled = true;


                SetLastPathOfDataFiles(m_lastPathToDataFiles);
            }
        }


        private List<FileInfo> GetFilesInDirectory(DirectoryInfo di, string filter, SearchOption searchOption)
        {
            List<FileInfo> allStringTablePaths = di.GetFiles(STRINGTABLE_NAME, searchOption).ToList();


            return allStringTablePaths;
        }


        private void btnConvertToExcel_Click(object sender, EventArgs e)
        {
            //var fsh = new FileSystemHelper();
            //List<String> allStringTablePaths = fsh.GetAllStringTablePaths(_selectedPath);

            if (m_fiExcelFile.Exists)
            {
                if (MessageBox.Show("Overwrite existing Excel file?", "Overwrite?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }

            List<FileInfo> allStringTablePaths = GetFilesInDirectory(m_lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories).ToList();


            var lstXDocuments = new List<XDocument>();

            var lstHeader = new List<string>();

            var allModInfos = new List<ModInfoContainer>();

            foreach (FileInfo  currentFile in allStringTablePaths)
            {
                var modInfo = new ModInfoContainer();
                modInfo.FileInfoStringTable = currentFile;
                modInfo.Name = currentFile.Directory.Name;


                XDocument xdoc = XDocument.Load(currentFile.FullName);
                lstXDocuments.Add(xdoc);

                //var elem = xdoc.Descendants().Where(a => a.Attribute("ID") != null && a.Attribute("ID").Value == "AGM_Explosives").FirstOrDefault();

                IEnumerable<XElement> keys = xdoc.Descendants().Where(x => x.Name == "Key");

                var dicKeyWithTranslations = new Dictionary<string, Dictionary<string, string>>();

                // all keys
                foreach (XElement key in keys)
                {
                    string currentKeyId = key.Attribute("ID").Value;

                    var dicTranslations = new Dictionary<string, string>();

                    // all languages of a key
                    foreach (XElement language in key.Descendants())
                    {
                        string languageName = language.Name.ToString();

                        dicTranslations.Add(languageName, language.Value);

                        // save all the languages
                        if (lstHeader.Contains(languageName) == false)
                        {
                            lstHeader.Add(languageName);
                        }
                    }

                    dicKeyWithTranslations.Add(currentKeyId, dicTranslations);
                }


                modInfo.Values = dicKeyWithTranslations;
                allModInfos.Add(modInfo);
            }

            lstHeader = lstHeader.OrderBy(l => l).ToList();

            if (lstHeader.Any(x => x.ToLowerInvariant() == "english"))
            {
                lstHeader.Remove("English");
                lstHeader.Insert(0, "English");
            }


            lstHeader.Insert(0, ExcelHelper.COLUMN_IDNAME);
            lstHeader.Insert(0, ExcelHelper.COLUMN_MODNAME);


            var eh = new ExcelHelper();

            //remove douplets
            lstHeader = lstHeader.Distinct().ToList();

            ExcelPackage pck = eh.CreateExcelDoc(m_fiExcelFile);
            //  structure of headers (columns) -> MOD | ID | English | lang1 | lang2, ...
            eh.CreateHeaderRow(pck, lstHeader);

            eh.WriteEntries(pck, allModInfos);
            eh.SaveExcelDoc(pck);

            m_btnOpenCreatedExcel.Enabled = true;
        }

        private void m_btnExcelToXml_Click(object sender, EventArgs e)
        {
            var eh = new ExcelHelper();

            ExcelWorksheet ws = eh.LoadExcelDoc(m_fiExcelFile);

            //load all mod infos
            List<ModInfoContainer> lstModInfos = eh.LoadModInfos(ws);

            //if go through mods isntead of files, we could create files
            // too tired :D ->  TODO
            foreach (FileInfo currentFileInfo in GetFilesInDirectory(m_lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories))
            {
                ModInfoContainer foundModInfo = lstModInfos.FirstOrDefault(x => x.Name.ToLowerInvariant() == currentFileInfo.Directory.Name.ToLowerInvariant());

                if (foundModInfo == null)
                {
                    continue;
                }

                //get file
                XDocument xdoc = XDocument.Load(currentFileInfo.FullName);

                bool changed = false;
                //for each id in excel
                //Values(ID)(LANGUAGE)
                foreach (var currentID in foundModInfo.Values)
                {
                    //for each language
                    foreach (var currentLanguage in currentID.Value)
                    {
                        if (UpdateOrInsertValue(xdoc, currentID.Key, currentLanguage.Key, currentLanguage.Value))
                        {
                            changed = true;
                        }
                    }
                }

                if (changed)
                {
                    xdoc.Save(currentFileInfo.FullName);
                }
            }
        }

        private bool UpdateOrInsertValue(XDocument xdoc, string id, string language, string value)
        {
            //var ii = xdoc.Descendants().Where(x => x.Name.ToString().ToLowerInvariant() == "key" && (string)x.Attribute("ID") == id);


            //var ii = from xel in  xdoc.Descendants()
            //         where xel.Name.ToString().ToLowerInvariant() == "key" && (string)xel.Attribute("ID") == id
            //         select xel.Descendants(language).ToList();

            bool changed = false;

            //get keys
            List<XElement> keys = (from xel in xdoc.Descendants() where xel.Name.ToString().ToLowerInvariant() == "key" select xel).ToList();

            //get ids
            XElement xID = (from xel in keys where xel.Attributes().Any(x => x.Value.ToString().ToLowerInvariant() == id.ToLowerInvariant()) select xel).FirstOrDefault();

            //get language
            XElement xLanguage = null;


            if (xID == null)
            {
                //new create
                //Too tired to make that in 1 single block :D
                var xelKeyNew = new XElement("Key");
                xelKeyNew.Add(new XAttribute("ID", id));
                //todo: if added first, problem here

                keys.FirstOrDefault().Parent.Add(xelKeyNew);
                xID = xelKeyNew;
            }


            xLanguage = (from xel in xID.Descendants() where xel.Name.ToString().ToLowerInvariant() == language select xel).FirstOrDefault();


            if (xLanguage != null)
            {
                //exist -> update
                if (xLanguage.Value != value)
                {
                    xLanguage.Value = value;
                    changed = true;
                }
            }
            else
            {
                xID.Add(new XElement(language, value));

                changed = true;
            }

            return changed;
        }

        private void m_btnOpenCreatedExcel_Click(object sender, EventArgs e)
        {
            Process.Start(m_fiExcelFile.FullName);
        }

        private void CreateOrLoadConfig()
        {
            if (m_xDocConfig == null)
            {
                if (m_fiConfig.Exists)
                {
                    m_xDocConfig = XDocument.Load(m_fiConfig.FullName);
                }
                else
                {
                    var path = new XElement(LASTPATHTODATAFILES);

                    //XElement config = new XElement("config",
                    //                               new XElement("lastSavePath",
                    //                                            XmlConvert.EncodeLocalName(m_tbTargetLocation.Text)));

                    var lstElements = new List<XElement>();

                    lstElements.Add(path);

                    m_xDocConfig = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XComment("Config file"), new XElement("config", lstElements.ToArray()));

                    m_xDocConfig.Save(m_fiConfig.FullName);
                    SaveConfigXML();
                }
            }
        }


        private void SetLastPathOfDataFiles(DirectoryInfo path)
        {
            if (path == null || path.Exists == false)
            {
                return;
            }

            if (m_xDocConfig == null)
            {
                CreateOrLoadConfig();
            }


            XElement pathElement = m_xDocConfig.Descendants().Where(d => d.Name == LASTPATHTODATAFILES).FirstOrDefault();

            if (pathElement != null)
            {
                pathElement.Value = XmlConvert.EncodeName(path.FullName);
            }

            SaveConfigXML();
        }

        private DirectoryInfo GetLastPathOfDataFiles()
        {
            if (m_xDocConfig == null)
            {
                CreateOrLoadConfig();
            }


            XElement pathElement = m_xDocConfig.Descendants().Where(d => d.Name == LASTPATHTODATAFILES).FirstOrDefault();

            if (pathElement != null)
            {
                string value = XmlConvert.DecodeName(pathElement.Value);
                if (string.IsNullOrEmpty(value) == false)
                {
                    return new DirectoryInfo(value);
                }
            }

            return null;
        }

        private void SaveConfigXML()
        {
            if (m_xDocConfig != null)
            {
                m_xDocConfig.Save(m_fiConfig.FullName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (m_fiExcelFile.Exists)
            {
                m_btnOpenCreatedExcel.Enabled = true;
                m_btnExcelToXml.Enabled = true;
            }
            else
            {
                //create dir
                if (m_fiExcelFile.Directory.Exists == false)
                {
                    m_fiExcelFile.Directory.Create();
                }
            }

            //create dir
            if (m_fiConfig.Directory.Exists == false)
            {
                m_fiConfig.Directory.Create();
            }


            m_lastPathToDataFiles = GetLastPathOfDataFiles();

            if (m_lastPathToDataFiles != null)
            {
                m_tbModFolder.Text = m_lastPathToDataFiles.FullName;

                btnConvertToExcel.Enabled = true;
            }
        }
    }
}