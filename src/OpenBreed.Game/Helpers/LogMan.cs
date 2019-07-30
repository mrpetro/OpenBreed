using OpenBreed.Core;
using System;

namespace OpenBreed.Game.Helpers
{
    internal class LogMan : ILogMan
    {
        #region Private Fields

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

        public void Warning(int channel, string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Warning(DefaultChannel, message);
        }

        #endregion Public Methods
    }
}