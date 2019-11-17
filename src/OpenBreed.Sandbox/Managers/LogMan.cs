using OpenBreed.Core;
using OpenBreed.Core.Managers;
using System;

namespace OpenBreed.Sandbox.Managers
{
    internal class LogMan : ILogMan
    {
        #region Private Fields

        private static ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        private static ConsoleColor VERBOSE_COLOR = ConsoleColor.Gray;

        private ICore core;

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

        public void Verbose(string message)
        {
            Verbose(DefaultChannel, message);
        }

        public void Verbose(int channel, string message)
        {
            WriteLineWithColor(message, VERBOSE_COLOR);
        }

        public void Warning(int channel, string message)
        {
            WriteLineWithColor(message, WARNING_COLOR);
        }

        public void Warning(string message)
        {
            Warning(DefaultChannel, message);
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