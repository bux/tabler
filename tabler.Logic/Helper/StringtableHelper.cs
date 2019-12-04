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
                stringtables.Add(ParseStringtable(currentFile).Result);
            });

            return stringtables.ToList();
        }

        public static async Task<Stringtable> ParseStringtable(FileInfo stringtableFile)
        {
            var stringtable = await ParseXmlFile(stringtableFile);
            ValidateStringtable(stringtable);
            return stringtable;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private static async Task<Stringtable> ParseXmlFile(FileInfo fileInfo)
        {
            var stringtable = new Stringtable
            {
                File = fileInfo,
                Name = fileInfo.Directory.Name
            };

            var res = await new TaskFactory<Stringtable>().StartNew(
                () =>
                {

                    try
                    {
                        var ser = new XmlSerializer(typeof(Project));

                        using (var sr = new StreamReader(fileInfo.FullName))
                        {
                            stringtable.FileHasBom = FileHelper.FileHasBom(sr.BaseStream);

                            using (var reader = XmlReader.Create(sr.BaseStream, XmlReaderSettings))
                            {
                                stringtable.Project = (Project)ser.Deserialize(reader);
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

                });

            return res;
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

            var xmlSerializer = new XmlSerializer(typeof(Project));

            using (var writer = XmlWriter.Create(currentFileInfo.FullName, xmlSettings))
            {
                xmlSerializer.Serialize(writer, currentStringtable.Project, dummyNamespace);

            }

            File.AppendAllText(currentFileInfo.FullName, Environment.NewLine);

            currentStringtable.HasChanges = false;

        }

        public static void SaveStringTableFile(FileInfo currentFileInfo, Stringtable currentStringtable)
        {
            SaveStringTableFile(currentFileInfo, currentStringtable, null);

        }


    }
}
