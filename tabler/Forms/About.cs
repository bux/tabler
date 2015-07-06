using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace tabler {
    public partial class About : Form {
        public About() {
            InitializeComponent();
        }

        private void About_Load(object sender, System.EventArgs e) {

            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            lblCopyright.Text = versionInfo.LegalCopyright.ToString();
            lblVersion.Text = versionInfo.FileVersion.ToString();

            const string licenseUrl = "http://creativecommons.org/licenses/by-sa/4.0/";
            lnkLicense.Links.Add(0, licenseUrl.Length, licenseUrl);

            const string githubUrl = "https://github.com/bux578/tabler";
            lnkGithub.Links.Add(0, githubUrl.Length, githubUrl);
        }
    }
}
