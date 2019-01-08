using System;

namespace tabler.Logic.Exceptions {
    public class MalformedStringtableException : Exception {
        public string FileName { get; set; }

        public new string Message { get; set; }

        public MalformedStringtableException(string fileName, string message) {
            FileName = fileName;
            Message = message;
        }
    }
}
