using System;
using System.Collections.Generic;
using System.IO;
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
using tabler.Logic.Helper;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for LanguageOverviewControl.xaml
    /// </summary>
    public partial class TranslationFileOverviewControl : UserControl
    {
        public TranslationFileOverviewControl()
        {
            InitializeComponent();
            
        }

        public List<DirectoryInfo> Directorys { get; set; } = new List<DirectoryInfo>();

        public void LoadDirectory(DirectoryInfo di )
        {
            if (!DirectoryAlreadyAdded(di))
            {
                Directorys.Add(di);
            }
            else
            {
                Logger.LogError($"Files of folder: {di.FullName} already added.");
                return;
            }

           var files =  di.GetFiles("*stringtable*.xml*", SearchOption.AllDirectories);

            tc_Main.Items.Clear();

            foreach (var file in files)
            {
                var languageControl = new TranslationFileSingleControl(file);
                var tiExtended = new Helper_TabItemExtended() { Header = $"{file.Directory.Name}\\{file.Name}", Content = languageControl };
                tiExtended.TabItemSelected += languageControl.Load;

                tc_Main.Items.Add(tiExtended);
            }

        }

        private bool DirectoryAlreadyAdded(DirectoryInfo di )
        {
            ///todo check for subfolders
            return false;


        }

    }
}
