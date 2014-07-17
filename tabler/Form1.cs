using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using OfficeOpenXml;

namespace tabler
{
    public partial class Form1 : Form
    {
        private string _selectedPath = "C:\\Users\\dajo\\Documents\\GitHub\\AGM";
        private string _selectedPath2 = "Z:\\git\\AGM";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseModFolder_Click(object sender, EventArgs e)
        {
            string curPath = "";
            if (_selectedPath != "" && Directory.Exists(_selectedPath))
            {
                curPath = _selectedPath;
            }

            if (_selectedPath2 != "" && Directory.Exists(_selectedPath2))
            {
                curPath = _selectedPath2;
            }

            if (curPath != "")
            {
                folderBrowserDialog1.SelectedPath = curPath;
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _selectedPath = folderBrowserDialog1.SelectedPath;
                tbModFolder.Text = _selectedPath;
                btnConvertToExcel.Enabled = true;
            }
        }

        private void btnConvertToExcel_Click(object sender, EventArgs e)
        {
            var fsh = new FileSystemHelper();
            List<String> allStringTablePaths = fsh.GetAllStringTablePaths(_selectedPath);

            var lstXDocuments = new List<XDocument>();

            var lstLanguages = new List<string>();

            var allTranslations = new List<Dictionary<string, Dictionary<string, string>>>();

            foreach (string stringTablePath in allStringTablePaths)
            {
                XDocument xdoc = XDocument.Load(stringTablePath);
                lstXDocuments.Add(xdoc);

                //var elem = xdoc.Descendants().Where(a => a.Attribute("ID") != null && a.Attribute("ID").Value == "AGM_Explosives").FirstOrDefault();

                IEnumerable<XElement> keys = xdoc.Descendants().Where(x => x.Name == "Key");

                var dicKeyWithTranslations = new Dictionary<string, Dictionary<string, string>>();

                // all keys
                foreach (XElement key in keys)
                {

                    var currentKeyId = key.Attribute("ID").Value;

                    var dicTranslations = new Dictionary<string, string>();
                    
                    // all languages of a key
                    foreach (XElement language in key.Descendants())
                    {
                        var languageName = language.Name.ToString();

                        dicTranslations.Add(languageName, language.Value);

                        // save all the languages
                        if (lstLanguages.Contains(languageName) == false)
                        {
                            lstLanguages.Add(languageName);
                        }
                    }

                    dicKeyWithTranslations.Add(currentKeyId, dicTranslations);
                }


                allTranslations.Add(dicKeyWithTranslations);
            }

            lstLanguages.Remove("English");
            lstLanguages = lstLanguages.OrderBy(l => l).ToList();
            lstLanguages.Insert(0, "English");

            var path = AppDomain.CurrentDomain.BaseDirectory;

            var eh = new ExcelHelper();
            ExcelPackage pck = eh.CreateExcelDoc(path,"sample");
            eh.CreateHeaderRow(pck, lstLanguages);
            eh.WriteEntries(pck, allTranslations, lstLanguages);
            eh.SaveExcelDoc(pck);

        }
    }
}