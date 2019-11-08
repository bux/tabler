using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;
using tabler.Logic.Extensions;

namespace tabler.Logic.Classes
{
    [DataContract]
    [Serializable] //for copy paste action
    public class Key
    {
        [DataMember]
        [XmlAttribute("ID")]
        public string Id { get; set; }

        [DataMember]
        public string Original { get; set; }

        [DataMember]
        public string English { get; set; }

        [DataMember]
        public string German { get; set; }

        [DataMember]
        public string French { get; set; }

        [DataMember]
        public string Italian { get; set; }

        [DataMember]
        public string Spanish { get; set; }

        [DataMember]
        public string Portuguese { get; set; }

        [DataMember]
        public string Polish { get; set; }

        [DataMember]
        public string Czech { get; set; }

        [DataMember]
        public string Hungarian { get; set; }

        [DataMember]
        public string Russian { get; set; }

        [DataMember]
        public string Turkish { get; set; }

        [DataMember]
        public string Japanese { get; set; }

        [DataMember]
        public string Chinesesimp { get; set; }

        [DataMember]
        public string Chinese { get; set; }

        [DataMember]
        public string Korean { get; set; }

        //[IgnoreDataMember]
        //[XmlIgnore]
        //[SoapIgnore]
        //public string PackageName { get; set; }
        //[IgnoreDataMember]
        //[XmlIgnore]
        //[SoapIgnore]
        //public string ContainerName { get; set; }

        //[IgnoreDataMember]
        //[XmlIgnore]
        //[SoapIgnore]
        //public bool IsComplete { get; set; }

        //[IgnoreDataMember]
        //[XmlIgnore]
        //[SoapIgnore]
        //public string IsCompleteName { get { return IsComplete ? "Complete translation" : "Incomplete translation"; } }

        //private bool IsLanguageComplete(string nameofProp, string valuesOfProp, List<string> languagesToHave)
        //{
        //    if (languagesToHave.Any(x => x == nameofProp))
        //    {
        //        if (string.IsNullOrEmpty(valuesOfProp))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public void UpdateIsComplete(List<string> languagesToHave)
        //{
        //    if (!IsLanguageComplete(nameof(English), English, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(German), German, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(French), French, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Italian), Italian, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Spanish), Spanish, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Portuguese), Portuguese, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Polish), Polish, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Czech), Czech, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Hungarian), Hungarian, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };

        //    if (!IsLanguageComplete(nameof(Russian), Russian, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };

        //    if (!IsLanguageComplete(nameof(Turkish), Turkish, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };

        //    if (!IsLanguageComplete(nameof(Japanese), Japanese, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };

        //    if (!IsLanguageComplete(nameof(Chinesesimp), Chinesesimp, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };

        //    if (!IsLanguageComplete(nameof(Chinese), Chinese, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
        //    if (!IsLanguageComplete(nameof(Korean), Korean, languagesToHave))
        //    {
        //        IsComplete = false;
        //        return;
        //    };
     
        //    IsComplete = true;
        //}

        //public bool ContainsText(string value, bool searchOnlyInId, bool ignoreCase)
        //{
        //    var comp = StringComparison.InvariantCulture;

        //    if (ignoreCase)
        //    {
        //        comp = StringComparison.InvariantCultureIgnoreCase;
        //    }
            
        //    if (Id.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }

        //    if (searchOnlyInId)
        //    {
        //        return false;
        //    }

        //    if (English.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (German.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (French.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Italian.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Spanish.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Portuguese.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Polish.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Czech.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Hungarian.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Russian.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Turkish.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Japanese.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Chinesesimp.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Chinese.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }
        //    if (Korean.ContainsEx(value, comp))
        //    {
        //        return true;
        //    }

        //    return false;
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
