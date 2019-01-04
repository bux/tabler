using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace tabler.Forms
{
    // https://social.msdn.microsoft.com/Forums/en-US/d28d8fbb-655a-45b4-a692-21420c5e014e/dark-transparent-layer-for-child-winform?forum=winforms
    public class OverlayForm : Form
    {
        private static readonly int HTTRANSPARENT = -1;
        private static readonly int WM_NCHITTEST = 0x0084;
        private static readonly int WS_EX_TOOLWINDOW = 0x00000080;
        private static readonly int SW_SHOWNOACTIVATE = 4;
        private static readonly int SW_SHOWNORMAL = 1;

        private OverlayForm(Form parent)
        {
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            Opacity = 0.5;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            SetStyle(ControlStyles.Selectable, false);

            parent.LocationChanged += (o, e) => { ResetLocationAndSize(parent); };
            parent.SizeChanged += (o, e) => { ResetLocationAndSize(parent); };
            //parent.Activated += (o, e) => { Hide(); };

            ResetLocationAndSize(parent);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOOLWINDOW;
                return createParams;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == WM_NCHITTEST)
        //    {
        //        m.Result = (IntPtr) HTTRANSPARENT;
        //        return;
        //    }

        //    base.WndProc(ref m);
        //}

        private void ResetLocationAndSize(Form parent)
        {
            var location = parent.PointToScreen(Point.Empty);
            Bounds = new Rectangle(location, parent.ClientSize);
        }

        //
        public static void ShowOverlay(Form parent, Form child)
        {
            var overlay = new OverlayForm(parent);
            overlay.Show(parent);

            child.Activated += (o, e) => { ShowWindow(overlay.Handle, SW_SHOWNORMAL); };
            child.FormClosed += (o, e) => { overlay.Close(); };
        }
    }
}
