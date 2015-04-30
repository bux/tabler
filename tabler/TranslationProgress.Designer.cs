namespace tabler
{
    partial class TranslationProgress
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslationProgress));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDataasMdTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.chart, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // chart
            // 
            resources.ApplyResources(this.chart, "chart");
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Name = "chart";
            // 
            // contextMenu
            // 
            resources.ApplyResources(this.contextMenu, "contextMenu");
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyDataToolStripMenuItem,
            this.copyDataasMdTableToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            // 
            // copyDataToolStripMenuItem
            // 
            resources.ApplyResources(this.copyDataToolStripMenuItem, "copyDataToolStripMenuItem");
            this.copyDataToolStripMenuItem.Name = "copyDataToolStripMenuItem";
            this.copyDataToolStripMenuItem.Click += new System.EventHandler(this.copyDataToolStripMenuItem_Click);
            // 
            // copyDataasMdTableToolStripMenuItem
            // 
            resources.ApplyResources(this.copyDataasMdTableToolStripMenuItem, "copyDataasMdTableToolStripMenuItem");
            this.copyDataasMdTableToolStripMenuItem.Name = "copyDataasMdTableToolStripMenuItem";
            this.copyDataasMdTableToolStripMenuItem.Click += new System.EventHandler(this.copyDataasMdTableToolStripMenuItem_Click);
            // 
            // TranslationProgress
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TranslationProgress";
            this.Load += new System.EventHandler(this.TranslationStatistics_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyDataasMdTableToolStripMenuItem;
    }
}