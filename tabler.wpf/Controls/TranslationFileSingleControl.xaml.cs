using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using tabler.Logic.Classes;
using tabler.Logic.Enums;
using tabler.Logic.Helper;
using tabler.wpf.Container;
using tabler.wpf.Helper;
using Key = tabler.Logic.Classes.Key;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for SingleLanguageTranslationControl.xaml
    /// </summary>
    public partial class TranslationFileSingleControl : UserControl
    {
        private CollectionViewSource _collViewSource = new CollectionViewSource();
        private class KeyCollection : ObservableCollection<Key_ExtendedWithChangeTracking>
        { }

        private class Key_ExtendedWithChangeTrackingValueConverter : IValueConverter
        {
            private string _propertyName;
            public Key_ExtendedWithChangeTrackingValueConverter(string systemValuesPropertyName)
            {
                _propertyName = systemValuesPropertyName;
            }
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Key_ExtendedWithChangeTracking key)
                {
                    return ((Key_ExtendedWithChangeTracking)value).SystemValues[_propertyName].CurrentValue;
                }
                return value;

            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private KeyCollection _keyCollection = new KeyCollection();
        public bool AlreadyLoaded { get; set; } = false;
        public Stringtable StringTable { get; set; }
        public FileInfo CurrentFile { get; set; }
        public List<ItemSelected> Header { get; set; }
        private bool _filteringDiabled = false;

        private PropertyGroupDescription _groupDescriptionPackageName = new PropertyGroupDescription(null, new Key_ExtendedWithChangeTrackingValueConverter(Key_ExtendedWithChangeTracking.PackageName_PropertyName));
        private PropertyGroupDescription _groupDescriptionIsComplete = new PropertyGroupDescription(null, new Key_ExtendedWithChangeTrackingValueConverter(Key_ExtendedWithChangeTracking.IsComplete_PropertyName));

        public ProgressHelper TranslationProgress { get; set; }


        #region CTOR and Load data
        public TranslationFileSingleControl(FileInfo file) : this()
        {
            CurrentFile = file;
        }

        public TranslationFileSingleControl()
        {
            InitializeComponent();

            TranslationProgress = new ProgressHelper();
            _collViewSource.Source = _keyCollection;
            _collViewSource.Filter += _collViewSource_Filter;

            if (_collViewSource.View.CanGroup == true)
            {
                _collViewSource.View.GroupDescriptions.Clear();
                _collViewSource.View.GroupDescriptions.Add(_groupDescriptionPackageName);
                _collViewSource.View.GroupDescriptions.Add(_groupDescriptionIsComplete);
            }

            dge_Main.ItemsSource = _collViewSource.View;
            //dge_Main.ItemsSource = _keyCollection;
        
            var menu = new MenuItem { Header = "Copy complete row(s)" };
            menu.Click += copyCompleteRow;
            this.dge_Main.ContextMenu.Items.Add(menu);

            menu = new MenuItem { Header = "Paste complete row(s)" };
            menu.Click += pasteCompleteRowBelowCurrentRow; ;
            this.dge_Main.ContextMenu.Items.Add(menu);

            dge_Main.AddingNewItem += Dge_Main_AddingNewItem;

        }

        private void Dge_Main_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new Key();
        }

        private bool _isLoadingStringtable;
        private bool _doLoadAll;



        public async Task LoadStringTableAsync(bool doLoadAll = false)
        {
            if (StringTable == null)
            {
                //about 0.03 seconds
                //var ts = new StopWatch($"LoadStringTableAsync {CurrentFile.Directory.Name}");
                //ts.Start();
                _isLoadingStringtable = true;
                StringTable = await StringtableHelper.ParseStringtable(CurrentFile);

                foreach (var package in StringTable.Project.Packages)
                {
                    foreach (var key in package.Keys)
                    {
                        _keyCollection.Add(new Key_ExtendedWithChangeTracking(key, StringTable.Project.Name, package.Name, string.Empty));
                    }
                    foreach (var container in package.Containers)
                    {
                        foreach (var key in package.Keys)
                        {
                            _keyCollection.Add(new Key_ExtendedWithChangeTracking(key, StringTable.Project.Name, package.Name, container.Name));
                        }
                    }
                }

                var initHeader = TranslationHelper.GetHeaders(StringTable);

                var allItems = new List<ItemSelected>();

                foreach (var languageName in Enum.GetNames(typeof(Languages)))
                {
                    allItems.Add(new ItemSelected()
                    {
                        DisplayName = languageName,
                        IsSelected = initHeader.Any(x => x == languageName),
                        Key = languageName
                    });
                }
                Header = allItems;

                UpdateIsCompleteFlag();


                UpdateTranslationProgress();

                _isLoadingStringtable = false;
                if (_doLoadAll || doLoadAll)
                {
                    _doLoadAll = false;
                    Load();
                }
                //ts.StopAndLog();

            }
        }

        private void UpdateTranslationProgress()
        {
            int progressValue = 0;
            if (_keyCollection.Count == 0)
            {
                progressValue = 100;
            }
            else
            {
                progressValue =(int) (100 * ((double)_keyCollection.Count(x => x.IsComplete) / (double)_keyCollection.Count));
                if (progressValue < 0)
                {
                    progressValue = 0;
                }
                if (progressValue > 100)
                {
                    progressValue = 100;
                }
            }

            TranslationProgress.FireProgressChangedEvent(progressValue);
        }

        public async void Load()
        {
            if (AlreadyLoaded)
            {
                return;
            }

            if (_isLoadingStringtable)
            {
                _doLoadAll = true;
                return;
            }

            _isLoadingStringtable = false;
            _doLoadAll = false;
            try
            {
                AlreadyLoaded = true;

                var ts = new StopWatch($"Load {CurrentFile.Directory.Name}")
                {
                    LogStart = true
                };
                ts.Start();
                ResetAll();

                await LoadStringTableAsync();

                CreateTableColumns();
                ts.StopAndLog("CreateTableColumns done");

                RefreshTableView();

                var incomplete = _keyCollection.FirstOrDefault(x => x.IsComplete == false);

                if (incomplete != null)
                {
                    dge_Main.ScrollIntoView(incomplete);

                }
               

                ts.StopAndLog("RefreshTableView done");
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
        }

        private void RefreshTableView()
        {
            _collViewSource.View.Refresh();
        }

        private void ResetAll()
        {
            AlreadyLoaded = false;
            _keyCollection.Clear();
            this.StringTable = null;
            dge_Main.Columns.Clear();

        }


        #endregion

        #region DataGridEvents

        private void _collViewSource_Filter(object sender, FilterEventArgs e)
        {
            var t = e.Item as Key_ExtendedWithChangeTracking;

            if (t != null)
            {
                if (string.IsNullOrEmpty(this.tbe_FilterAndSearchAll.Value) && string.IsNullOrEmpty(this.tbe_FilterAndSearchInId.Value))
                {
                    e.Accepted = true;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.tbe_FilterAndSearchAll.Value))
                    {
                        if (t.ContainsText(this.tbe_FilterAndSearchAll.Value, false, true))
                        {
                            e.Accepted = true;
                        }
                        else
                        {
                            e.Accepted = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(this.tbe_FilterAndSearchInId.Value))
                    {
                        if (t.ContainsText(this.tbe_FilterAndSearchInId.Value, true, true))
                        {
                            e.Accepted = true;
                        }
                        else
                        {
                            e.Accepted = false;
                        }
                    }

                }
            }
        }

        private void tbe_FilterAndSearchInId_changed(int value)
        {
            if (_filteringDiabled)
            {
                return;
            }
            _filteringDiabled = true;
            tbe_FilterAndSearchAll.Value = "";
            _filteringDiabled = false;
            _collViewSource.View.Refresh();
        }

        private void tbe_FilterAndSearchAll_3CharsAdded(int value)
        {
            if (_filteringDiabled)
            {
                return;
            }
            _filteringDiabled = true;
            tbe_FilterAndSearchInId.Value = "";
            _filteringDiabled = false;
            _collViewSource.View.Refresh();
        }

        private void cbGroupByProject_click(object sender, RoutedEventArgs e)
        {
            var existsAlready = this._collViewSource.View.GroupDescriptions.Any(x => x == _groupDescriptionPackageName);

            if (cbGroupByProject.IsChecked.GetValueOrDefault())
            {
                if (existsAlready)
                {
                    return;
                }

                this._collViewSource.View.GroupDescriptions.Insert(0, _groupDescriptionPackageName);
            }
            else
            {
                if (existsAlready)
                {
                    this._collViewSource.View.GroupDescriptions.Remove(_groupDescriptionPackageName);
                }
            }

            _collViewSource.View.Refresh();

        }

        private void cbGroupByStatus_click(object sender, RoutedEventArgs e)
        {
            var existsAlready = this._collViewSource.View.GroupDescriptions.Any(x => x == _groupDescriptionIsComplete);

            if (cbGroupByStatus.IsChecked.GetValueOrDefault())
            {
                if (existsAlready)
                {
                    return;
                }

                this._collViewSource.View.GroupDescriptions.Add(_groupDescriptionIsComplete);
            }
            else
            {
                if (existsAlready)
                {
                    this._collViewSource.View.GroupDescriptions.Remove(_groupDescriptionIsComplete);
                }
            }

            //_collViewSource.View.Refresh();
        }

        #endregion      

        #region ContextMenu actions
        private void pasteCompleteRowBelowCurrentRow(object sender, RoutedEventArgs e)
        {
            var selected = this.dge_Main.SelectedCells.FirstOrDefault();
            if (selected == null)
            {
                Logger.LogGeneral("Please select a target row first.");
                return;
            }

            var keys = ClipBoardHelper.GetKeyObjectsFromClipboard();

            if (keys == null || !keys.Any())
            {
                return;
            }

            //StringTable.Project.Packages.FirstOrDefault().Keys.AddRange(keys);

        }

        private void copyCompleteRow(object sender, RoutedEventArgs e)
        {
            var b = _collViewSource.IsLiveGrouping;

            var keys = this.dge_Main.SelectedCells.Select(x => x.Item).OfType<Key_ExtendedWithChangeTracking>().Distinct().ToList();
            if (keys != null && keys.Any())
            {
                ClipBoardHelper.AddKeyObjectsToClipboard(keys);
            }
        }

        #endregion

        #region Buttons

        private void btnSave_click(object sender, RoutedEventArgs e)
        {
            if (CurrentFile == null)
            {
                return;
            }
            try
            {
                StringTable.HasChanges = true;
                SaveStringTableFile(CurrentFile, StringTable.FileHasBom, this._keyCollection.ToList(), Header);

            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
        }

        private void SaveStringTableFile(
                                FileInfo currentFileInfo,
                                bool fileHasBom,
                                List<Key_ExtendedWithChangeTracking> currentKeys,
                                List<ItemSelected> header)
        {
            if (currentKeys == null)
            {
                return;
            }

            if (!currentKeys.Any(x => x.HasChanged))
            {
                return;
            }

            var dummyNamespace = new XmlSerializerNamespaces();
            dummyNamespace.Add("", "");

            var xmlSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    ",
                Encoding = new UTF8Encoding(fileHasBom)
            };

            if (ConfigHelper.CurrentSettings.IndentationSettings == IndentationSettings.Spaces)
            {
                var indentChars = "";

                for (var i = 0; i < ConfigHelper.CurrentSettings.TabSize; i++)
                {
                    indentChars += " ";
                }

                xmlSettings.IndentChars = indentChars;
            }

            if (ConfigHelper.CurrentSettings.IndentationSettings == IndentationSettings.Tabs)
            {
                xmlSettings.IndentChars = "\t";
            }

            var projectGroups = _keyCollection.GroupBy(x => x.SystemValues[Key_ExtendedWithChangeTracking.Project_PropertyName].CurrentValue).ToList();

            var languagesToWrite = header.Where(x => x.IsSelected).Select(x => x.Key).Distinct().ToList();

            XDocument srcTree = new XDocument(new XDeclaration("1.0", "utf-8", "true"));

            foreach (var projectItems in projectGroups)
            {
                var project = new XElement("Project", new XAttribute("name", projectItems.Key));
                srcTree.Add(project);
                var packages = projectItems.GroupBy(x => x.PackageName).ToList();

                foreach (var package in packages)
                {
                    XElement toAddTo = project;
                    if (!string.IsNullOrEmpty(package.Key))
                    {
                        //we have packages
                        toAddTo = new XElement("Package", new XAttribute("name", package.Key));
                        project.Add(toAddTo);
                    }

                    foreach (var itemInPackage in package)
                    {
                        toAddTo.Add(itemInPackage.AsXElement(true, languagesToWrite));
                    }
                }
            }

            using (var writer = XmlWriter.Create(currentFileInfo.FullName, xmlSettings))
            {
                srcTree.Save(writer);
            }

            foreach (var item in _keyCollection)
            {
                item.ResetHasChanged();
            }
        }


        private void btnReloadFile_click(object sender, RoutedEventArgs e)
        {
            ResetAll();
            
            Load();
        }

        private void btnSelectLanguages_click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ctrl = new ItemSelectControl();

                ctrl.SetItemsAndSelectedItems(Header);
                ctrl.ShowInNewWindow(true, this, "Select Languages");

                SynchVisibleHeader();
                UpdateIsCompleteFlag();

            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }

        }

        #endregion

        #region private methods

        private void CreateTableColumns()
        {

            //AddColumns(new ItemSelected() { IsSelected = true, DisplayName = Key_ExtendedWithChangeTracking.IsComplete_PropertyName, Key = Key_ExtendedWithChangeTracking.IsComplete_PropertyName });
            AddColumns(new ItemSelected()
            {
                IsSelected = true,
                DisplayName = Key_ExtendedWithChangeTracking.Id_PropertyName,
                Key = Key_ExtendedWithChangeTracking.Id_PropertyName
            }, "SystemValues");
            //AddColumns(new ItemSelected() { IsSelected = true, DisplayName = Key_ExtendedWithChangeTracking.Project_PropertyName, Key = Key_ExtendedWithChangeTracking.Project_PropertyName });

            foreach (var header in Header)
            {
                AddColumns(header, "Languages");
            }
        }

        private void AddColumns(ItemSelected header, string dictionaryInKey_ExtendedWithChangeTracking)
        {
            var binding = new Binding(header.Key);
            binding.Mode = BindingMode.TwoWay;
            var dgNew = new DataGridTextColumn
            {
                // bind to a dictionary property
                Binding = new Binding(dictionaryInKey_ExtendedWithChangeTracking + "[" + header.Key + "].CurrentValue"),
                Header = header.DisplayName,
                Width = 100,
                Visibility = header.IsSelected ? Visibility.Visible : Visibility.Collapsed,
            };

            //dgNew.PastingCellClipboardContent += DgNew_PastingCellClipboardContent;
            //dgNew.CopyingCellClipboardContent += DgNew_CopyingCellClipboardContent;

            dge_Main.Columns.Add(dgNew);
        }

        private void UpdateIsCompleteFlag()
        {
            var selectedHeader = Header.Where(x => x.IsSelected).Select(x => x.Key).ToList();

            foreach (var item in _keyCollection)
            {
                item.Update_IsCompletedValue(selectedHeader);
            }
           
        }

        private void SynchVisibleHeader()
        {
            try
            {
                foreach (var column in dge_Main.Columns.OfType<DataGridColumn>())
                {
                    var header = Header.FirstOrDefault(x => x.Key == column.Header.ToString());

                    if (header == null)
                    {
                        continue;
                    }

                    column.Visibility = header.IsSelected ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }

        }

        #endregion






        //private void DgNew_CopyingCellClipboardContent(object sender, DataGridCellClipboardEventArgs e)
        //{

        //    var i = e.Item;
        //    var k = i as Logic.Classes.Key;
        //    if (k != null)
        //    {
        //        ClipBoardHelper.AddKeyObjectToClipboard(k);
        //    }
        //}

        //private void DgNew_PastingCellClipboardContent(object sender, DataGridCellClipboardEventArgs e)
        //{
        //    try
        //    {
        //        var k = ClipBoardHelper.GetKeyObjectFromClipboard();
        //        if (k != null)
        //        {

        //        }

        //        //if (!Clipboard.ContainsText())
        //        //{
        //        //    return;
        //        //}

        //        //var data = Clipboard.GetData(DataFormats.UnicodeText);

        //        //var datao = Clipboard.GetDataObject();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogEx(ex);
        //    }


        //}


    }
}
