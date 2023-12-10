namespace Pyther.Core.Logging
{
    public class NullLogger : BaseLogger
    {
        #region ILogger

        public override void Log(LogLevel level, string message)
        {
            // NOOP
        }

        #endregion

    }
}
