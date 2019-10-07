using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace tabler.wpf.Helper
{
    public static class TranslationHelperRessources
    {

        public static string GetString(string key, CultureInfo info )
        {
            var profs = Properties.Resources.ResourceManager.GetString("HelloWorld", info);
            return profs;
        }
        public static string GetString(string key)
        {
            var profs = Properties.Resources.ResourceManager.GetString("HelloWorld", System.Threading.Thread.CurrentThread.CurrentCulture);
            return profs;
        }

    }
}
