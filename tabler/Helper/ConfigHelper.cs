using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace tabler
{
    public class ConfigHelper
    {
        private const string LASTPATHTODATAFILES_NAME = "LastPathToDataFiles";
        private const string INDENTATION_NAME = "Indentation";
        private const string TABSIZE_NAME = "TabSize";
        private const string EMPTYNODES_NAME = "RemoveEmptyNodes";

        private readonly FileInfo _fiConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\config.xml"));
        private XDocument _xDocConfig;

        public ConfigHelper()
        {
            //create dir
            if (_fiConfig.Directory != null && _fiConfig.Directory.Exists == false)
            {
                _fiConfig.Directory.Create();
            }
        }


        private XDocument CreateOrLoadConfig(bool forceCreation)
        {
            if (_xDocConfig != null && forceCreation == false)
            {
                return _xDocConfig;
            }

            if (_fiConfig.Exists && forceCreation == false)
            {
                _xDocConfig = XDocument.Load(_fiConfig.FullName);
            }
            else
            {
                var path = new XElement(LASTPATHTODATAFILES_NAME);
                var indent = new XElement(INDENTATION_NAME, 0);
                var tabsize = new XElement(TABSIZE_NAME, 4);
                var removeEmptyNodes = new XElement(EMPTYNODES_NAME, true);

                var lstElements = new List<XElement> {path, indent, tabsize, removeEmptyNodes};

                _xDocConfig = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Config file"),
                    new XElement("config", lstElements.ToArray()));

                SaveConfigXml();

                return _xDocConfig;
            }

            return null;
        }

        public void SetLastPathOfDataFiles(DirectoryInfo path)
        {
            if (path == null || path.Exists == false)
            {
                return;
            }

            if (_xDocConfig == null)
            {
                CreateOrLoadConfig(false);
            }


            if (_xDocConfig != null)
            {
                var pathElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == LASTPATHTODATAFILES_NAME);

                if (pathElement != null)
                {
                    pathElement.Value = XmlConvert.EncodeName(path.FullName);
                }
            }

            SaveConfigXml();
        }

        public DirectoryInfo GetLastPathOfDataFiles()
        {
            if (_xDocConfig == null)
            {
                CreateOrLoadConfig(false);
            }


            if (_xDocConfig != null)
            {
                var pathElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == LASTPATHTODATAFILES_NAME);

                if (pathElement != null)
                {
                    var value = XmlConvert.DecodeName(pathElement.Value);
                    if (string.IsNullOrEmpty(value) == false)
                    {
                        return new DirectoryInfo(value);
                    }
                }
                else
                {
                    CreateOrLoadConfig(true);
                }
            }

            return null;
        }

        /// <summary>
        ///     Saves the xml to the file system
        /// </summary>
        private void SaveConfigXml()
        {
            if (_xDocConfig != null)
            {
                _xDocConfig.Save(_fiConfig.FullName);
            }
        }

        /// <summary>
        ///     Read Settings from config file
        /// </summary>
        /// <returns></returns>
        public Settings GetSettings()
        {
            if (_xDocConfig == null)
            {
                CreateOrLoadConfig(false);
            }

            if (_xDocConfig == null)
            {
                return null;
            }

            var loadedSettings = new Settings();

            var indentElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == INDENTATION_NAME);
            if (indentElement != null)
            {
                try
                {
                    loadedSettings.IndentationSettings = (IndentationSettings) Enum.Parse(typeof(IndentationSettings), indentElement.Value);
                }
                catch (Exception)
                {
                    // in case of error, use the better method
                    loadedSettings.IndentationSettings = IndentationSettings.Spaces;
                }
            }

            var tabSizeElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == TABSIZE_NAME);
            if (tabSizeElement != null)
            {
                loadedSettings.TabSize = int.Parse(tabSizeElement.Value);
            }

            var removeEmptyNodes = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == EMPTYNODES_NAME);
            if (removeEmptyNodes != null)
            {
                loadedSettings.RemoveEmptyNodes = bool.Parse(removeEmptyNodes.Value);
            }

            return loadedSettings;
        }

        /// <summary>
        ///     Read Settings from config file
        /// </summary>
        /// <returns></returns>
        public bool SaveSettings(Settings settingsToSave)
        {
            if (settingsToSave == null)
            {
                return false;
            }

            if (_xDocConfig == null)
            {
                CreateOrLoadConfig(false);
            }

            if (_xDocConfig != null)
            {
                var indentElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == INDENTATION_NAME);
                if (indentElement != null)
                {
                    indentElement.Value = settingsToSave.IndentationSettings.ToString();
                }
                else
                {
                    CreateOrLoadConfig(true);
                }

                var tabSizeElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == TABSIZE_NAME);
                if (tabSizeElement != null)
                {
                    tabSizeElement.Value = settingsToSave.TabSize.ToString();
                }
                else
                {
                    CreateOrLoadConfig(true);
                }

                var removeEmptyNodes = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == EMPTYNODES_NAME);
                if (removeEmptyNodes != null)
                {
                    removeEmptyNodes.Value = settingsToSave.RemoveEmptyNodes.ToString();
                }
                else
                {
                    CreateOrLoadConfig(true);
                }

                SaveConfigXml();
                return true;
            }

            return false;
        }
    }
}
