using System;
using System.Runtime.Serialization;

namespace tabler
{
    [Serializable]
    public class DuplicateKeyException : Exception, ISerializable
    {
        public string KeyName { get; set; }
        public string FileName { get; set; }

        public DuplicateKeyException(string keyName)
        {
            KeyName = keyName;
        }
        public DuplicateKeyException(string keyName, string fileName)
        {
            KeyName = keyName;
            FileName = fileName;
        }
    }
}