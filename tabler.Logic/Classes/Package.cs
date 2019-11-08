using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace tabler.Logic.Classes {
    public class Package {

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Container")]
        public List<Container> Containers { get; set; }

        [XmlElement("Key")]
        public List<Key> Keys { get; set; }

        //internal XElement AsXElement(bool ignoreEmptyValues, List<string> languagesToWrite)
        //{
        //    return new XElement("Package", new XAttribute("name", Name), Keys.Select(x => x.AsXElement( ignoreEmptyValues, languagesToWrite)).ToList());
        //}
    }
}
