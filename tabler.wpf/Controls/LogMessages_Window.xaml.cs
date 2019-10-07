using System.Windows;
using System.Windows.Controls;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for LogMessages_Window.xaml
    /// </summary>
    public partial class LogMessages_Window : Window
    {
        public LogMessages_Window()
        {
            InitializeComponent();
        }
        
        public LogMessages_Window(Helper_LogMessages control)
        {
            InitializeComponent();

            SetMainControl(control);
            control.UpdateAllogs();
        }

        private void SetMainControl(Control control)
        {
            grdMainContent.Children.Clear();
            grdMainContent.Children.Add(control);
        }
    }
}
