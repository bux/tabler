using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabler.Classes {
    public class Settings {
        public IndentationSettings IndentationSettings { get; set; }
        public int TabSize { get; set; }
        public bool RemoveEmptyNodes { get; set; }

        public Settings() {
            // defaults
            IndentationSettings = IndentationSettings.Spaces;
            TabSize = 4;
            RemoveEmptyNodes = true;
        }

    }

    public enum IndentationSettings {
        Spaces = 0,
        Tabs = 1
    }
}
