using log4net;
using System;

namespace Common
{
    public class Log4netHelper
    {
        private ILog log;
        private Type type;
        public Log4netHelper(ILog log,Type type)
        {
            this.log = log;
            this.type = type;
        }

        /// <summary>
        /// log4net日志，Level级别：DEBUG<INFO<WARN<ERROR<FATAL
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logType">日志类型：Debug<Info<Warn<Error<Fatal</param>
        /// <param name="exception">异常：默认null</param>
        public void Log(string message, LogType logType, Exception exception = null)
        {
            string formatMessage = GetFormatMessage(message);
            switch (logType)
            {
                case LogType.Debug:
                    log.Debug(formatMessage, exception);
                    break;
                case LogType.Info:
                    log.Info(formatMessage, exception);
                    break;
                case LogType.Warn:
                    log.Warn(formatMessage, exception);
                    break;
                case LogType.Error:
                    log.Error(formatMessage, exception);
                    break;
                case LogType.Fatal:
                    log.Fatal(formatMessage, exception);
                    break;

            }

        }

        private string GetFormatMessage(string message)
        {
            return string.Format("{0}:{1}", type, message);
        }
    }

    /// <summary>
    /// Level级别：Debug<Info<Warn<Error<Fatal
    /// </summary>
    public enum LogType
    {
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatal = 5
    }
}
