﻿using System.Text;

namespace Pyther.Core.Logging
{
    public static class Log
    {
        private static LogDataInstance? instance = null;

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
                    logger.Log(type, string.Format(format, args));
                }
            }
        }

        public static void WriteLine(LogLevel type, string? data)
        {
            data ??= "<NULL>";
            DateTime now = DateTime.Now;
            foreach (ILogger logger in Instance.logger)
            {
                if (logger.IsEnabled && (type >= logger.MinLevel || type == LogLevel.Temp))
                {
                    StringBuilder sb = new(5);
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
            string data = string.Format(format + Environment.NewLine, args);
            foreach (ILogger logger in Instance.logger)
            {
                if (logger.IsEnabled && (type >= logger.MinLevel || type == LogLevel.Temp))
                {
                    StringBuilder sb = new(5);
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

        public static void Temp(string? str) => WriteLine(LogLevel.Temp, str);
        public static void Debug(string? str) => WriteLine(LogLevel.Debug, str);
        public static void Process(string? str) => WriteLine(LogLevel.Process, str);
        public static void Info(string? str) => WriteLine(LogLevel.Info, str);
        public static void Warning(string? str) => WriteLine(LogLevel.Warning, str);
        public static void Error(string? str) => WriteLine(LogLevel.Error, str);

        public static void Temp(object? obj) => WriteLine(LogLevel.Temp, obj?.ToString() ?? "<NULL>");
        public static void Debug(object? obj) => WriteLine(LogLevel.Debug, obj?.ToString() ?? "<NULL>");
        public static void Process(object? obj) => WriteLine(LogLevel.Process, obj?.ToString() ?? "<NULL>");
        public static void Info(object? obj) => WriteLine(LogLevel.Info, obj?.ToString() ?? "<NULL>");
        public static void Warnings(object? obj) => WriteLine(LogLevel.Warning, obj?.ToString() ?? "<NULL>");
        public static void Error(object? obj) => WriteLine(LogLevel.Error, obj?.ToString() ?? "<NULL>");

        public static void Temp(Exception? ex) => Exception(ex, LogLevel.Temp);
        public static void Debug(Exception? ex) => Exception(ex, LogLevel.Debug);
        public static void Process(Exception? ex) => Exception(ex, LogLevel.Process);
        public static void Info(Exception? ex) => Exception(ex, LogLevel.Info);
        public static void Warnings(Exception? ex) => Exception(ex, LogLevel.Warning);
        public static void Error(Exception? ex) => Exception(ex, LogLevel.Error);

        public static void FTemp(string format, params object[] args) => WriteFormatLine(LogLevel.Temp, format, args);
        public static void FDebug(string format, params object[] args) => WriteFormatLine(LogLevel.Debug, format, args);
        public static void FProcess(string format, params object[] args) => WriteFormatLine(LogLevel.Process, format, args);
        public static void FInfo(string format, params object[] args) => WriteFormatLine(LogLevel.Info, format, args);
        public static void FWarning(string format, params object[] args) => WriteFormatLine(LogLevel.Warning, format, args);
        public static void FError(string format, params object[] args) => WriteFormatLine(LogLevel.Error, format, args);

        private static void Exception(Exception? ex, LogLevel level = LogLevel.Error)
        {
            if (ex == null) return;
            StringBuilder sb = new(9);
            sb.Append($"'{ex.GetType().Name}' Exception:").Append(Environment.NewLine);
            sb.Append($"Message:{Environment.NewLine}   '{ex.Message}'").Append(Environment.NewLine);
            sb.Append($"Type:{Environment.NewLine}   '{ex.GetType().FullName}'").Append(Environment.NewLine);            
            sb.Append($"StackTrace:{Environment.NewLine}{ex.StackTrace}").Append(Environment.NewLine);
            if ((ex.InnerException != null))
            {
                sb.Append($"{Environment.NewLine}Inner Exeption:{Environment.NewLine}");
            }
            WriteLine(level, sb.ToString());

            while ((ex = ex.InnerException) != null)
            {                
                Log.Exception(ex, level);
            }
            
        }
    }
}
