using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabler.Classes {
    public class Settings {
        public IndentationSettings IndentationSettings { get; set; }
        public int TabSize { get; set; }

        public Settings() {
            // defaults
            IndentationSettings = IndentationSettings.Spaces;
            TabSize = 4;
        }

    }

    public enum IndentationSettings {
        Spaces = 0,
        Tabs = 1
    }
}
