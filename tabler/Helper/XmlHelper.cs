using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace tabler
{
    public class XmlHelper
    {
        public TranslationComponents ParseXmlFiles(List<FileInfo> allStringTablePaths)
        {
            var lstXDocuments = new List<XDocument>();

            var lstHeader = new List<string>();

            var allModInfos = new List<ModInfoContainer>();

            var transComp = new TranslationComponents();

            foreach (FileInfo currentFile in allStringTablePaths)
            {
                var modInfo = new ModInfoContainer();
                modInfo.FileInfoStringTable = currentFile;
                modInfo.Name = currentFile.Directory.Name;


                XDocument xdoc = XDocument.Load(currentFile.FullName);
                lstXDocuments.Add(xdoc);

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

            transComp.AllModInfo = allModInfos;
            transComp.Headers = lstHeader;

            return transComp;
        }


        public void UpdateXmlFiles(List<FileInfo> filesByNameInDirectory, List<ModInfoContainer> lstModInfos)
        {
            foreach (FileInfo currentFileInfo in filesByNameInDirectory)
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


            xLanguage = (from xel in xID.Descendants() where xel.Name.ToString().ToLowerInvariant() == language.ToLowerInvariant() select xel).FirstOrDefault();


            if (xLanguage != null)
            {
                //exist -> update (or delete)
                if (xLanguage.Value != value)
                {

                    if (string.IsNullOrEmpty(value))
                    {
                        xLanguage.Remove();
                    }
                    else
                    {
                        xLanguage.Value = value;    
                    }

                    changed = true;
                }
            }
            else
            {
                // don't add a new language if the value is empty
                if (!string.IsNullOrEmpty(value))
                {
                    xID.Add(new XElement(language, value));

                    changed = true;
                }
            }

            return changed;
        }
    }
}