using System.Collections.Generic;
using System.Xml.Serialization;

namespace tabler.Logic.Classes {

    [XmlRoot("Project")]
    public class Project {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Package")]
        public List<Package> Packages { get; set; }
    }
}
