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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.m_tbModFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseModFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnConvertToExcel = new System.Windows.Forms.Button();
            this.m_btnExcelToXml = new System.Windows.Forms.Button();
            this.m_btnOpenCreatedExcel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tbModFolder
            // 
            this.m_tbModFolder.Location = new System.Drawing.Point(236, 19);
            this.m_tbModFolder.Name = "m_tbModFolder";
            this.m_tbModFolder.Size = new System.Drawing.Size(227, 20);
            this.m_tbModFolder.TabIndex = 0;
            // 
            // btnBrowseModFolder
            // 
            this.btnBrowseModFolder.Location = new System.Drawing.Point(469, 17);
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
            this.btnConvertToExcel.Location = new System.Drawing.Point(236, 45);
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
            this.m_btnExcelToXml.Location = new System.Drawing.Point(322, 45);
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
            this.m_btnOpenCreatedExcel.Location = new System.Drawing.Point(236, 74);
            this.m_btnOpenCreatedExcel.Name = "m_btnOpenCreatedExcel";
            this.m_btnOpenCreatedExcel.Size = new System.Drawing.Size(166, 23);
            this.m_btnOpenCreatedExcel.TabIndex = 4;
            this.m_btnOpenCreatedExcel.Text = "Open Excel";
            this.m_btnOpenCreatedExcel.UseVisualStyleBackColor = true;
            this.m_btnOpenCreatedExcel.Click += new System.EventHandler(this.m_btnOpenCreatedExcel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 90);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 116);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.m_btnOpenCreatedExcel);
            this.Controls.Add(this.m_btnExcelToXml);
            this.Controls.Add(this.btnConvertToExcel);
            this.Controls.Add(this.btnBrowseModFolder);
            this.Controls.Add(this.m_tbModFolder);
            this.Name = "Form1";
            this.Text = "tabler - Arma 3 Translation Helper";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

