using System.Collections.Generic;
using System.Linq;


namespace tabler {

    public class TranslationComponents {

        private List<LanguageStatistics> _statistics;

        public List<ModInfoContainer> AllModInfo { get; set; }

        public List<string> Headers { get; set; }

        public List<LanguageStatistics> Statistics {
            get {
                if (_statistics == null) {
                    _statistics = new List<LanguageStatistics>();
                }
                return _statistics;
            }
            set { _statistics = value; }
        }

        public int KeyCount {
            get {
                int count = 0;
                if (AllModInfo != null) {
                    count = AllModInfo.Sum(mi => mi.Values.Count);
                }
                return count;
            }
        }
    }

}