using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using tabler.Logic.Classes;
using tabler.Logic.Exceptions;

namespace tabler.Logic.Helper
{
    public class StringtableHelper
    {
        private XmlReaderSettings _xmlReaderSettings;

        public XmlReaderSettings XmlReaderSettings
        {
            get {
                if (_xmlReaderSettings == null)
                {
                    var settings = new XmlReaderSettings
                    {
                        ValidationType = ValidationType.Schema
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
        public IEnumerable<Stringtable> ParseStringtables(IEnumerable<FileInfo> allStringtableFiles)
        {
            var stringtables = new System.Collections.Concurrent.ConcurrentBag<Stringtable>();
            Parallel.ForEach(allStringtableFiles, currentFile =>
            {
                stringtables.Add(ParseXmlFile(currentFile));
            });

            return stringtables.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private Stringtable ParseXmlFile(FileInfo fileInfo)
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
                        stringtable.Project = (Project) ser.Deserialize(reader);
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
        /// <param name="filesByNameInDirectory"></param>
        /// <param name="lstStringtables"></param>
        public void SaveStringtableFiles(List<FileInfo> filesByNameInDirectory, IEnumerable<Stringtable> lstStringtables)
        {
            var configHelper = new ConfigHelper();
            var settings = configHelper.GetSettings();

            var dummyNamespace = new XmlSerializerNamespaces();
            dummyNamespace.Add("","");

            Parallel.ForEach(filesByNameInDirectory, currentFileInfo =>
            {
                var currentStringtable = lstStringtables.FirstOrDefault(x => string.Equals(x.Name.ToLowerInvariant(), currentFileInfo.Directory.Name.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase));

                if (currentStringtable == null)
                {
                    return;
                }

                if (!currentStringtable.HasChanges)
                {
                    return;
                }


                var xmlSettings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ",
                    Encoding = new UTF8Encoding(currentStringtable.FileHasBom)
                };

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

                    // TODO Remove empty nodes

                }

                var xmlSerializer = new XmlSerializer(typeof(Project));
                using (var writer = XmlWriter.Create(currentFileInfo.FullName, xmlSettings))
                {
                    xmlSerializer.Serialize(writer, currentStringtable.Project,dummyNamespace);
                }

                File.AppendAllText(currentFileInfo.FullName, Environment.NewLine);
            });
        }
    }
}
