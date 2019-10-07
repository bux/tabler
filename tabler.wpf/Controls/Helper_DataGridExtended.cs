
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using tabler.Logic.Helper;
using tabler.wpf.Helper;

namespace tabler.wpf.Controls
{
    public class Helper_DataGridExtended : DataGrid
    {
        public Helper_DataGridExtended()
        {
            AutoGenerateColumns = false;
            CanUserAddRows = false;
            CanUserDeleteRows = false;
            this.Initialized += (sender, args) =>
            {

                if (this.ContextMenu == null)
                {
                    this.ContextMenu = new ContextMenu();
                }
                var menu = new MenuItem { Header = "Switch copymode" };
                menu.Click += switchCopyMode;
                this.ContextMenu.Items.Add(menu);
            };
            //this.CommandBindings.Add(new CommandBinding(new Command() )

            //CommandManager.RegisterClassCommandBinding(typeof(Helper_DataGridExtended),
            // new CommandBinding(ApplicationCommands.Copy,
            // new ExecutedRoutedEventHandler(OnExecutedCopy),
            // new CanExecuteRoutedEventHandler(OnCanExecuteCopy)));

            //CommandManager.RegisterClassCommandBinding(typeof(Helper_DataGridExtended),
            //        new CommandBinding(ApplicationCommands.Paste,
            //        new ExecutedRoutedEventHandler(OnExecutedPaste),
            //        new CanExecuteRoutedEventHandler(OnCanExecutePaste)));

            //CommandManager.RegisterClassCommandBinding(typeof(DataGrid),
            //        new CommandBinding(ApplicationCommands.Paste, new ExecutedRoutedEventHandler(OnExecutedPaste),
            //        new CanExecuteRoutedEventHandler(OnCanExecutePaste)));
            //CommandManager.RegisterClassCommandBinding(typeof(DataGridCell),
            //   new CommandBinding(ApplicationCommands.Paste, new ExecutedRoutedEventHandler(OnExecutedPaste),
            //   new CanExecuteRoutedEventHandler(OnCanExecutePaste)));

            //CommandManager.RegisterClassCommandBinding(typeof(DataGridColumn),
            // new CommandBinding(ApplicationCommands.Paste, new ExecutedRoutedEventHandler(OnExecutedPaste),
            // new CanExecuteRoutedEventHandler(OnCanExecutePaste)));
        }

        //private void OnCanExecuteCopy(object sender, CanExecuteRoutedEventArgs args)
        //{
        //    args.CanExecute = true;
        //    args.Handled = true;
           
        //}
        //private void OnExecutedCopy(object sender, ExecutedRoutedEventArgs  args)
        //{
        //    var i = this.SelectedValue;

        //}
        

        //private void OnCanExecutePaste(object sender, CanExecuteRoutedEventArgs args)
        //{
        //    args.CanExecute = CurrentCell != null;
        //    args.Handled = true;
        //}
    
        //private void OnExecutedPaste(object sender, ExecutedRoutedEventArgs e)
        //{
        //    try
        //    {


        //        var k = ClipBoardHelper.GetKeyObjectFromClipboard();

        //        //if (!Clipboard.ContainsText())
        //        //{
        //        //    return;
        //        //}

        //        //var data = (string)Clipboard.GetData(DataFormats.UnicodeText);

        //        //var cell = sender as DataGridCell;
        //        ////cell.IsEditing = true;
        //        //(cell.Content as TextBlock).Text = data;
        //        //cell.IsEditing = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogEx(ex);
        //    }
        //}
      
            
        private void switchCopyMode(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ClipboardCopyMode == DataGridClipboardCopyMode.IncludeHeader)
                {
                    this.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
                }
                else
                {
                    this.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
                }
                Logger.LogGeneral($"Switched mode to: {this.ClipboardCopyMode}");
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_DataGridExtended)}.{nameof(switchCopyMode)} Exception: {ex}");
            }
        }
    }
}
