namespace tabler {
    partial class About {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblVersionName = new System.Windows.Forms.Label();
            this.lblCopyrightName = new System.Windows.Forms.Label();
            this.lblLicenseName = new System.Windows.Forms.Label();
            this.lnkLicense = new System.Windows.Forms.LinkLabel();
            this.lnkGithub = new System.Windows.Forms.LinkLabel();
            this.lblGitHubName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLogo
            // 
            resources.ApplyResources(this.pbLogo, "pbLogo");
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblCopyright
            // 
            resources.ApplyResources(this.lblCopyright, "lblCopyright");
            this.lblCopyright.Name = "lblCopyright";
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // lblVersionName
            // 
            resources.ApplyResources(this.lblVersionName, "lblVersionName");
            this.lblVersionName.Name = "lblVersionName";
            // 
            // lblCopyrightName
            // 
            resources.ApplyResources(this.lblCopyrightName, "lblCopyrightName");
            this.lblCopyrightName.Name = "lblCopyrightName";
            // 
            // lblLicenseName
            // 
            resources.ApplyResources(this.lblLicenseName, "lblLicenseName");
            this.lblLicenseName.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblLicenseName.Name = "lblLicenseName";
            // 
            // lnkLicense
            // 
            resources.ApplyResources(this.lnkLicense, "lnkLicense");
            this.lnkLicense.Name = "lnkLicense";
            this.lnkLicense.TabStop = true;
            // 
            // lnkGithub
            // 
            resources.ApplyResources(this.lnkGithub, "lnkGithub");
            this.lnkGithub.Name = "lnkGithub";
            this.lnkGithub.TabStop = true;
            // 
            // lblGitHubName
            // 
            resources.ApplyResources(this.lblGitHubName, "lblGitHubName");
            this.lblGitHubName.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblGitHubName.Name = "lblGitHubName";
            // 
            // About
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkGithub);
            this.Controls.Add(this.lblGitHubName);
            this.Controls.Add(this.lnkLicense);
            this.Controls.Add(this.lblLicenseName);
            this.Controls.Add(this.lblCopyrightName);
            this.Controls.Add(this.lblVersionName);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbLogo);
            this.Name = "About";
            this.Load += new System.EventHandler(this.About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblVersionName;
        private System.Windows.Forms.Label lblCopyrightName;
        private System.Windows.Forms.Label lblLicenseName;
        private System.Windows.Forms.LinkLabel lnkLicense;
        private System.Windows.Forms.LinkLabel lnkGithub;
        private System.Windows.Forms.Label lblGitHubName;
    }
}