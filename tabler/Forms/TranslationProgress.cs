﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace tabler
{
    public partial class TranslationProgress : Form
    {
        private readonly GridUI _myParent;

        public TranslationProgress(GridUI parent)
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
            var inc = 0;

            var series = new Series("Missing Translations")
            {
                IsVisibleInLegend = false,
                XValueType = ChartValueType.String
            };

            var highestCount = 0;

            foreach (var header in _myParent.TranslationManager.TranslationComponents.Headers)
            {
                if (header == "ID")
                {
                    continue;
                }

                DataPoint dataPoint;

                var modInfoStat = _myParent.TranslationManager.TranslationComponents.Statistics.FirstOrDefault(s => s.LanguageName.ToLower().Equals(header.ToLower()));

                if (modInfoStat == null)
                {
                    dataPoint = new DataPoint(inc, 0)
                    {
                        AxisLabel = header,
                        Label = "0"
                    };
                }
                else
                {
                    var missingTranslationCount = GetMissingTranslationCount(modInfoStat);

                    if (missingTranslationCount > highestCount)
                    {
                        highestCount = missingTranslationCount;
                    }


                    dataPoint = new DataPoint(inc, missingTranslationCount)
                    {
                        AxisLabel = modInfoStat.LanguageName,
                        Label = missingTranslationCount.ToString(CultureInfo.CurrentCulture),
                        ToolTip = AggregateMods(modInfoStat, "\n")
                    };
                }


                series.Points.Add(dataPoint);

                inc += 1;
            }

            //foreach (LanguageStatistics modInfoStatistics in _myParent.TranslationManager.TranslationComponents.Statistics) {
            //    int missingTranslationCount = GetMissingTranslationCount(modInfoStatistics);

            //    if (missingTranslationCount > highestCount) {
            //        highestCount = missingTranslationCount;
            //    }


            //    var dataPoint = new DataPoint(inc, missingTranslationCount);
            //    dataPoint.AxisLabel = modInfoStatistics.LanguageName;
            //    dataPoint.Label = missingTranslationCount.ToString();
            //    dataPoint.ToolTip = AggregateMods(modInfoStatistics, "\n");

            //    series.Points.Add(dataPoint);

            //    inc += 1;
            //}


            chart.Series.Add(series);

            var chartArea = chart.ChartAreas.FirstOrDefault();
            if (chartArea != null)
            {
                chartArea.AxisX.MajorGrid.Enabled = false;

                chartArea.AxisY.Interval = 10.0;

                if (highestCount > 100)
                {
                    chartArea.AxisY.Interval = 25.0;
                }
            }
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

            var total = 0;

            foreach (var modInfoStatistics in _myParent.TranslationManager.TranslationComponents.Statistics)
            {
                var missingTranslationCount = GetMissingTranslationCount(modInfoStatistics);
                var mods = AggregateMods(modInfoStatistics, ", ");

                total += missingTranslationCount;

                var spacer = "".PadRight(15 - modInfoStatistics.LanguageName.Length);

                innterSb.AppendLine(string.Format("{0}{1}{2} missing stringtable entry/entries. ({3})", modInfoStatistics.LanguageName, spacer, missingTranslationCount.ToString().PadLeft(3), mods));
            }

            outerSb.AppendLine(string.Format("Total number of keys: {0}", _myParent.TranslationManager.TranslationComponents.KeyCount));
            outerSb.AppendLine(string.Format("Total number of missing translations: {0}", total));
            outerSb.AppendLine("");
            outerSb.Append(innterSb);

            Clipboard.SetText(outerSb.ToString());
        }

        private void copyDataasMdTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var outerSb = new StringBuilder();
            var innerSb = new StringBuilder();

            var totalKeys = _myParent.TranslationManager.TranslationComponents.KeyCount;


            foreach (var modInfoStatistics in _myParent.TranslationManager.TranslationComponents.Statistics)
            {
                var missingTranslationCount = GetMissingTranslationCount(modInfoStatistics);
                var mods = AggregateMods(modInfoStatistics, ", ");

                var percentage = (totalKeys - (double) missingTranslationCount) / totalKeys * 100;

                innerSb.AppendLine(string.Format("| {0} | {1} | {2} | {3} |", modInfoStatistics.LanguageName, missingTranslationCount, mods, Math.Round(percentage, 1)));
            }

            outerSb.AppendLine(DateTime.Now.ToString("yyMMdd HH:mm"));
            outerSb.AppendLine(string.Format("Total number of keys: {0}", totalKeys));
            outerSb.AppendLine("");
            outerSb.AppendLine("| Language | Missing Entries | Relevant Modules | % done |");
            outerSb.AppendLine("|----------|----------------:|------------------|--------|");

            outerSb.Append(innerSb);

            Clipboard.SetText(outerSb.ToString());
        }
    }
}
