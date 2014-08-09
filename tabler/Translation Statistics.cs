using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace tabler
{
    public partial class TranslationStatistics : Form
    {
        private readonly GridUI _myParent;

        public TranslationStatistics(GridUI parent)
        {
            _myParent = parent;
            InitializeComponent();
        }

        private void TranslationStatistics_Load(object sender, EventArgs e)
        {
            chart.ContextMenuStrip = contextMenu;

            _myParent.TranslationManager.TranslationComponents.Statistics = _myParent.TranslationManager.TranslationComponents.Statistics.OrderBy(x => x.LanguageName).ToList();

            PopulateChart();
        }

        private void PopulateChart()
        {
            int inc = 0;

            var chartArea = chart.ChartAreas.FirstOrDefault();
            if (chartArea != null)
            {
                chartArea.AxisX.MajorGrid.Enabled = false;
            }

            var series = new Series("Missing Translations");
            series.IsVisibleInLegend = false;
            series.XValueType = ChartValueType.String;

            foreach (LanguageStatistics modInfoStatistics in _myParent.TranslationManager.TranslationComponents.Statistics)
            {
                int missingTranslationCount = GetMissingTranslationCount(modInfoStatistics);

                var dataPoint = new DataPoint(inc, missingTranslationCount);
                dataPoint.AxisLabel = modInfoStatistics.LanguageName;
                dataPoint.Label = missingTranslationCount.ToString();
                dataPoint.ToolTip = AggregateMods(modInfoStatistics, "\n");

                series.Points.Add(dataPoint);

                inc += 1;
            }

            chart.Series.Add(series);
        }

        private static int GetMissingTranslationCount(LanguageStatistics modInfoStatistics)
        {
            return modInfoStatistics.MissingModStrings.Sum(ms => ms.Value);
        }

        private static string AggregateMods(LanguageStatistics modInfoStatistics, string seperator)
        {
            return modInfoStatistics.MissingModStrings.Select(ms => ms.Key).ToList().Aggregate((cur, next) => cur + seperator + next);
        }

        private void copyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var outerSb = new StringBuilder();
            var innterSb = new StringBuilder();

            int total = 0;

            foreach (LanguageStatistics modInfoStatistics in _myParent.TranslationManager.TranslationComponents.Statistics)
            {
                int missingTranslationCount = GetMissingTranslationCount(modInfoStatistics);
                var mods = AggregateMods(modInfoStatistics, ", ");
                
                total += missingTranslationCount;

                var spacer = "".PadRight(15 - modInfoStatistics.LanguageName.Length);

                innterSb.AppendLine(String.Format("{0}{1}{2} missing stringtable entry/entries. ({3})", modInfoStatistics.LanguageName, spacer, missingTranslationCount.ToString().PadLeft(3), mods));
            }

            outerSb.AppendLine(String.Format("Total number of missing keys: {0}", total));
            outerSb.AppendLine("");
            outerSb.Append(innterSb);

            Clipboard.SetText(outerSb.ToString());
        }
    }
}