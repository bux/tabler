using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace tabler.Logic.Classes {

    [XmlRoot("Project")]
    public class Project {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Package")]
        public List<Package> Packages { get; set; }

        public XElement AsXelement(bool ignoreEmptyValues, List<string> languagesToWrite)
        {
            return new XElement("Project", new XAttribute("name", Name), Packages.Select(x=> x.AsXElement(ignoreEmptyValues, languagesToWrite)).ToList());
        }

    }
}
