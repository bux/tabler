using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for LanguageSelect.xaml
    /// </summary>
    public partial class ItemSelectControl : UserControl
    {
        
     
        public ItemSelectControl()
        {
            InitializeComponent();
        }

        public void SetItemsAndSelectedItems(List<ItemSelected> items)
        {
            lbMain.ItemsSource = null;
            lbMain.ItemsSource = items;


        }

    }
}
