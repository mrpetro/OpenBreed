using Microsoft.Extensions.Logging;

namespace OpenBreed.Common.Interface.Logging
{
    public delegate void Message(LogLevel type, string msg);

    public interface ILoggerClient
    {
        #region Public Events

        event Message MessageAdded;

        #endregion Public Events

        #region Public Methods

        void OnMessage(LogLevel type, string msg);

        #endregion Public Methods
    }
}