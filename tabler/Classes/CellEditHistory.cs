using System;
using System.Drawing;
using System.Windows.Forms;

namespace tabler
{
    public class CellEditHistory
    {
        public DataGridViewCell Cell { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Mod { get; set; }

        public Color OldBackColor { get; set; }
    }
}