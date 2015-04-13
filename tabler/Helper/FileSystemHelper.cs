using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace tabler {
    public class FileSystemHelper {
        public static List<FileInfo> GetFilesByNameInDirectory(DirectoryInfo di, string fileName, SearchOption searchOption) {
            List<FileInfo> allStringTablePaths = di.GetFiles(fileName, searchOption).ToList();
            return allStringTablePaths;
        }
    }
}