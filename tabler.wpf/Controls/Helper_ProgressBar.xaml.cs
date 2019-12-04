using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using tabler.Logic.Helper;
using tabler.wpf.Helper;

namespace tabler.wpf.Controls
{

    /// <summary>
    /// Interaction logic for Helper_ProgressBar.xaml
    /// </summary>
    public partial class Helper_ProgressBar : UserControl, IProgressBarControl
    {
        public Helper_ProgressBar()
        {
            InitializeComponent();
            _updateTimer = new Timer {Interval = 1000};
            _updateTimer.AutoReset = false;
            _updateTimer.Start();
            _updateTimer.Elapsed += (sender, args) =>
            {
                _updateTimer.Stop();
                UpdateProgressName();
                _updateTimer.Start();
            };
            this.Visibility = Visibility.Collapsed;
        }

        private Timer _updateTimer;

        private string Name { get; set; }


        public delegate void OperationNameChangedDelegate(string value);

        public event OperationNameChangedDelegate OperationNameChanged;

        private void FireOperationNameChangedEvent(string value)
        {
            OperationNameChanged?.Invoke(value);
        }


        public void SetOperationName(string name)
        {
            Name = name;
            FireOperationNameChangedEvent(name);
        }

        private int _currentValue;
        private int _currentMax;
        private int _currentMin;

        public bool HandleVisibility { get; set; } = false;

        private void UpdateProgressName()
        {
            if (Dispatcher.HasShutdownStarted)
            {
                return;
            }

            try
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        //if not started or already done
                        if (_currentValue <= 0 || _currentValue >= _currentMax)
                        {
                            if (HandleVisibility)
                            {
                                if (Visibility != Visibility.Collapsed)
                                {
                                    Visibility = Visibility.Collapsed;
                                }
                            }
                           
                            return ;
                        }
                        if (HandleVisibility)
                        {
                            //make visible
                            if (Visibility != Visibility.Visible)
                            {
                                Visibility = Visibility.Visible;
                            }
                        }
                          

                        if (_currentMax != pbCurrentProgress.Maximum)
                        {
                            pbCurrentProgress.Maximum = _currentMax;
                        }

                        if (_currentMin != pbCurrentProgress.Minimum)
                        {
                            pbCurrentProgress.Minimum = _currentMin;
                        }

                        pbCurrentProgress.Value = _currentValue;

                        try
                        {
                            if (_currentMax == 0)
                            {
                                //lblProgressStatusMessage.Content = Name;
                                lblProgressStatusMessage.Content = "0%";
                                return;
                            }

                            var percent = (int) ((int) (100*_currentValue)/_currentMax);
                            lblProgressStatusMessage.Content = Name + $" {((int) _currentValue).ToString()}/{((int) _currentMax) + " -> " + percent.ToString() + "%"}";
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(UpdateProgressName)} Exception: {ex}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(UpdateProgressName)} Exception: {ex}");
                    }
                },DispatcherPriority.Background);

            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(UpdateProgressName)} Exception: {ex}");
            }


        }

        public void Reset()
        {
            try
            {
                _currentValue = 0;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(Reset)} Exception: {ex}");
            }

        }


        public delegate void ValueIncreasedDelegate(int value);

        public event ValueIncreasedDelegate ValueIncreased;

        private void FireValueIncreasedEvent(int value)
        {
            ValueIncreased?.Invoke(value);
        }


        public delegate void MaxChangedDelegate(int value, bool doReset);

        public event MaxChangedDelegate MaxChanged;

        private void FireMaxChangedEvent(int value, bool doReset)
        {
            MaxChanged?.Invoke(value, doReset);
        }

        public void SetMax(int max, bool doReset)
        {
            try
            {
                FireMaxChangedEvent(max, doReset);
                if (doReset)
                {
                    Reset();
                }

                _currentMax = max;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(SetMax)} Exception: {ex}");
            }
        }

        private object _incLock = new object();

        public void Increase(int value)
        {
            try
            {
                lock (_incLock)
                {
                    if (_currentValue + value < _currentMin)
                    {
                        FireValueIncreasedEvent(_currentMin);
                        _currentValue = _currentMin;
                        return;
                    }

                    if (_currentValue + value > _currentMax)
                    {
                        FireValueIncreasedEvent(_currentMax);
                        _currentValue = _currentMax;
                        return;
                    }
                    _currentValue += value;
                    FireValueIncreasedEvent(_currentValue);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(Increase)} Exception: {ex}");
            }
        }

        public void Set(int value)
        {
            try
            {
                lock (_incLock)
                {
                    if (value < _currentMin)
                    {
                        FireValueIncreasedEvent(_currentMin);
                        _currentValue = _currentMin;
                        return;
                    }

                    if (value > _currentMax)
                    {
                        FireValueIncreasedEvent(_currentMax);
                        _currentValue = _currentMax;
                        return;
                    }
                    _currentValue = value;
                    FireValueIncreasedEvent(_currentValue);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(Increase)} Exception: {ex}");
            }
        }


        public void Increase()
        {
            Increase(1);
        }

        public void Decrease(int value)
        {
            Increase(-value);
        }

        public void Decrease()
        {
            Increase(-1);
        }

        #region Implementation of IProgress<in double>

        /// <summary>
        /// Values from 0-100 for whatever max is set currently
        /// </summary>
        /// <param name="value"></param>
        public void Report(double value)
        {
            try
            {
                var current = _currentMax * (value/100);
                Set((int)current);
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(Helper_ProgressBar)}.{nameof(Report)} Report Exception: {ex}");
            }
        }

        #endregion
    }
}
