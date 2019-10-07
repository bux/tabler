using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using tabler.wpf.Container;
using tabler.wpf.Helper;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for General_VersionInfo.xaml
    /// </summary>
    public partial class General_VersionInfo : UserControl
    {
        public General_VersionInfo()
        {
            InitializeComponent();

            var lstVersions = new List<VersionInfoContainer>();

            _dgvVersionNumbers.SelectionMode = DataGridSelectionMode.Single;

            lstVersions.Add(CreateVersion00001());

            _dgvVersionNumbers.ItemsSource = lstVersions;

        }

        private VersionInfoContainer CreateVersion00001()
        {
            var versionInfoContainer = new VersionInfoContainer
            {
                Version = "0.0.0.01",
                ReleaseDate = new DateTime(2019, 9, 16)
            };

            versionInfoContainer.SpecialThanks.Add("Bux: for being very patient :D");

            versionInfoContainer.Features.Add("- Initial release");

            versionInfoContainer.Fixes.Add("- soon to come ;)");
            return versionInfoContainer;
        }

        private string CreateResultStringFromVersionInfo(VersionInfoContainer version)
        {
            string result = string.Empty;

            result += "Version: " + version.Version + Environment.NewLine;
            result += "ReleaseDate: " + version.ReleaseDate.ToString("yyyy.MM.dd") + Environment.NewLine;
            if (!string.IsNullOrEmpty(version.SpecialThanksStr))
            {
                result += "Special Thanks: " + version.SpecialThanksStr + Environment.NewLine;
            }

            result += Environment.NewLine;

            result += "Features: " + Environment.NewLine;
            result += "--------------------------------------------------------------------" + Environment.NewLine;
            result += version.FeaturesStr + Environment.NewLine;


            result += Environment.NewLine;

            result += "Bugfixes: " + Environment.NewLine;
            result += "--------------------------------------------------------------------" + Environment.NewLine;
            result += version.FixesStr + Environment.NewLine;

            result += Environment.NewLine;

            return result;
        }

        private void _dgvVersionNumbers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (_dgvVersionNumbers.SelectedItems.Count > 0)
            {
                var items = _dgvVersionNumbers.GetSelectedBoundItems<VersionInfoContainer>();
                if (items != null && items.Any())
                {
                    _tbVersionInfos.Text = CreateResultStringFromVersionInfo(items.First());
                }
            }
        }
    }
}
