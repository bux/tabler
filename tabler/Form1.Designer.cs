namespace tabler
{
    partial class Form1
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
            this.tbModFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseModFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnConvertToExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbModFolder
            // 
            this.tbModFolder.Location = new System.Drawing.Point(12, 12);
            this.tbModFolder.Name = "tbModFolder";
            this.tbModFolder.Size = new System.Drawing.Size(227, 20);
            this.tbModFolder.TabIndex = 0;
            // 
            // btnBrowseModFolder
            // 
            this.btnBrowseModFolder.Location = new System.Drawing.Point(245, 10);
            this.btnBrowseModFolder.Name = "btnBrowseModFolder";
            this.btnBrowseModFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseModFolder.TabIndex = 1;
            this.btnBrowseModFolder.Text = "Browse";
            this.btnBrowseModFolder.UseVisualStyleBackColor = true;
            this.btnBrowseModFolder.Click += new System.EventHandler(this.btnBrowseModFolder_Click);
            // 
            // btnConvertToExcel
            // 
            this.btnConvertToExcel.Enabled = false;
            this.btnConvertToExcel.Location = new System.Drawing.Point(12, 58);
            this.btnConvertToExcel.Name = "btnConvertToExcel";
            this.btnConvertToExcel.Size = new System.Drawing.Size(80, 23);
            this.btnConvertToExcel.TabIndex = 2;
            this.btnConvertToExcel.Text = "xml -> Excel";
            this.btnConvertToExcel.UseVisualStyleBackColor = true;
            this.btnConvertToExcel.Click += new System.EventHandler(this.btnConvertToExcel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 239);
            this.Controls.Add(this.btnConvertToExcel);
            this.Controls.Add(this.btnBrowseModFolder);
            this.Controls.Add(this.tbModFolder);
            this.Name = "Form1";
            this.Text = "tabler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbModFolder;
        private System.Windows.Forms.Button btnBrowseModFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnConvertToExcel;
    }
}

