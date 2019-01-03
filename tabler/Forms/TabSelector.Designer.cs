namespace tabler.Forms
{
    partial class TabSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabSelector));
            this.tbSelectTab = new System.Windows.Forms.TextBox();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panelContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSelectTab
            // 
            this.tbSelectTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSelectTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSelectTab.Location = new System.Drawing.Point(10, 10);
            this.tbSelectTab.Name = "tbSelectTab";
            this.tbSelectTab.Size = new System.Drawing.Size(754, 32);
            this.tbSelectTab.TabIndex = 0;
            this.tbSelectTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSelectTab_KeyDown);
            // 
            // panelContainer
            // 
            this.panelContainer.Controls.Add(this.tbSelectTab);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Padding = new System.Windows.Forms.Padding(10);
            this.panelContainer.Size = new System.Drawing.Size(774, 55);
            this.panelContainer.TabIndex = 1;
            // 
            // TabSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 55);
            this.Controls.Add(this.panelContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "TabSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TabSelector";
            this.Load += new System.EventHandler(this.TabSelector_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TabSelector_KeyDown);
            this.panelContainer.ResumeLayout(false);
            this.panelContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbSelectTab;
        private System.Windows.Forms.Panel panelContainer;
    }
}