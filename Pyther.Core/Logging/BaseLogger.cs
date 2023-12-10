using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyther.Core.Logging
{
    public abstract class BaseLogger : ILogger
    {
        public bool IsEnabled { get; set; } = true;
        public LogLevel MinLevel { get; set; }
        public bool UseTimestamp { get; set; } = false;
        public bool UseLogtype { get; set; } = false;
        public string TimeStampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";
        public bool Lock { get; set; } = false;        

        public abstract void Log(LogLevel level, string message);
    }
}
