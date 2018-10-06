using System;
using System.Drawing;

namespace tabler
{
    public class CellEditHistory
    {
        public int CellColumnIndex { get; set; }

        public int CellRowIndex { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Mod { get; set; }

        public Color OldBackColor { get; set; }
    }
}
