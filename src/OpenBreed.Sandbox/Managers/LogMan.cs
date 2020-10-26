using OpenBreed.Common.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Sandbox.Managers
{
    internal class LogMan : ILogger
    {
        #region Private Fields

        private static ConsoleColor VERBOSE_COLOR = ConsoleColor.Gray;
        private static ConsoleColor INFO_COLOR = ConsoleColor.White;
        private static ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        private static ConsoleColor ERROR_COLOR = ConsoleColor.Red;
        private static ConsoleColor CRITICAL_COLOR = ConsoleColor.DarkRed;

        private ICore core;

        public event Message MessageAdded;

        #endregion Private Fields

        #region Internal Constructors

        internal LogMan(ICore core)
        {
            this.core = core;
        }

        #endregion Internal Constructors

        #region Public Properties

        public int DefaultChannel { get; }

        #endregion Public Properties

        #region Public Methods

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

        private void WriteLineWithColor(string message, ConsoleColor color)
        {
            var prevColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = prevColor;
        }

        #endregion Private Methods
    }
}