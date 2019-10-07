using Polenter.Serialization;
using Polenter.Serialization.Advanced.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using tabler.Logic.Classes;
using tabler.Logic.Exceptions;

namespace tabler.Logic.Helper
{
    public static class StringtableHelper
    {
        private static XmlReaderSettings _xmlReaderSettings;

        public static XmlReaderSettings XmlReaderSettings
        {
            get
            {
                if (_xmlReaderSettings == null)
                {
                    var settings = new XmlReaderSettings
                    {
                        ValidationType = ValidationType.Schema,
                        DtdProcessing = DtdProcessing.Parse //it did not allow to parse on my machine, so i had to enable this
                    };
                    settings.Schemas.Add(null, "xml\\stringtable.xsd");
                    _xmlReaderSettings = settings;
                }
                return _xmlReaderSettings;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allStringtableFiles"></param>
        public static IEnumerable<Stringtable> ParseStringtables(IEnumerable<FileInfo> allStringtableFiles)
        {
            var stringtables = new System.Collections.Concurrent.ConcurrentBag<Stringtable>();
            Parallel.ForEach(allStringtableFiles, currentFile =>
            {
                stringtables.Add(ParseStringtable(currentFile));
            });

            return stringtables.ToList();
        }
        public static Stringtable ParseStringtable(FileInfo stringtableFile)
        {

            var stringtable = ParseXmlFile(stringtableFile);
            ValidateStringtable(stringtable);
            return stringtable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private static Stringtable ParseXmlFile(FileInfo fileInfo)
        {
            var stringtable = new Stringtable
            {
                File = fileInfo,
                Name = fileInfo.Directory.Name
            };

            try
            {
                var ser = new XmlSerializer(typeof(Project));

                using (var sr = new StreamReader(fileInfo.FullName))
                {
                    stringtable.FileHasBom = FileHelper.FileHasBom(sr.BaseStream);

                    using (var reader = XmlReader.Create(sr.BaseStream, XmlReaderSettings))
                    {
                        stringtable.Project = (Project)ser.Deserialize(reader);
                        foreach (var package in stringtable.Project.Packages)
                        {
                            foreach (var item in package.Keys)
                            {
                                item.PackageName = package.Name;
                            }
                            foreach (var container in package.Containers)
                            {
                                foreach (var item in package.Keys)
                                {
                                    item.PackageName = package.Name;
                                    item.ContainerName = container.Name;
                                }
                            }
                        }
                    }
                }
            }
            catch (XmlException ex)
            {
                throw new GenericXmlException("", fileInfo.FullName, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                var message = new StringBuilder();
                message.Append(ex.Message);
                if (ex.InnerException != null)
                {
                    message.AppendLine().Append(ex.InnerException.Message);
                }

                throw new MalformedStringtableException(fileInfo.FullName, message.ToString());
            }

            return stringtable;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringtable"></param>
        private static void ValidateStringtable(Stringtable stringtable)
        {
            var duplicates = stringtable.AllKeys.GroupBy(k => k.Id).Where(g => g.Skip(1).Any()).ToList();
            if (duplicates.Any())
            {
                var keys = duplicates.Select(g => g.Key).Distinct().ToList();
                var keyNames = string.Join(", ", keys);
                throw new DuplicateKeyException(keyNames, stringtable.File.FullName, keyNames);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesByNameInDirectory"></param>
        /// <param name="lstStringtables"></param>
        public static void SaveStringtableFiles(List<FileInfo> filesByNameInDirectory, IEnumerable<Stringtable> lstStringtables)
        {
            Parallel.ForEach(filesByNameInDirectory, currentFileInfo =>
            {
                var currentStringtable = lstStringtables.FirstOrDefault(x => string.Equals(x.Name.ToLowerInvariant(), currentFileInfo.Directory.Name.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase));

                SaveStringTableFile(currentFileInfo, currentStringtable);
            });
        }

        public static void SaveStringTableFile(FileInfo currentFileInfo, Stringtable currentStringtable, List<ItemSelected> header)
        {
            if (currentStringtable == null)
            {
                return;
            }

            if (!currentStringtable.HasChanges)
            {
                return;
            }

            var dummyNamespace = new XmlSerializerNamespaces();
            dummyNamespace.Add("", "");

            var xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                Encoding = new UTF8Encoding(currentStringtable.FileHasBom)
            };

            if (ConfigHelper.CurrentSettings.IndentationSettings == IndentationSettings.Spaces)
            {
                var indentChars = "";

                for (var i = 0; i < ConfigHelper.CurrentSettings.TabSize; i++)
                {
                    indentChars += " ";
                }

                xmlSettings.IndentChars = indentChars;
            }

            if (ConfigHelper.CurrentSettings.IndentationSettings == IndentationSettings.Tabs)
            {
                xmlSettings.IndentChars = "\t";
            }
          
            XDocument srcTree = new XDocument( new XDeclaration("1.0","utf-8","true"),currentStringtable.Project.AsXelement(true, header.Where(x => x.IsSelected).Select(x=> x.Key).ToList()));

            var xmlSerializer = new XmlSerializer(typeof(Project));

            using (var writer = XmlWriter.Create(currentFileInfo.FullName, xmlSettings))
            {
                //xmlSerializer.Serialize(writer, currentStringtable.Project, dummyNamespace);
                srcTree.Save(writer);
            }

            File.AppendAllText(currentFileInfo.FullName, Environment.NewLine);

            currentStringtable.HasChanges = false;

        }

        private class Conv : ISimpleValueConverter
        {
            public object ConvertFromString(string text, Type type)
            {
                throw new NotImplementedException();
            }

            public string ConvertToString(object value)
            {
                throw new NotImplementedException();
            }
        }

        public static void SaveStringTableFile(FileInfo currentFileInfo, Stringtable currentStringtable)
        {
            SaveStringTableFile(currentFileInfo, currentStringtable, null);



        }


    }
}
