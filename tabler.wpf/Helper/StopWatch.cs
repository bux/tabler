
using System;
using tabler.Logic.Enums;
using tabler.Logic.Helper;

namespace tabler.wpf.Helper
{
    /// <summary>
    /// Class StopWatch.
    /// </summary>
    public class StopWatch
    {
        private DateTime _start;
        //private string OperationName = "undefined";
        private LogTypeEnum _logType =  LogTypeEnum.General;
        private string _operationName = "undefined";

        public string OperationName
        {
            get { return _operationName; }
            set
            {
                _operationName = value;
                
            } 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StopWatch"/> class.
        /// </summary>
        /// <param name="operationName">Description of the operation.</param>
        public StopWatch(string operationName)
        {
            OperationName = operationName;
            Start();
        }

        public bool LogStart { get; set; }
        public StopWatch(string operationName, bool logStart) : this(operationName)
        {
            LogStart = logStart;
        }
        
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _start = DateTime.Now;
            if (LogStart)
            {
                Logger.LogInternal($"Started operation: {OperationName}");
                _lastExecutionTimeInSeconds = null;
            }
        }

        private double? _lastExecutionTimeInSeconds ;
        public double LastExecutionTimeInSeconds
        {
            get
            {
                if (_lastExecutionTimeInSeconds.HasValue)
                {
                    return _lastExecutionTimeInSeconds.Value;
                }

               return DateTime.Now.Subtract(_start).TotalSeconds;
                
            } 
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void StopAndLog()
        {
         StopAndLog(null);
        }
        public void StopAndLog(bool resetTimer)
        {
            StopAndLog(null,resetTimer);
        }
        public void StopAndLog(string name,bool resetTimer = true)
        {
            var tolog = OperationName;
            if (!string.IsNullOrEmpty(name))
            {
                tolog = name;
            }
            _lastExecutionTimeInSeconds = LastExecutionTimeInSeconds;
            
            Logger.Log($"Finished operation: {tolog} in seconds: {LastExecutionTimeInSeconds}", _logType);
            if (resetTimer)
            {
                _start = DateTime.Now;
            }
            
        }
    }
}
