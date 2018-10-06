using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tabler.Helper
{
    public class FileSystemHelper
    {
        public static List<FileInfo> GetFilesByNameInDirectory(DirectoryInfo di, string fileName, SearchOption searchOption)
        {
            var allStringTablePaths = di.GetFiles(fileName, searchOption).ToList();
            return allStringTablePaths;
        }
    }
}
