using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tabler.Logic.Classes;
using tabler.Logic.Enums;
using tabler.Logic.Helper;
using tabler.wpf.Container;
using tabler.wpf.Controls;
using tabler.wpf.Helper;
using Control = System.Windows.Controls.Control;

namespace tabler.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly TranslationHelper TranslationHelper;
        private ReleaseVersion _newerRelease;

        public MainWindow()
        {
            //The Internationalization Fix 
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            var culture = CultureInfo.GetCultureInfo(ConfigHelper.CurrentSettings.Language);
            Dispatcher.Thread.CurrentCulture = culture;
            Dispatcher.Thread.CurrentUICulture = culture;

            InitializeComponent();

            Logger.LogMessageWithTypeArrived += ctrl_HelperLogMessages.AddMessage;
           
            TranslationHelper = new TranslationHelper();
        }

        private void btn_openLanguageFile(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Select mod/mission folder, all stringtable.xml files in subfolders will be opened.";
                dlg.IsFolderPicker = true;
                dlg.AddToMostRecentlyUsedList = true;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (!string.IsNullOrEmpty(ConfigHelper.CurrentSettings.LastPathOfDataFiles))
                {
                    dlg.InitialDirectory = ConfigHelper.CurrentSettings.LastPathOfDataFiles;
                }
              
                var result = dlg.ShowDialog();

                if (result  == CommonFileDialogResult.Ok )
                {
                    ctrl_languageOverview.LoadDirectory(new DirectoryInfo(dlg.FileName));//);new System.IO.DirectoryInfo(@"I:\arbeit\GitHub\Mamilacan\tabler\tabler\tabler.wpf\ExampleData"));
                    if (ConfigHelper.CurrentSettings.LastPathOfDataFiles != dlg.FileName)
                    {
                        ConfigHelper.CurrentSettings.LastPathOfDataFiles = dlg.FileName;
                        ConfigHelper.SaveSettings();
                    }
                }

                //// OpenOpenModFolderDialog();
                //if (MessageBox.Show(Properties.Resources.GridUI_Discard_all_changes, Properties.Resources.GridUI_Open,MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //{
                //    OpenOpenModFolderDialog();
                //}
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
        }

        private void btn_SaveLanguageFile(object sender, RoutedEventArgs e)
        {




        }

        private void btn_checkForNewVersion(object sender, RoutedEventArgs e)
        {
            CheckForNewVersion();
        }

        private void CheckForNewVersion()
        {
            try
            {
                var gh = new GitHubVersionHelper();
                var productVersion = Assembly.GetExecutingAssembly().GetName().Version;
              
                _newerRelease = gh.CheckForNewVersion(productVersion.ToString());

                if (_newerRelease != null)
                {
                    mi_getNewVersion.Visibility =  Visibility.Visible;
                    Logger.LogGeneral($"{Resources["GridUI_CheckForNewVersion_New_version_available"]} -> {_newerRelease.Version}");
                    Logger.LogGeneral($"{Resources["GridUI_CheckForNewVersion_Download_the_new_version_at"]}: {_newerRelease.HtmlUrl}");
                }
                else
                {
                    mi_getNewVersion.Visibility = Visibility.Collapsed;
                    Logger.LogGeneral($"{Resources["GridUI_CheckForNewVersion_Current_version_is_up_to_date"]} {productVersion}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Error: " + ex.Message);
            }
        }

        //private void comboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        var selectedItem = comboBoxLanguage.SelectedItem as ComboBoxItem;

        //        var culture = CultureInfo.GetCultureInfo(selectedItem.Tag as string);
        //        // Properties.Resources.ResourceManager.GetString("HelloWorld", culture);
        //        Dispatcher.Thread.CurrentCulture = culture;
        //        Dispatcher.Thread.CurrentUICulture = culture;
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.LogError(ex.ToString());
        //    }

        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           
        }

        private void languageSelected(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as Control;
            ConfigHelper.CurrentSettings.Language = selectedItem.Tag as string;
            ConfigHelper.SaveSettings();
            Logger.LogGeneral($"Language changed to: {ConfigHelper.CurrentSettings.Language}. Restart required.");
           
        }

        private void btn_exit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}