using System;
using System.Windows.Forms;

namespace tabler.Classes {
    static class ControlExtensions {
        public static void UiThread(this Control control, Action code) {
            if (control.InvokeRequired) {
                control.BeginInvoke(code);
                return;
            }
            code.Invoke();
        }

        public static void UiThreadInvoke(this Control control, Action code) {
            if (control.InvokeRequired) {
                control.Invoke(code);
                return;
            }
            code.Invoke();
        }
    }
}
