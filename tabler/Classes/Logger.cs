using System;
using System.Text;
using System.Timers;

namespace tabler.Classes {
    public static class Logger {

        private static readonly StringBuilder StringBuilderForLogging = new StringBuilder();
        public static System.Windows.Forms.TextBox TextBoxToLogIn;

        private static Timer _timer;

        public static void ClearLog() {
            StringBuilderForLogging.Clear();
        }

        public static void Log(string message) {
            if (TextBoxToLogIn == null) {
                return;
            }

            if (_timer == null) {
                _timer = new Timer {Interval = 500};
                _timer.Elapsed += timer_Elapsed;
            }

            StringBuilderForLogging.Insert(0, DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss") + " - " + message + Environment.NewLine);
            _timer.Start();
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e) {
            _timer.Stop();

            if (TextBoxToLogIn == null) {
                _timer.Start();
            }

            TextBoxToLogIn.UiThread(delegate {
                TextBoxToLogIn.Text = StringBuilderForLogging.ToString();
            });


            _timer.Start();
        }


    }
}
