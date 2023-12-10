namespace Pyther.Core.Logging
{
    public class ConsoleColoredLogger : BaseLogger
    {
        private bool isColored = true;
        private readonly object lockObject = new();

        /// <summary>
        /// Enable/Disable console coloring.
        /// </summary>
        public bool IsColored
        {
            get => isColored;
            set
            {
                if (value != isColored)
                {
                    isColored = value;
                    if (!isColored)
                    {
                        Console.ResetColor();
                    }
                }                
            }
        }

        public Dictionary<LogLevel, ConsoleColor> Colors { get; private set; } = new Dictionary<LogLevel, ConsoleColor>()
        {
            { LogLevel.Temp, ConsoleColor.DarkGray },
            { LogLevel.Debug, ConsoleColor.Cyan },
            { LogLevel.Process, ConsoleColor.Gray },
            { LogLevel.Info, ConsoleColor.Blue },
            { LogLevel.Warning, ConsoleColor.DarkYellow },
            { LogLevel.Error, ConsoleColor.Red }
        };

        #region ILogger

        public override void Log(LogLevel level, string message)
        {
            Log(Colors[level], message);
        }

        public void Log(System.ConsoleColor color, string message)
        {
            if (Lock)
            {
                lock (lockObject)
                {
                    ConsoleColor savedColor = Console.ForegroundColor;
                    if (IsColored)
                    {
                        Console.ForegroundColor = color;
                    }
                    Console.Write(message);
                    if (IsColored)
                    {
                        Console.ForegroundColor = savedColor;
                    }
                }
            }
            else
            {
                ConsoleColor savedColor = Console.ForegroundColor;
                if (IsColored)
                {
                    Console.ForegroundColor = color;
                }
                Console.Write(message);
                if (IsColored)
                {
                    Console.ForegroundColor = savedColor;
                }
            }
        }

        #endregion

    }
}
