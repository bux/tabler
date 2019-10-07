using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using tabler.Logic.Classes;
using tabler.Logic.Enums;
using tabler.Logic.Helper;
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
        private class KeyCollection : ObservableCollection<Key>
        {}

        private KeyCollection _keyCollection = new KeyCollection();
        public bool AlreadyLoaded { get; set; } = false;
        public Stringtable StringTable { get; set; }
        public FileInfo CurrentFile { get; set; }
        public List<ItemSelected> Header { get; set; }
        private bool _filteringDiabled = false;

        private PropertyGroupDescription _groupDescriptionPackageName = new PropertyGroupDescription("PackageName");
        private PropertyGroupDescription _groupDescriptionIsComplete = new PropertyGroupDescription("IsCompleteName");
       
        #region CTOR and Load data
        public TranslationFileSingleControl(FileInfo file) : this()
        {
            CurrentFile = file;
        }

        public TranslationFileSingleControl()
        {
            InitializeComponent();

            _collViewSource.Source = _keyCollection;
            _collViewSource.Filter += _collViewSource_Filter;
            
            if (_collViewSource.View.CanGroup == true)
            {
                _collViewSource.View.GroupDescriptions.Clear();
                _collViewSource.View.GroupDescriptions.Add(_groupDescriptionPackageName);
                _collViewSource.View.GroupDescriptions.Add(_groupDescriptionIsComplete);
            }

            dge_Main.ItemsSource = _collViewSource.View;

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

        public void Load()
        {
            if (AlreadyLoaded)
            {
                return;
            }

            try
            {
                AlreadyLoaded = true;
                dge_Main.Columns.Clear();

                StringTable = StringtableHelper.ParseStringtable(CurrentFile);
              
                SetHeader();
                UpdateIsCompleteFlag();

                foreach (var item in StringTable.Project.Packages.SelectMany(x => x.Keys).ToList())
                {
                    _keyCollection.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
        }


        #endregion

        #region DataGridEvents

        private void _collViewSource_Filter(object sender, FilterEventArgs e)
        {
            Key t = e.Item as Key;

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

                this._collViewSource.View.GroupDescriptions.Add( _groupDescriptionIsComplete);
            }
            else
            {
                if (existsAlready)
                {
                    this._collViewSource.View.GroupDescriptions.Remove(_groupDescriptionIsComplete);
                }
            }

            _collViewSource.View.Refresh();
        }

        #endregion      

        #region ContextMenu actions
        private void pasteCompleteRowBelowCurrentRow(object sender, RoutedEventArgs e)
        {
            var keys = ClipBoardHelper.GetKeyObjectsFromClipboard();
            if (keys != null && keys.Any())
            {
                StringTable.Project.Packages.FirstOrDefault().Keys.AddRange(keys);
            }
        }

        private void copyCompleteRow(object sender, RoutedEventArgs e)
        {
            var b = _collViewSource.IsLiveGrouping;

            var keys = this.dge_Main.SelectedCells.Select(x => x.Item).OfType<Key>().Distinct().ToList();
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
                StringtableHelper.SaveStringTableFile(CurrentFile, StringTable, Header);
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }
        }

        private void btnReloadFile_click(object sender, RoutedEventArgs e)
        {
            AlreadyLoaded = false;
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
                _collViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                Logger.LogEx(ex);
            }

        }

        #endregion

        #region private methods

  private void SetHeader()
        {
            var initHeader = TranslationHelper.GetHeaders(StringTable);

            var allItems = new List<ItemSelected>();

            foreach (var languageName in Enum.GetNames(typeof(Languages)))
            {
                allItems.Add(new ItemSelected() { DisplayName = languageName, IsSelected = initHeader.Any(x => x == languageName), Key = languageName });
            }
            Header = allItems;

            AddColumns(new ItemSelected() { IsSelected = true, DisplayName = "IsComplete", Key = "IsComplete" });
            AddColumns(new ItemSelected() { IsSelected = true, DisplayName = "Id", Key = "Id" });
            AddColumns(new ItemSelected() { IsSelected = true, DisplayName = "Project", Key = "Project" });

            foreach (var header in allItems)
            {
                AddColumns(header);
            }
        }

        private void AddColumns(ItemSelected header)
        {
            var binding = new Binding(header.Key);
            binding.Mode = BindingMode.TwoWay;
            var dgNew = new DataGridTextColumn
            {
                // bind to a dictionary property
                Binding = binding,//new Binding("Custom[" + name + "]"),
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
            //foreach (var item in _keyCollection)
            //{
            //   item.UpdateIsComplete(Header.Where(x=> x.IsSelected).Select(x=> x.Key).ToList());
            //}

            var selectedHeader = Header.Where(x => x.IsSelected).Select(x => x.Key).ToList();

            foreach (var item in StringTable.Project.Packages.SelectMany(x => x.Keys))
            {
                item.UpdateIsComplete(selectedHeader);
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
