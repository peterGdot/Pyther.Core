using System.Text;

namespace Pyther.Core.Logging
{
    public static class Log
    {
        private static LogDataInstance? instance = null;
        private static StringBuilder sb = new StringBuilder(5);

        public static LogDataInstance Instance {
            get
            {
                instance ??= new LogDataInstance();
                return instance;
            }
            set
            {
                if (value != instance)
                {
                    instance = value;
                }
            }
        }

        public static bool Register(ILogger logger)
        {
            if (!Log.Instance.logger.Contains(logger))
            {
                Log.Instance.logger.Add(logger);
            }
            return false;
        }

        public static bool Unregister(ILogger logger)
        {
            return Log.Instance.logger.Remove(logger);
        }

        public static void Write(LogLevel type, string str)
        {
            foreach (ILogger logger in Instance.logger)
            {
                if (type >= logger.MinLevel || type == LogLevel.Temp)
                {
                    logger.Log(type, str);
                }
            }
        }

        public static void WriteFormat(LogLevel type, string format, params object[] args)
        {
            foreach (ILogger logger in Instance.logger)
            {
                if (type >= logger.MinLevel || type == LogLevel.Temp)
                {
                    logger.Log(type, String.Format(format, args));
                }
            }
        }

        public static void WriteLine(LogLevel type, string data)
        {
            DateTime now = DateTime.Now;
            foreach (ILogger logger in Instance.logger)
            {
                if (logger.IsEnabled && (type >= logger.MinLevel || type == LogLevel.Temp))
                {
                    sb.Clear();
                    if (logger.UseTimestamp)
                    {
                        sb.Append(now.ToString(logger.TimeStampFormat)).Append(" | ");
                    }
                    if (logger.UseLogtype)
                    {
                        sb.Append(type.ToString().PadRight(7)).Append(" | ");
                    }
                    sb.Append(data).Append(Environment.NewLine);
                    logger.Log(type, sb.ToString());
                }
            }
        }

        public static void WriteFormatLine(LogLevel type, string format, params object[] args)
        {
            DateTime now = DateTime.Now;
            string data = String.Format(format + Environment.NewLine, args);
            foreach (ILogger logger in Instance.logger)
            {
                if (logger.IsEnabled && (type >= logger.MinLevel || type == LogLevel.Temp))
                {
                    sb.Clear();
                    if (logger.UseTimestamp)
                    {
                        sb.Append(now.ToString(logger.TimeStampFormat)).Append(" | ");
                    }
                    if (logger.UseLogtype)
                    {
                        sb.Append(type.ToString().PadRight(7)).Append(" | ");
                    }
                    sb.Append(data);
                    logger.Log(type, sb.ToString());
                }
            }
        }

        public static void Temp(string str) => WriteLine(LogLevel.Temp, str);
        public static void Debug(string str) => WriteLine(LogLevel.Debug, str);
        public static void Process(string str) => WriteLine(LogLevel.Process, str);
        public static void Info(string str) => WriteLine(LogLevel.Info, str);
        public static void Warning(string str) => WriteLine(LogLevel.Warning, str);
        public static void Error(string str) => WriteLine(LogLevel.Error, str);

        public static void TempF(string format, params object[] args) => WriteFormatLine(LogLevel.Temp, format, args);
        public static void DebugF(string format, params object[] args) => WriteFormatLine(LogLevel.Debug, format, args);
        public static void ProcessF(string format, params object[] args) => WriteFormatLine(LogLevel.Process, format, args);
        public static void InfoF(string format, params object[] args) => WriteFormatLine(LogLevel.Info, format, args);
        public static void WarningF(string format, params object[] args) => WriteFormatLine(LogLevel.Warning, format, args);
        public static void ErrorF(string format, params object[] args) => WriteFormatLine(LogLevel.Error, format, args);
    }
}
