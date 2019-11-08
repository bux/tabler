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

        //public XElement AsXelement(bool ignoreEmptyValues, List<string> languagesToWrite)
        //{
        //    return new XElement("Project", new XAttribute("name", Name), Packages.Select(x => x.AsXElement(ignoreEmptyValues, languagesToWrite)).ToList());
        //}
        //internal XElement AsXElement(bool ignoreEmptyValues, List<string> languagesToWrite)
        //{
        //    return new XElement("Package", new XAttribute("name", Name), Keys.Select(x => x.AsXElement(ignoreEmptyValues, languagesToWrite)).ToList());
        //}
        //internal XElement AsXElement(bool ignoreEmptyValues, List<string> languagesToWrite)
        //{
        //    var res= new XElement("Key", new XAttribute("ID", Id));

        //    AddIfNecessary(res,"English", English, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "German", German, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Italian", Italian, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Spanish", Spanish, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Portuguese", Portuguese, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Polish", Polish, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Czech", Czech, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Hungarian", Hungarian, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Russian", Russian, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Turkish", Turkish, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Japanese", Japanese, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Chinesesimp", Chinesesimp, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Chinese", Chinese, ignoreEmptyValues, languagesToWrite);
        //    AddIfNecessary(res, "Korean", Korean, ignoreEmptyValues, languagesToWrite);

        //    return res;
        //}

        //private void AddIfNecessary(XElement parent,string nameOfKey , string valueOfKey, bool ignoreEmptyValues, List<string> languagesToWrite)
        //{
        //    if (ignoreEmptyValues && string.IsNullOrEmpty(valueOfKey))
        //    {
        //        return;
        //    }

        //    if (!languagesToWrite.Contains(nameOfKey))
        //    {
        //        return;
        //    }

        //    parent.Add(new XElement(nameOfKey, valueOfKey));



        //}

    }
}
