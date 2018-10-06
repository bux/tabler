using System;

namespace tabler
{
    public class GenericXmlException : Exception
    {
        public GenericXmlException(string keyName)
        {
            KeyName = keyName;
        }

        public GenericXmlException(string keyName, string fileName)
        {
            KeyName = keyName;
            FileName = fileName;
        }

        public GenericXmlException(string keyName, string fileName, string entryName)
        {
            KeyName = keyName;
            FileName = fileName;
            EntryName = entryName;
        }

        public string KeyName { get; set; }
        public string FileName { get; set; }
        public string EntryName { get; set; }
    }
}
