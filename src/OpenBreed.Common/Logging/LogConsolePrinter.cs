using System;

namespace OpenBreed.Common.Logging
{
    public class LogConsolePrinter
    {
        #region Private Fields

        private static ConsoleColor VERBOSE_COLOR = ConsoleColor.Gray;
        private static ConsoleColor INFO_COLOR = ConsoleColor.White;
        private static ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        private static ConsoleColor ERROR_COLOR = ConsoleColor.Red;
        private static ConsoleColor CRITICAL_COLOR = ConsoleColor.DarkRed;

        private ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public LogConsolePrinter(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public bool IsPrinting { get; private set; }

        public int DefaultChannel { get; }

        #endregion Public Properties

        #region Public Methods

        public void StartPrinting()
        {
            if (IsPrinting)
                return;

            logger.MessageAdded += Logger_MessageAdded;
            IsPrinting = true;
        }

        public void StopPrinting()
        {
            if (!IsPrinting)
                return;

            logger.MessageAdded -= Logger_MessageAdded;
            IsPrinting = false;
        }

        public void Verbose(string message) => Verbose(DefaultChannel, message);

        public void Verbose(int channel, string message)
        {
            WriteLineWithColor(message, VERBOSE_COLOR);
        }

        public void Info(string message) => Info(DefaultChannel, message);

        public void Info(int channel, string message)
        {
            WriteLineWithColor(message, INFO_COLOR);
        }

        public void Warning(string message) => Warning(DefaultChannel, message);

        public void Warning(int channel, string message)
        {
            WriteLineWithColor(message, WARNING_COLOR);
        }

        public void Error(string message) => Error(DefaultChannel, message);

        public void Error(int channel, string message)
        {
            WriteLineWithColor(message, ERROR_COLOR);
        }

        public void Critical(string message) => Critical(DefaultChannel, message);

        public void Critical(int channel, string message)
        {
            WriteLineWithColor(message, CRITICAL_COLOR);
        }

        #endregion Public Methods

        #region Private Methods

        private void Logger_MessageAdded(LogLevel type, int channel, string msg)
        {
            switch (type)
            {
                case LogLevel.Verbose:
                    Verbose(channel, msg);
                    break;

                case LogLevel.Info:
                    Info(channel, msg);
                    break;

                case LogLevel.Warning:
                    Warning(channel, msg);
                    break;

                case LogLevel.Error:
                    Error(channel, msg);
                    break;

                case LogLevel.Critical:
                    Critical(channel, msg);
                    break;

                default:
                    break;
            }
        }

        private void WriteLineWithColor(string message, ConsoleColor color)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prevColor;
        }

        #endregion Private Methods

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LogConsolePrinter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
    }
}