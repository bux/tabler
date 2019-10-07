using System;
using System.Collections.Generic;
using tabler.wpf.Helper;

namespace tabler.wpf.Container
{
    public class VersionInfoContainer
    {

        public VersionInfoContainer()
        {
            Fixes = new List<string>();
            Features = new List<string>();
            SpecialThanks = new List<string>();
        }

        public DateTime ReleaseDate { get; set; }

        public string ReleaseDateStr => ReleaseDate.GetDateTimeString_yyyyMMddhhmmss();


        public string Version { get; set; }
        public List<string> Fixes { get; set; }
        public List<string> Features { get; set; }
        public List<string> SpecialThanks { get; set; }

        public string FixesStr => string.Join(Environment.NewLine, Fixes);

        public string FeaturesStr => string.Join(Environment.NewLine, Features);

        public string SpecialThanksStr => string.Join(",", SpecialThanks);
    }
}
