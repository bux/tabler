using System.Collections.Generic;
using System.Xml.Serialization;

namespace tabler.Logic.Classes {
    public class Container {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Key")]
        public List<Key> Keys { get; set; }
    }
}
