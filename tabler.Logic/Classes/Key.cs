using System.Xml.Serialization;

namespace tabler.Logic.Classes {
    public class Key {

        [XmlAttribute("ID")]
        public string Id { get; set; }

        public string Original { get; set; }

        public string English { get; set; }

        public string German { get; set; }

        public string French { get; set; }

        public string Italian { get; set; }

        public string Spanish { get; set; }

        public string Portuguese { get; set; }

        public string Polish { get; set; }

        public string Czech { get; set; }

        public string Hungarian { get; set; }

        public string Russian { get; set; }

        public string Turkish { get; set; }

        public string Japanese { get; set; }

        public string Chinesesimp { get; set; }

        public string Chinese { get; set; }

        public string Korean { get; set; }
    }
}
