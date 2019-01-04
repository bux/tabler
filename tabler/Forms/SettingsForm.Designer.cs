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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbRemoveEmptyNodes = new System.Windows.Forms.CheckBox();
            this.grpBIndentation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSaveSettings
            // 
            resources.ApplyResources(this.btnSaveSettings, "btnSaveSettings");
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // grpBIndentation
            // 
            this.grpBIndentation.Controls.Add(this.rbIndentSpaces);
            this.grpBIndentation.Controls.Add(this.tbIndentation);
            this.grpBIndentation.Controls.Add(this.rbIndentTabs);
            resources.ApplyResources(this.grpBIndentation, "grpBIndentation");
            this.grpBIndentation.Name = "grpBIndentation";
            this.grpBIndentation.TabStop = false;
            // 
            // rbIndentSpaces
            // 
            resources.ApplyResources(this.rbIndentSpaces, "rbIndentSpaces");
            this.rbIndentSpaces.Checked = true;
            this.rbIndentSpaces.Name = "rbIndentSpaces";
            this.rbIndentSpaces.TabStop = true;
            this.rbIndentSpaces.UseVisualStyleBackColor = true;
            // 
            // tbIndentation
            // 
            resources.ApplyResources(this.tbIndentation, "tbIndentation");
            this.tbIndentation.Name = "tbIndentation";
            // 
            // rbIndentTabs
            // 
            resources.ApplyResources(this.rbIndentTabs, "rbIndentTabs");
            this.rbIndentTabs.Name = "rbIndentTabs";
            this.rbIndentTabs.UseVisualStyleBackColor = true;
            this.rbIndentTabs.CheckedChanged += new System.EventHandler(this.rbIndentTabs_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbRemoveEmptyNodes);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbRemoveEmptyNodes
            // 
            resources.ApplyResources(this.cbRemoveEmptyNodes, "cbRemoveEmptyNodes");
            this.cbRemoveEmptyNodes.Checked = true;
            this.cbRemoveEmptyNodes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRemoveEmptyNodes.Name = "cbRemoveEmptyNodes";
            this.cbRemoveEmptyNodes.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpBIndentation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveSettings);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.Settings_Load);
            this.grpBIndentation.ResumeLayout(false);
            this.grpBIndentation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.GroupBox grpBIndentation;
        private System.Windows.Forms.RadioButton rbIndentSpaces;
        private System.Windows.Forms.TextBox tbIndentation;
        private System.Windows.Forms.RadioButton rbIndentTabs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbRemoveEmptyNodes;
    }
}