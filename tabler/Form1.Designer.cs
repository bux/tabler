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
            this.m_tbModFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseModFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnConvertToExcel = new System.Windows.Forms.Button();
            this.m_btnExcelToXml = new System.Windows.Forms.Button();
            this.m_btnOpenCreatedExcel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_tbModFolder
            // 
            this.m_tbModFolder.Location = new System.Drawing.Point(12, 12);
            this.m_tbModFolder.Name = "m_tbModFolder";
            this.m_tbModFolder.Size = new System.Drawing.Size(227, 20);
            this.m_tbModFolder.TabIndex = 0;
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
            this.btnConvertToExcel.Text = "Xml -> Excel";
            this.btnConvertToExcel.UseVisualStyleBackColor = true;
            this.btnConvertToExcel.Click += new System.EventHandler(this.btnConvertToExcel_Click);
            // 
            // m_btnExcelToXml
            // 
            this.m_btnExcelToXml.Enabled = false;
            this.m_btnExcelToXml.Location = new System.Drawing.Point(12, 97);
            this.m_btnExcelToXml.Name = "m_btnExcelToXml";
            this.m_btnExcelToXml.Size = new System.Drawing.Size(80, 23);
            this.m_btnExcelToXml.TabIndex = 3;
            this.m_btnExcelToXml.Text = "Excel -> Xml";
            this.m_btnExcelToXml.UseVisualStyleBackColor = true;
            this.m_btnExcelToXml.Click += new System.EventHandler(this.m_btnExcelToXml_Click);
            // 
            // m_btnOpenCreatedExcel
            // 
            this.m_btnOpenCreatedExcel.Enabled = false;
            this.m_btnOpenCreatedExcel.Location = new System.Drawing.Point(98, 58);
            this.m_btnOpenCreatedExcel.Name = "m_btnOpenCreatedExcel";
            this.m_btnOpenCreatedExcel.Size = new System.Drawing.Size(131, 23);
            this.m_btnOpenCreatedExcel.TabIndex = 4;
            this.m_btnOpenCreatedExcel.Text = "Open Excel";
            this.m_btnOpenCreatedExcel.UseVisualStyleBackColor = true;
            this.m_btnOpenCreatedExcel.Click += new System.EventHandler(this.m_btnOpenCreatedExcel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 396);
            this.Controls.Add(this.m_btnOpenCreatedExcel);
            this.Controls.Add(this.m_btnExcelToXml);
            this.Controls.Add(this.btnConvertToExcel);
            this.Controls.Add(this.btnBrowseModFolder);
            this.Controls.Add(this.m_tbModFolder);
            this.Name = "Form1";
            this.Text = "Arma 3 Translation Helper (tabler :D)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_tbModFolder;
        private System.Windows.Forms.Button btnBrowseModFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnConvertToExcel;
        private System.Windows.Forms.Button m_btnExcelToXml;
        private System.Windows.Forms.Button m_btnOpenCreatedExcel;
    }
}

