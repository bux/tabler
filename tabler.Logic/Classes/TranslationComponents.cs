using System;
using System.Collections.Generic;
using System.Linq;

namespace tabler.Logic.Classes
{
    public class TranslationComponents
    {
        private List<LanguageStatistics> _statistics;

        public List<string> Headers { get; set; }

        public IEnumerable<Stringtable> Stringtables { get; set; }

        public List<LanguageStatistics> Statistics
        {
            get
            {
                if (_statistics == null)
                {
                    _statistics = new List<LanguageStatistics>();
                }

                return _statistics;
            }
            set => _statistics = value;
        }

        public int KeyCount
        {
            get
            {
                return Stringtables.Sum(stringtable => stringtable.AllKeys.Count());
            }
        }
    }
}
