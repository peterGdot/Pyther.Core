using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyther.Core.Logging
{
    public interface ILogger
    {
        /// <summary>Enable/Disable Logging.</summary>
        public bool IsEnabled { get; set; }

        /// <summary>Minimum log level. All logs with lower priorities will be skipped.</summary>
        public LogLevel MinLevel { get; set; }

        /// <summary>Enable/Disable logging timestamp.</summary>
        public bool UseTimestamp { get; set; }

        /// <summary>Enable/Disable logging logtype.</summary>
        public bool UseLogtype { get; set; }

        /// <summary>Timestamp format used for logging.</summary>
        public string TimeStampFormat { get; set; }

        void Log(LogLevel level, string message);
    }
}
