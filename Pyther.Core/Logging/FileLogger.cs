namespace Pyther.Core.Logging
{
    /// <summary>
    /// A logfile logger.
    /// - file is created on first use
    /// - path is created if it doesn't exists
    /// - data are flushed on after every entry
    /// - optional log logtype
    /// - optional log timestamp
    /// - optional timestamp format

    /// </summary>
    public class FileLogger : BaseLogger
    {
        private string path;
        private bool appendMode = true;
        private readonly object lockObject = new();

        /// <summary>
        /// Set to false to create a new file on startup.
        /// </summary>
        public bool AppendMode {
            get => appendMode;
            set {
                if (value != appendMode)
                {
                    appendMode = value;
                    if (!appendMode)
                    {
                        System.IO.File.Delete(path);
                    }
                }
            }
        }

        public FileLogger(string path)
        {
            this.path = path;
            // ensure logfile path exists
            if (Path.GetDirectoryName(this.path) is string directory)
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Delete all logfile older than the given amount of seconds and the optional given pattern.
        /// Only files in the logfile directory will be deleted (subdirectories are ignored).
        /// </summary>
        /// <param name="seconds">The file must be older (last written) than the given amount of seconds (for example 90 * 60 * 60 * 24 for 90 days).</param>
        /// <param name="pattern">The logfile pattern to delete ("*.log" by default)</param>
        /// <param name="ignoreExceptions">Set to 'false', if this method should fire exceptions ('true' by default)</param>
        /// <returns>The amout of deleted files.</returns>
        public int DeleteOldFiles(int seconds, string pattern = "*.log", bool ignoreExceptions = true)
        {
            int total = 0;
            if (Path.GetDirectoryName(this.path) is string directory)
            {
                foreach (var fi in new DirectoryInfo(directory).GetFiles(pattern, SearchOption.TopDirectoryOnly))
                {
                    if (fi.LastWriteTime < DateTime.Now.AddSeconds(-seconds))
                    {
                        if (!fi.FullName.Equals(path))
                        {
                            try
                            {
                                fi.Delete();
                                total++;
                            }
                            catch (Exception)
                            {
                                if (!ignoreExceptions)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            return total;
        }

        #region ILogger

        public override void Log(LogLevel level, string message)
        {
            if (Lock)
            {
                lock (lockObject)
                {
                    using (StreamWriter sw = System.IO.File.AppendText(path))
                    {
                        sw.Write(message);
                    }
                }
            } else
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.Write(message);
                }
            }
        }

        #endregion
    }
}
