using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using tabler.Classes;

namespace tabler
{
    public class XmlHelper
    {
        private const string KEY_NAME = "Key";
        private const string ID_NAME = "ID";
        private const string PACKAGE_NAME = "Package";

        public TranslationComponents ParseXmlFiles(List<FileInfo> allStringTablePaths)
        {
            var lstHeader = new ConcurrentBag<string>();

            var allModInfos = new ConcurrentBag<ModInfoContainer>();

            var transComp = new TranslationComponents();

            Parallel.ForEach(allStringTablePaths, currentFile =>
            {
                var modInfo = new ModInfoContainer
                {
                    FileInfoStringTable = currentFile,
                    Name = currentFile.Directory.Name
                };

                XDocument xdoc;

                try
                {
                    xdoc = XDocument.Load(currentFile.FullName);
                }
                catch (XmlException xmlException)
                {
                    throw new GenericXmlException("", currentFile.FullName, xmlException.Message);
                }

                var keys = xdoc.Descendants().Where(x => x.Name == KEY_NAME);

                var dicKeyWithTranslations = new Dictionary<string, List<XmlKeyTranslation>>();

                // all keys
                foreach (var key in keys)
                {
                    var currentKeyId = key.Attribute(ID_NAME).Value;

                    var lstTranslations = new List<XmlKeyTranslation>();

                    // all languages of a key
                    var i = 0;
                    foreach (var language in key.Descendants())
                    {
                        var xmlKeyTranslation = new XmlKeyTranslation();
                        xmlKeyTranslation.Language = language.Name.ToString();
                        xmlKeyTranslation.Position = i;

                        if (lstTranslations.Any(xkt => xkt.Language == xmlKeyTranslation.Language))
                        {
                            throw new DuplicateKeyException(xmlKeyTranslation.Language, currentFile.FullName, currentKeyId);
                        }

                        lstTranslations.Add(xmlKeyTranslation);

                        // save all the languages
                        if (lstHeader.Contains(xmlKeyTranslation.Language) == false)
                        {
                            lstHeader.Add(xmlKeyTranslation.Language);
                        }
                        i++;
                    }

                    if (dicKeyWithTranslations.ContainsKey(currentKeyId))
                    {
                        throw new DuplicateKeyException(currentKeyId, currentFile.FullName, currentKeyId);
                    }

                    dicKeyWithTranslations.Add(currentKeyId, lstTranslations);
                }

                modInfo.Values = dicKeyWithTranslations;
                allModInfos.Add(modInfo);
            });

            transComp.AllModInfo = allModInfos.OrderBy(mic => mic.Name).ToList();
            transComp.Headers = lstHeader.OrderBy(h => h).ToList();

            return transComp;
        }


        public void UpdateXmlFiles(List<FileInfo> filesByNameInDirectory, List<ModInfoContainer> lstModInfos)
        {
            Parallel.ForEach(filesByNameInDirectory, currentFileInfo =>
            {
                var foundModInfo = lstModInfos.FirstOrDefault(x => x.Name.ToLowerInvariant() == currentFileInfo.Directory.Name.ToLowerInvariant());

                if (foundModInfo == null)
                {
                    return;
                }

                //get file
                var xdoc = XDocument.Load(currentFileInfo.FullName);

                var changed = false;
                //for each id in excel
                //Values(ID)(LANGUAGE)
                foreach (var currentID in foundModInfo.Values)
                {
                    //for each language
                    foreach (var currentLanguage in currentID.Value)
                    {
                        if (UpdateOrInsertValue(xdoc, currentID.Key, currentLanguage.Language, currentLanguage.Value))
                        {
                            changed = true;
                        }
                    }
                }


                // Now check if someone deleted a row
                var keysInXml = xdoc.Descendants().Where(x => x.Name == KEY_NAME);
                foreach (var currentKeyElement in keysInXml.ToList())
                {
                    var currentKeyId = currentKeyElement.Attribute(ID_NAME).Value;
                    if (foundModInfo.Values.Keys.Contains(currentKeyId))
                    {
                        // hmm, forgot what this was for
                        var a = true;
                    }
                    else
                    {
                        currentKeyElement.Remove();
                        changed = true;
                    }
                }

                if (changed)
                {
                    var xmlSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "    "
                    };

                    var configHelper = new ConfigHelper();
                    var settings = configHelper.GetSettings();

                    if (settings != null)
                    {
                        if (settings.IndentationSettings == IndentationSettings.Spaces)
                        {
                            var indentChars = "";

                            for (var i = 0; i < settings.TabSize; i++)
                            {
                                indentChars += " ";
                            }

                            xmlSettings.IndentChars = indentChars;
                        }

                        if (settings.IndentationSettings == IndentationSettings.Tabs)
                        {
                            xmlSettings.IndentChars = "\t";
                        }


                        if (settings.RemoveEmptyNodes)
                        {
                            xdoc.Descendants().Where(d => d.IsEmpty || string.IsNullOrWhiteSpace(d.Value)).Remove();
                        }
                    }


                    using (var writer = XmlWriter.Create(currentFileInfo.FullName, xmlSettings))
                    {
                        xdoc.Save(writer);
                    }

                    File.AppendAllText(currentFileInfo.FullName, Environment.NewLine);
                }
            });
        }


        private bool UpdateOrInsertValue(XDocument xdoc, string id, string language, string value)
        {
            var changed = false;
            //get keys
            var keys = (from xel in xdoc.Descendants() where xel.Name.ToString().ToLowerInvariant() == KEY_NAME.ToLowerInvariant() select xel).ToList();

            XElement parent;

            if (keys.Any())
            {
                parent = keys.FirstOrDefault().Parent;
            }
            else
            {
                parent = (from xel in xdoc.Descendants() where xel.Name.ToString().ToLowerInvariant() == PACKAGE_NAME.ToLowerInvariant() select xel).FirstOrDefault();
            }

            //get ids
            var xID = (from xel in keys where xel.Attributes().Any(x => x.Value.ToString().ToLowerInvariant() == id.ToLowerInvariant()) select xel).FirstOrDefault();

            //get language


            if (xID == null)
            {
                //new create
                //Too tired to make that in 1 single block :D
                var xelKeyNew = new XElement(KEY_NAME);
                xelKeyNew.Add(new XAttribute(ID_NAME, id));

                parent.Add(xelKeyNew);
                xID = xelKeyNew;
            }


            var xLanguage = (from xel in xID.Descendants() where xel.Name.ToString().ToLowerInvariant() == language.ToLowerInvariant() select xel).FirstOrDefault();


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