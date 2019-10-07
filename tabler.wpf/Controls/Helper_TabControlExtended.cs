using System.Windows.Controls;

namespace tabler.wpf.Controls
{
    public class Helper_TabControlExtended : TabControl
    {
        public Helper_TabControlExtended()
        {
           
            this.SelectionChanged += (sender, args) =>
            {

                if (this.SelectedItem == null)
                {
                    return;
                }
                if (_lastSelectedTabItem == this.SelectedItem)
                {
                    return;
                }

                _lastSelectedTabItem = this.SelectedItem;

                if (_lastSelectedTabItem is Helper_TabItemExtended)
                {
                    var ti = (Helper_TabItemExtended)_lastSelectedTabItem;
                    ti.FireTabItemSelectedEvent();
                }
            };
        }

        private object _lastSelectedTabItem = null;
        
    }
}
