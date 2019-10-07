using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using tabler.Logic.Enums;
using tabler.wpf.Helper;

namespace tabler.wpf.Controls
{
    /// <summary>
    /// Interaction logic for Helper_LogMessages.xaml
    /// </summary>
    public partial class Helper_LogMessages : UserControl, ICloneable
    {
        public ObservableCollection<LogRow> LogData { get; set; } = new ObservableCollection<LogRow>();

        private List<LogRow> _lstGeneral = new List<LogRow>();
        private List<LogRow> _lstInternal = new List<LogRow>();
        private List<LogRow> _lstError = new List<LogRow>();
        
        public delegate void LogMessageWithTypeDelegate(string message, LogTypeEnum logType);

        public event LogMessageWithTypeDelegate LogMessageWithTypeArrived;

        public void FireLogMessageWithTyperArivedEvent(string message, LogTypeEnum logType)
        {
            LogMessageWithTypeArrived?.Invoke(message, logType);
        }

        public Helper_LogMessages()
        {
            InitializeComponent();
        }

        public void AddMessage(string message, LogTypeEnum logtype)
        {
            FireLogMessageWithTyperArivedEvent(message, logtype);
            var row = new LogRow { DateTime = DateTime.Now, Message = message };

            //if logging seem to be a problem, just make this asynch
            Dispatcher.InvokeAsync(() =>
            {
                AddMessage(row, logtype);
            });

        }

        private void AddMessage(LogRow row, LogTypeEnum logtype)
        {
            switch (logtype)
            {
                case LogTypeEnum.General:
                    _lstGeneral.Add(row);
                    if (cbShowGeneral.IsChecked.Value == false)
                    {
                        return;
                    }

                    break;
                case LogTypeEnum.Error:
                    _lstError.Add( row);
                    if (cbShowError.IsChecked.Value == false)
                    {
                        return;
                    }
                    break;
                case LogTypeEnum.Internal:
                    _lstInternal.Add( row);
                    if (cbShowInternal.IsChecked.Value == false)
                    {
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logtype), logtype, null);
            }

            if (CbPauseScrolling.IsChecked.Value == false)
            {
                AddMessageToGrid(row);
            }


        }

        private void CbPauseScrollingGeneral_OnChecked(object sender, RoutedEventArgs e)
        {
            if (CbPauseScrolling.IsChecked.Value == false)
            {
                UpdateAllogs();
            }
        }

        public class LogRow
        {
            public DateTime DateTime { get; set; }
            public string Message { get; set; }
        }

        private void UpdateLogsShowing(object sender, RoutedEventArgs e)
        {
            UpdateAllogs();

        }

        public void UpdateAllogs()
        {
            var toShow = new List<LogRow>();

            if (cbShowGeneral == null)
            {
                return;
            }

            if (cbShowGeneral.IsChecked.Value)
            {
                toShow.AddRange(_lstGeneral);
            }
            if (cbShowInternal.IsChecked.Value)
            {
                toShow.AddRange(_lstInternal);
            }
            if (cbShowError.IsChecked.Value)
            {
                toShow.AddRange(_lstError);
            }

            LogData.Clear();

            toShow.OrderByDescending(x => x.DateTime).ToList().ForEach(AddMessageToGrid);
        }

        private void AddMessageToGrid(LogRow logRow)
        {
            if (CbShowDatetime.IsChecked.Value)
            {
                LogData.Insert(0,new LogRow() {Message = $"{logRow.DateTime.GetDateTimeString_yyyyMMddhhmmss()}: {logRow.Message}"});
            }
            else
            {
                LogData.Insert(0,logRow);
            }
        }

        public object Clone()
        {
            var clone = new Helper_LogMessages { _lstError = this._lstError.ToList(), _lstGeneral = _lstGeneral.ToList(), _lstInternal = _lstInternal.ToList() };
            this.LogMessageWithTypeArrived += clone.AddMessage;

            return clone;

        }

        private void CbShowDatetime_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateAllogs();
        }

        private void ResetAllMessages()
        {
            _lstInternal.Clear();
            _lstError.Clear();
            _lstGeneral.Clear();
            UpdateAllogs();

        }

        private void BtnResetAllMessages_OnClick(object sender, RoutedEventArgs e)
        {
            ResetAllMessages();
        }

    }
}
