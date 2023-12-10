namespace Pyther.Core.Logging
{
    public class ConsoleLogger : BaseLogger
    {
        private readonly object lockObject = new();

        #region ILogger

        public override void Log(LogLevel level, string message)
        {
            if (Lock)
            {
                lock (lockObject)
                {
                    Console.Write(message);
                }
            }
            else
            {
                Console.Write(message);
            }
        }

        #endregion

    }
}
