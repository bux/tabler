using System;
using System.Collections;
using System.Collections.Generic;
using tabler.Logic.Helper;

namespace tabler.wpf.Helper
{
    public class ProgressLogger
    {

        public ProgressLogger(string name, int maxCount, DateTime? startDate = null, bool? logEstimatedTime = null)
        {
            if (maxCount > 0)
            {
                MaxItems = maxCount;
            }

            Name = name;

            CurrentIndex = 0;

            if (startDate.GetValueOrDefault() != DateTime.MinValue)
            {
                StartDate = startDate.GetValueOrDefault();
            }

            if (logEstimatedTime.HasValue)
            {
                LogEstimatedTime = logEstimatedTime.Value;
            }
        }

        public ProgressLogger(string name, IList listItems, DateTime? startDate = null, bool? logEstimatedTime = null) : this(name, 0, startDate, logEstimatedTime)
        {


            if (listItems == null) return;

            MaxItems = listItems.Count;

        }

        public string Name { get; set; }
        public int MaxItems { get; set; }
        public int CurrentIndex { get; set; }
        public double PercentageStartLogEstimatedTime { get; set; } = 1;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public bool LogEstimatedTime { get; set; } = true;

        public void IncAndLog(int? value = null)
        {
            IncIndex(value);
            Log();
        }

        public void IncIndex(int? value = null)
        {
            CurrentIndex += value ?? 1;
        }

        public void Log()
        {
            if (MaxItems <= 0 || CurrentIndex > MaxItems)
            {
                Logger.LogGeneral(Name + ": done");
                return;
            }

            var percent = CurrentIndex / (double)MaxItems * 100;

            var message = $"{Name}: {CurrentIndex}/{MaxItems} ({percent.SafeRound(2)}%) ";

            if (LogEstimatedTime && percent > PercentageStartLogEstimatedTime)
            {
                var timeGoneSeconds = DateTime.UtcNow.Subtract(StartDate).TotalSeconds;
                var secondsToGo = timeGoneSeconds * 100 / percent;
                var timeUntil = StartDate.AddSeconds(secondsToGo);
                var timeSpan = timeUntil.Subtract(DateTime.UtcNow);

                message += $" est.Time: {timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
            }
            Logger.LogGeneral(message);
        }

    }
}
