using System;
using System.Collections;
using System.Collections.Generic;
using tabler.wpf.Helper;

namespace tabler.wpf.Helper
{

    public class ProgressLoggerIProgressBarControl<T> : ProgressLogger, IProgressBarControl
    {
        public ProgressLoggerIProgressBarControl(string name, int maxCount, DateTime? startDate = null, bool? logEstimatedTime = null) : base(name, maxCount, startDate, logEstimatedTime)
        {
        }

        public ProgressLoggerIProgressBarControl(string name, IList<T> listItems, DateTime? startDate = null, bool? logEstimatedTime = null) : base(name, (IList)listItems, startDate, logEstimatedTime)
        {
        }

        #region Implementation of IProgress<in double>

        public void Report(double value)
        {
            Log();
        }

        #endregion

        #region Implementation of IProgressBarControl

        public void Reset()
        {
            CurrentIndex = 0;
        }

        public void Increase(int value)
        {
            Increase(value);
            Log();
        }

        public void Set(int value)
        {
            CurrentIndex = value;
            Log();
        }

        public void Decrease(int value)
        {
            CurrentIndex -= value;
            Log();
        }

        public void SetMax(int max, bool doReset)
        {
            MaxItems = max;
            if (doReset)
            {
                Reset();
            }
        }

        public void SetOperationName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Name = name;
            }
        }

        #endregion
    }
}
