using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tabler.Logic.Helper;

namespace tabler.wpf.Helper
{
    public class ProgressLoggerForEach<T> : ProgressLogger, IEnumerable<T>, IEnumerator<T>
    {
        public IList<T> List { get; set; }
        private IEnumerator<T> _iEnumerator;
        private Func<T, string> _propertySelector;


        public ProgressLoggerForEach(string name, IList<T> listItems, DateTime? startDate = null, bool? logEstimatedTime = null, Func<T, string> propertySelector = null) : base(name, (IList)listItems, startDate, logEstimatedTime)
        {

            _propertySelector = propertySelector;

            if (listItems == null) return;



            MaxItems = listItems.Count;
            List = listItems;
            
            _iEnumerator = listItems.GetEnumerator();
        
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

            var percent = (((double)CurrentIndex / (double)MaxItems) * (double)100);

            var message = $"{Name}{GetCurrentObjectsName()}: {CurrentIndex}/{MaxItems} ({percent.SafeRound(2)}%) ";

            if (LogEstimatedTime && percent > PercentageStartLogEstimatedTime)
            {
                var timeGoneSeconds = DateTime.UtcNow.Subtract(StartDate).TotalSeconds;
                var secondsToGo = (timeGoneSeconds * 100) / percent;
                var timeUntil = StartDate.AddSeconds(secondsToGo);
                var timeSpan = timeUntil.Subtract(DateTime.UtcNow);

                message += $" est.Time: {timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
            }
            Logger.LogGeneral(message);
        }

        private string GetCurrentObjectsName()
        {
            if (_propertySelector == null || _iEnumerator.Current == null)
            {
                return null;
            }
            var ss = "." + new List<T> { _iEnumerator.Current }.Select(_propertySelector).First();
            return ss;
        }

        #region Implementation of IEnumerable

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _iEnumerator?.Dispose();
            _iEnumerator = null;
        }

        #endregion

        #region Implementation of IEnumerator

        public bool MoveNext()
        {
            IncAndLog();

            if (_iEnumerator != null)
            {
                return _iEnumerator.MoveNext();
            }
            return false;

        }

        public void Reset()
        {
            _iEnumerator?.Reset();
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public T Current
        {
            get
            {
                if (_iEnumerator == null)
                {
                    return default(T);
                }
                return _iEnumerator.Current;

            }
        }

        #endregion
    }
}
