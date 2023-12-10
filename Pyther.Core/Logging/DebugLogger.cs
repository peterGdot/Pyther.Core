using System.Diagnostics;

namespace Pyther.Core.Logging
{
    public class DebugLogger : BaseLogger
    {
        private readonly object lockObject = new();

        #region ILogger

        public override void Log(LogLevel level, string message)
        {
            if (Lock)
            {
                lock(lockObject)
                {
                    Debug.Write(message);
                }
            } else
            {
                Debug.Write(message);
            }
        }

        #endregion

    }
}
