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
            this.SuspendLayout();
            // 
            // tbSelectTab
            // 
            this.tbSelectTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSelectTab.Location = new System.Drawing.Point(12, 12);
            this.tbSelectTab.Name = "tbSelectTab";
            this.tbSelectTab.Size = new System.Drawing.Size(776, 32);
            this.tbSelectTab.TabIndex = 0;
            this.tbSelectTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSelectTab_KeyDown);
            // 
            // TabSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 60);
            this.Controls.Add(this.tbSelectTab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "TabSelector";
            this.Text = "TabSelector";
            this.Load += new System.EventHandler(this.TabSelector_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TabSelector_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSelectTab;
    }
}