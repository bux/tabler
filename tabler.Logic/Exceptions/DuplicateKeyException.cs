using System;

namespace tabler.Logic.Exceptions
{
    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string keyName)
        {
            KeyName = keyName;
        }

        public DuplicateKeyException(string keyName, string fileName)
        {
            KeyName = keyName;
            FileName = fileName;
        }

        public DuplicateKeyException(string keyName, string fileName, string entryName)
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
