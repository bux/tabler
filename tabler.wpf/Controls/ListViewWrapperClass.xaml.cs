using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for ListViewWrapperClass.xaml
    /// </summary>
    public partial class ListViewWrapperClass : UserControl , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public ListViewWrapperClass(TranslationFileSingleControl control, string name)
        {
            InitializeComponent();
            this.pb_Progress.SetMax(100,true);
           
            Control = control;
            this.pb_Progress.SetOperationName("");
            this.pb_Progress.Set(control.TranslationProgress.CurrentProgressValue);
            Control.TranslationProgress.ProgressChangedEvent += TranslationProgress_ProgressChangedEvent;
            TranslationFileName = name;
            this.tbName.Content = name;

        }

        private void TranslationProgress_ProgressChangedEvent(int progress, string currentAction)
        {
            this.pb_Progress.Set(progress);

        }

      

        public TranslationFileSingleControl Control { get; set; }

        public string TranslationFileName { get; set; }
        

    }
}
