using System.Collections.Generic;

namespace tabler
{
    public class LanguageStatistics
    {
        public string LanguageName { get; set; }
        public Dictionary<string, int> MissingModStrings { get; set; }
    }
}
