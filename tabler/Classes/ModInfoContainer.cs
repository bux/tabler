using System.Collections.Generic;
using System.IO;
using tabler.Classes;

namespace tabler {
    public class ModInfoContainer {
        //Values(ID)(LANGUAGE)
        public Dictionary<string, List<XmlKeyTranslation>> Values = new Dictionary<string, List<XmlKeyTranslation>>();

        public string Name { get; set; }
        public FileInfo FileInfoStringTable { get; set; }
    }
}