using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using tabler.Logic.Helper;
using tabler.wpf.Helper;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for LanguageOverviewControl.xaml
    /// </summary>
    public partial class DirectoryFilesOverviewControl : UserControl
    {
        //private class ListViewWrapperClass : UserControl
        //{



        //}

        private List<ListViewWrapperClass> _listFileControls = new List<ListViewWrapperClass>();
        public DirectoryFilesOverviewControl()
        {
            InitializeComponent();
        }
        private DirectoryInfo _di;
        public void LoadDirectory(DirectoryInfo di)
        {
            _di = di;

            var files = di.GetFiles("*stringtable*.xml*", SearchOption.AllDirectories);

            //tc_MainPerProject.Items.Clear();

            var ts = new StopWatch($"Loading {files.Length} translation files...");

            foreach (var file in files)
            {
                var languageControl = new TranslationFileSingleControl(file);
                _ = languageControl.LoadStringTableAsync();
                _listFileControls.Add(new ListViewWrapperClass(languageControl, $"{file.Directory.Name}\\{file.Name}"));
            }
            _listFileControls = _listFileControls.OrderBy(x => x.TranslationFileName).ToList();

            lvSubProjects.ItemsSource = _listFileControls;
            ts.StopAndLog();

        }

        private void LvSubProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvSubProjects.SelectedItem == null)
            {
                return;
            }
            var control = lvSubProjects.SelectedItem as ListViewWrapperClass;
            if (control != null)
            {
                ccMainContent.Content = control.Control;
                control.Control.Load();
            }

        }
    }
}
