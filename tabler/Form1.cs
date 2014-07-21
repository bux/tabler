using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace tabler
{
    public partial class Form1 : Form
    {
        private const string STRINGTABLE_NAME = "stringtable.xml";
        private const string LASTPATHTODATAFILES = "LastPathToDataFiles";

        private readonly FileInfo _fiConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\config.xml"));
        private readonly FileInfo _fiExcelFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"ExcelFile\Translations.xlsx"));
        private readonly TranslationManager _translationManager;
        private Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private DirectoryInfo _lastPathToDataFiles;
        private ConfigurationSection _section;
        private XDocument _xDocConfig;

        public Form1()
        {
            InitializeComponent();
            _translationManager = new TranslationManager(_fiExcelFile);
        }

        private void btnBrowseModFolder_Click(object sender, EventArgs e)
        {
            // TODO - remove this for release
            string preDefSelectedPath = "C:\\Users\\dajo\\Documents\\GitHub\\AGM";
            string preDefSelectedPath2 = "Z:\\git\\AGM";
            string preDefSelectedPath3 = @" J:\arbeit\githubrepo\AGM";

            string curPath = null;

            if (_lastPathToDataFiles != null)
            {
                curPath = _lastPathToDataFiles.FullName;
            }

            if (string.IsNullOrEmpty(curPath))
            {
                if (preDefSelectedPath != "" && Directory.Exists(preDefSelectedPath))
                {
                    curPath = preDefSelectedPath;
                }

                if (preDefSelectedPath2 != "" && Directory.Exists(preDefSelectedPath2))
                {
                    curPath = preDefSelectedPath2;
                }

                if (preDefSelectedPath3 != "" && Directory.Exists(preDefSelectedPath3))
                {
                    curPath = preDefSelectedPath3;
                }
            }

            if (string.IsNullOrEmpty(curPath) == false)
            {
                folderBrowserDialog1.SelectedPath = curPath;
            }
            folderBrowserDialog1.ShowNewFolderButton = true;


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                preDefSelectedPath = folderBrowserDialog1.SelectedPath;

                _lastPathToDataFiles = new DirectoryInfo(folderBrowserDialog1.SelectedPath);

                m_tbModFolder.Text = preDefSelectedPath;

                btnConvertToExcel.Enabled = true;
                m_btnExcelToXml.Enabled = true;


                SetLastPathOfDataFiles(_lastPathToDataFiles);
            }
        }


        private void btnConvertToExcel_Click(object sender, EventArgs e)
        {
            if (_fiExcelFile.Exists)
            {
                if (MessageBox.Show("Overwrite existing Excel file?", "Overwrite?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }

            _translationManager.ConvertXmlToExcel(_lastPathToDataFiles, true);

            m_btnOpenCreatedExcel.Enabled = true;
        }


        private void m_btnExcelToXml_Click(object sender, EventArgs e)
        {
            _translationManager.ConvertExcelToXml(_lastPathToDataFiles);
        }


        private void m_btnOpenCreatedExcel_Click(object sender, EventArgs e)
        {
            Process.Start(_fiExcelFile.FullName);
        }

        private void CreateOrLoadConfig()
        {
            if (_xDocConfig == null)
            {
                if (_fiConfig.Exists)
                {
                    _xDocConfig = XDocument.Load(_fiConfig.FullName);
                }
                else
                {
                    var path = new XElement(LASTPATHTODATAFILES);

                    //XElement config = new XElement("config",
                    //                               new XElement("lastSavePath",
                    //                                            XmlConvert.EncodeLocalName(m_tbTargetLocation.Text)));

                    var lstElements = new List<XElement>();

                    lstElements.Add(path);

                    _xDocConfig = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XComment("Config file"), new XElement("config", lstElements.ToArray()));

                    _xDocConfig.Save(_fiConfig.FullName);
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

            if (_xDocConfig == null)
            {
                CreateOrLoadConfig();
            }


            XElement pathElement = _xDocConfig.Descendants().Where(d => d.Name == LASTPATHTODATAFILES).FirstOrDefault();

            if (pathElement != null)
            {
                pathElement.Value = XmlConvert.EncodeName(path.FullName);
            }

            SaveConfigXML();
        }

        private DirectoryInfo GetLastPathOfDataFiles()
        {
            if (_xDocConfig == null)
            {
                CreateOrLoadConfig();
            }


            XElement pathElement = _xDocConfig.Descendants().Where(d => d.Name == LASTPATHTODATAFILES).FirstOrDefault();

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
            if (_xDocConfig != null)
            {
                _xDocConfig.Save(_fiConfig.FullName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_fiExcelFile.Exists)
            {
                m_btnOpenCreatedExcel.Enabled = true;
                m_btnExcelToXml.Enabled = true;
            }
            else
            {
                //create dir
                if (_fiExcelFile.Directory.Exists == false)
                {
                    _fiExcelFile.Directory.Create();
                }
            }

            //create dir
            if (_fiConfig.Directory.Exists == false)
            {
                _fiConfig.Directory.Create();
            }


            _lastPathToDataFiles = GetLastPathOfDataFiles();

            if (_lastPathToDataFiles != null)
            {
                m_tbModFolder.Text = _lastPathToDataFiles.FullName;

                btnConvertToExcel.Enabled = true;
            }
        }
    }
}