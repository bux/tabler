namespace tabler
{
    partial class GridUI
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
            this.btnBrowseModFolder = new System.Windows.Forms.Button();
            this.m_tbModFolder = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoadStringtablexmls = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSaveStringtableXmls = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowseModFolder
            // 
            this.btnBrowseModFolder.Location = new System.Drawing.Point(236, 1);
            this.btnBrowseModFolder.Name = "btnBrowseModFolder";
            this.btnBrowseModFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseModFolder.TabIndex = 3;
            this.btnBrowseModFolder.Text = "Browse";
            this.btnBrowseModFolder.UseVisualStyleBackColor = true;
            this.btnBrowseModFolder.Click += new System.EventHandler(this.btnBrowseModFolder_Click);
            // 
            // m_tbModFolder
            // 
            this.m_tbModFolder.Location = new System.Drawing.Point(3, 3);
            this.m_tbModFolder.Name = "m_tbModFolder";
            this.m_tbModFolder.Size = new System.Drawing.Size(227, 20);
            this.m_tbModFolder.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1190, 649);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSaveStringtableXmls);
            this.panel1.Controls.Add(this.btnLoadStringtablexmls);
            this.panel1.Controls.Add(this.m_tbModFolder);
            this.panel1.Controls.Add(this.btnBrowseModFolder);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 94);
            this.panel1.TabIndex = 5;
            // 
            // btnLoadStringtablexmls
            // 
            this.btnLoadStringtablexmls.Enabled = false;
            this.btnLoadStringtablexmls.Location = new System.Drawing.Point(4, 29);
            this.btnLoadStringtablexmls.Name = "btnLoadStringtablexmls";
            this.btnLoadStringtablexmls.Size = new System.Drawing.Size(134, 23);
            this.btnLoadStringtablexmls.TabIndex = 4;
            this.btnLoadStringtablexmls.Text = "Load stringtable.xml files";
            this.btnLoadStringtablexmls.UseVisualStyleBackColor = true;
            this.btnLoadStringtablexmls.Click += new System.EventHandler(this.btnLoadStringtablexmls_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 103);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1184, 543);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // btnSaveStringtableXmls
            // 
            this.btnSaveStringtableXmls.Enabled = false;
            this.btnSaveStringtableXmls.Location = new System.Drawing.Point(144, 30);
            this.btnSaveStringtableXmls.Name = "btnSaveStringtableXmls";
            this.btnSaveStringtableXmls.Size = new System.Drawing.Size(134, 23);
            this.btnSaveStringtableXmls.TabIndex = 5;
            this.btnSaveStringtableXmls.Text = "Save stringtable.xml files";
            this.btnSaveStringtableXmls.UseVisualStyleBackColor = true;
            this.btnSaveStringtableXmls.Click += new System.EventHandler(this.btnSaveStringtableXmls_Click);
            // 
            // GridUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 649);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GridUI";
            this.Text = "tabler - Arma 3 Translation Helper";
            this.Load += new System.EventHandler(this.GridUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseModFolder;
        private System.Windows.Forms.TextBox m_tbModFolder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLoadStringtablexmls;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnSaveStringtableXmls;

    }
}