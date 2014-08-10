using System.Collections.Generic;
using System.Linq;

namespace tabler
{
    public class TranslationComponents
    {
        private List<LanguageStatistics> m_statistics;
        public List<ModInfoContainer> AllModInfo { get; set; }

        public List<string> Headers { get; set; }

        public List<LanguageStatistics> Statistics
        {
            get
            {
                if (m_statistics == null)
                {
                    m_statistics = new List<LanguageStatistics>();
                }
                return m_statistics;
            }
            set { m_statistics = value; }
        }

        public int KeyCount
        {
            get
            {
                int count = 0;
                if (AllModInfo != null)
                {
                    count = AllModInfo.Sum(mi => mi.Values.Count);
                }
                return count;
            }
        }
    }
}