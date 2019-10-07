using System.Windows.Controls;

namespace tabler.wpf.Controls
{
    public class Helper_TabItemExtended:TabItem
    {
        public delegate void TabItemSelectedDelegate();

        public event TabItemSelectedDelegate TabItemSelected;

        public void FireTabItemSelectedEvent()
        {
            TabItemSelected?.Invoke();
        }


    }
}
