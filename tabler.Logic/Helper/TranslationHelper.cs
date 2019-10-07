using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using tabler.Logic.Classes;
using tabler.Logic.Enums;

namespace tabler.Logic.Helper
{
    public class TranslationHelper
    {
        public const string COLUMN_MODNAME = "Mod";
        public const string COLUMN_IDNAME = "ID";
        public const string STRINGTABLE_NAME = "stringtable.xml";

        private readonly FileInfo _fiExcelFile;
        public TranslationComponents TranslationComponents;

        public static List<string> GetHeaders(Stringtable stringtable)
        {

            var headers = new List<string>();
            // Get list of all used languages
            var allKeys = stringtable.Project.Packages.SelectMany(p =>
            {
                var keys = new List<Key>();

                if (p.Containers.Any())
                {
                    keys.AddRange(p.Containers.SelectMany(c => c.Keys));
                }

                keys.AddRange(p.Keys);

                return keys;
            });

            var keyPropertyInfos = typeof(Key).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var key in allKeys)
            {
                foreach (var property in keyPropertyInfos)
                {
                    if (property.Name.ToLowerInvariant() == COLUMN_IDNAME.ToLowerInvariant() || headers.Contains(property.Name))
                    {
                        continue;
                    }

                    var value = property.GetValue(key, null)?.ToString();
                    if (value != null && !string.IsNullOrEmpty(value))
                    {
                        headers.Add(property.Name);
                    }
                }
            }

            headers = headers.OrderBy(l => l).ToList();

            var hasOriginal = false;

            if (headers.Any(x => x.ToLowerInvariant() == Languages.Original.ToString().ToLowerInvariant()))
            {
                hasOriginal = true;
                headers.Remove(Languages.Original.ToString());
                headers.Insert(0, Languages.Original.ToString());
            }

            if (headers.Any(x => x.ToLowerInvariant() == Languages.English.ToString().ToLowerInvariant()))
            {
                headers.Remove(Languages.English.ToString());
                headers.Insert(hasOriginal ? 1 : 0, Languages.English.ToString());
            }

            headers.Insert(0, COLUMN_IDNAME);

            return headers;
        }
        public static List<string> GetHeaders(IEnumerable<Stringtable> stringtables)
        {

            var headers = new List<string>();
            // Get list of all used languages
            var allKeys = stringtables.SelectMany(s => s.Project.Packages).SelectMany(p =>
            {
                var keys = new List<Key>();

                if (p.Containers.Any())
                {
                    keys.AddRange(p.Containers.SelectMany(c => c.Keys));
                }

                keys.AddRange(p.Keys);

                return keys;
            });

            var keyPropertyInfos = typeof(Key).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var key in allKeys)
            {
                foreach (var property in keyPropertyInfos)
                {
                    if (property.Name.ToLowerInvariant() == COLUMN_IDNAME.ToLowerInvariant() || headers.Contains(property.Name))
                    {
                        continue;
                    }

                    var value = property.GetValue(key, null)?.ToString();
                    if (value != null && !string.IsNullOrEmpty(value))
                    {
                        headers.Add(property.Name);
                    }
                }
            }

            headers = headers.OrderBy(l => l).ToList();

            var hasOriginal = false;

            if (headers.Any(x => x.ToLowerInvariant() == Languages.Original.ToString().ToLowerInvariant()))
            {
                hasOriginal = true;
                headers.Remove(Languages.Original.ToString());
                headers.Insert(0, Languages.Original.ToString());
            }

            if (headers.Any(x => x.ToLowerInvariant() == Languages.English.ToString().ToLowerInvariant()))
            {
                headers.Remove(Languages.English.ToString());
                headers.Insert(hasOriginal ? 1 : 0, Languages.English.ToString());
            }

            headers.Insert(0, COLUMN_IDNAME);

            return headers;
        }


        private TranslationComponents GetTranslationComponents(DirectoryInfo lastPathToDataFiles)
        {
            var allStringtableFiles = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories).ToList();

            if (allStringtableFiles.Any() == false)
            {
                return null;
            }

           
            var transComp = new TranslationComponents
            {
                Stringtables = StringtableHelper.ParseStringtables(allStringtableFiles)
            };

            transComp.Headers = GetHeaders(transComp.Stringtables);

            TranslationComponents = transComp;
            return TranslationComponents;
        }


        private static bool SaveModInfosToXml(DirectoryInfo lastPathToDataFiles, IEnumerable<Stringtable> lstStringtables)
        {
            //if going through mods instead of files, 
            // we could create files
            // too tired :D ->  TODO
            var filesByNameInDirectory = FileSystemHelper.GetFilesByNameInDirectory(lastPathToDataFiles, STRINGTABLE_NAME, SearchOption.AllDirectories);

            StringtableHelper.SaveStringtableFiles(filesByNameInDirectory, lstStringtables);

            return true;
        }


        public TranslationComponents GetGridData(DirectoryInfo lastPathToDataFiles)
        {
            return GetTranslationComponents(lastPathToDataFiles);
        }

        //public bool SaveGridData(DirectoryInfo lastPathToDataFiles, List<ModInfoContainer> lstModInfos)
        //{
        //    return SaveModInfosToXml(lastPathToDataFiles, lstModInfos);
        //}

        public bool SaveGridData(DirectoryInfo lastPathToDataFiles, IEnumerable<Stringtable> lstStringtables)
        {
            return SaveModInfosToXml(lastPathToDataFiles, lstStringtables);
        }
    }
}
