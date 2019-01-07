using System.Collections.Generic;
using System.Xml.Serialization;

namespace tabler.Logic.Classes {
    public class Package {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Container")]
        public List<Container> Containers { get; set; }

        [XmlElement("Key")]
        public List<Key> Keys { get; set; }
    }
}
