namespace tabler
{
    partial class RemoveLanguage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveLanguage));
            this.lblLanguage = new System.Windows.Forms.Label();
            this.btnRemoveThisLanguage = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            // 
            // btnRemoveThisLanguage
            // 
            resources.ApplyResources(this.btnRemoveThisLanguage, "btnRemoveThisLanguage");
            this.btnRemoveThisLanguage.Name = "btnRemoveThisLanguage";
            this.btnRemoveThisLanguage.UseVisualStyleBackColor = true;
            this.btnRemoveThisLanguage.Click += new System.EventHandler(this.btnRemoveThisLanguage_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.Name = "cmbLanguage";
            // 
            // RemoveLanguage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRemoveThisLanguage);
            this.Controls.Add(this.lblLanguage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RemoveLanguage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.RemoveLanguage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.Button btnRemoveThisLanguage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbLanguage;
    }
}
