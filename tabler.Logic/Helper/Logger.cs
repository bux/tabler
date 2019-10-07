using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabler.Logic.Enums;

namespace tabler.Logic.Helper
{
    public static class Logger
    {
        public delegate void LogMessageDelegate(string message);

        public static event LogMessageDelegate LogMessageArrived;

        public static void FireLogMessageArivedEvent(string message)
        {
            LogMessageArrived?.Invoke(message);
        }

        public delegate void LogMessageWithTypeDelegate(string message, LogTypeEnum logType);

        public static event LogMessageWithTypeDelegate LogMessageWithTypeArrived;

        public static void FireLogMessageWithTyperArivedEvent(string message, LogTypeEnum logType)
        {
            LogMessageWithTypeArrived?.Invoke(message, logType);
        }
        public static void Log(string message, LogTypeEnum logType)
        {
            FireLogMessageArivedEvent(message);
            FireLogMessageWithTyperArivedEvent(message, logType);

        }
        public static void Log(string message)
        {
            Log(message, LogTypeEnum.General);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log.</param>
        public static void LogError(string message)
        {
            Log(message, LogTypeEnum.Error);
        }

        public static void LogEx(Exception ex)
        {
            Log(ex.ToString(), LogTypeEnum.Error);
        }

        public static void LogGeneral(string message)
        {
            Log(message, LogTypeEnum.General);
        }

        public static void LogInternal(string message)
        {
            Log(message, LogTypeEnum.Internal);
        }

    }
}
