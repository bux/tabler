using System.Collections.Generic;
using System.IO;

namespace tabler {
    public class ModInfoContainer {
        //Values(ID)(LANGUAGE)
        public Dictionary<string, Dictionary<string, string>> Values = new Dictionary<string, Dictionary<string, string>>();

        public string Name { get; set; }
        public FileInfo FileInfoStringTable { get; set; }
    }
}