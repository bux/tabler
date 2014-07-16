using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabler
{
    public class FileSystemHelper
    {
        private List<string> _allStringTablePaths = new List<string>();
        private const string STRINGTABLE_NAME= "stringtable.xml";

        public List<string> GetAllStringTablePaths(string path)
        {
            var di = new DirectoryInfo(path);

            FindStringTable(di);

            return _allStringTablePaths;
        }

        private void FindStringTable(DirectoryInfo parentDirectory)
        {

            // recursive function
            if (parentDirectory == null)
            {
                return;
            }

            // iterate through all subfolders
            foreach (DirectoryInfo directoryInfo in parentDirectory.GetDirectories())
            {
                if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden) == false || directoryInfo.Attributes.HasFlag(FileAttributes.System) == false)
                {
                    FindStringTable(directoryInfo);
                }
            }

            // iterate through all files
            foreach (FileInfo fileInfo in parentDirectory.GetFiles())
            {

                if (fileInfo.Name.ToLowerInvariant().Equals(STRINGTABLE_NAME))
                {
                    _allStringTablePaths.Add(fileInfo.FullName);
                }
            }
        }
    }
}
