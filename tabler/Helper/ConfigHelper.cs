using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace tabler
{
    public class ConfigHelper
    {
        private const string LASTPATHTODATAFILES = "LastPathToDataFiles";
        private readonly FileInfo _fiConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\config.xml"));
        private XDocument _xDocConfig;

        public ConfigHelper()
        {
            //create dir
            if (_fiConfig.Directory.Exists == false)
            {
                _fiConfig.Directory.Create();
            }
        }


        public void CreateOrLoadConfig()
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

                    var lstElements = new List<XElement>();
                    lstElements.Add(path);

                    _xDocConfig = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"), 
                        new XComment("Config file"), 
                        new XElement("config", lstElements.ToArray()));

                    _xDocConfig.Save(_fiConfig.FullName);
                    SaveConfigXML();
                }
            }
        }

        public void SetLastPathOfDataFiles(DirectoryInfo path)
        {
            if (path == null || path.Exists == false)
            {
                return;
            }

            if (_xDocConfig == null)
            {
                CreateOrLoadConfig();
            }


            if (_xDocConfig != null)
            {
                XElement pathElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == LASTPATHTODATAFILES);

                if (pathElement != null)
                {
                    pathElement.Value = XmlConvert.EncodeName(path.FullName);
                }
            }

            SaveConfigXML();
        }

        public DirectoryInfo GetLastPathOfDataFiles()
        {
            if (_xDocConfig == null)
            {
                CreateOrLoadConfig();
            }


            if (_xDocConfig != null)
            {
                XElement pathElement = _xDocConfig.Descendants().FirstOrDefault(d => d.Name == LASTPATHTODATAFILES);

                if (pathElement != null)
                {
                    string value = XmlConvert.DecodeName(pathElement.Value);
                    if (String.IsNullOrEmpty(value) == false)
                    {
                        return new DirectoryInfo(value);
                    }
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
    }
}