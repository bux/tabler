namespace tabler {
    partial class SettingsForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.grpBIndentation = new System.Windows.Forms.GroupBox();
            this.rbIndentSpaces = new System.Windows.Forms.RadioButton();
            this.tbIndentation = new System.Windows.Forms.TextBox();
            this.rbIndentTabs = new System.Windows.Forms.RadioButton();
            this.grpBIndentation.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(93, 116);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(12, 116);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 6;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // grpBIndentation
            // 
            this.grpBIndentation.Controls.Add(this.rbIndentSpaces);
            this.grpBIndentation.Controls.Add(this.tbIndentation);
            this.grpBIndentation.Controls.Add(this.rbIndentTabs);
            this.grpBIndentation.Location = new System.Drawing.Point(12, 12);
            this.grpBIndentation.Name = "grpBIndentation";
            this.grpBIndentation.Size = new System.Drawing.Size(156, 72);
            this.grpBIndentation.TabIndex = 10;
            this.grpBIndentation.TabStop = false;
            this.grpBIndentation.Text = "Indentation";
            // 
            // rbIndentSpaces
            // 
            this.rbIndentSpaces.AutoSize = true;
            this.rbIndentSpaces.Checked = true;
            this.rbIndentSpaces.Location = new System.Drawing.Point(6, 42);
            this.rbIndentSpaces.Name = "rbIndentSpaces";
            this.rbIndentSpaces.Size = new System.Drawing.Size(61, 17);
            this.rbIndentSpaces.TabIndex = 12;
            this.rbIndentSpaces.TabStop = true;
            this.rbIndentSpaces.Text = "Spaces";
            this.rbIndentSpaces.UseVisualStyleBackColor = true;
            // 
            // tbIndentation
            // 
            this.tbIndentation.Location = new System.Drawing.Point(73, 42);
            this.tbIndentation.Name = "tbIndentation";
            this.tbIndentation.Size = new System.Drawing.Size(31, 20);
            this.tbIndentation.TabIndex = 10;
            this.tbIndentation.Text = "4";
            // 
            // rbIndentTabs
            // 
            this.rbIndentTabs.AutoSize = true;
            this.rbIndentTabs.Location = new System.Drawing.Point(6, 19);
            this.rbIndentTabs.Name = "rbIndentTabs";
            this.rbIndentTabs.Size = new System.Drawing.Size(49, 17);
            this.rbIndentTabs.TabIndex = 11;
            this.rbIndentTabs.Text = "Tabs";
            this.rbIndentTabs.UseVisualStyleBackColor = true;
            this.rbIndentTabs.CheckedChanged += new System.EventHandler(this.rbIndentTabs_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 151);
            this.Controls.Add(this.grpBIndentation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.grpBIndentation.ResumeLayout(false);
            this.grpBIndentation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.GroupBox grpBIndentation;
        private System.Windows.Forms.RadioButton rbIndentSpaces;
        private System.Windows.Forms.TextBox tbIndentation;
        private System.Windows.Forms.RadioButton rbIndentTabs;
    }
}