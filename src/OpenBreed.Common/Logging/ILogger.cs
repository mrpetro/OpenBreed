namespace OpenBreed.Common.Logging
{
    public delegate void Message(LogLevel type, int channel, string msg);

    public interface ILogger
    {
        #region Public Events

        event Message MessageAdded;

        #endregion Public Events

        #region Public Properties

        int DefaultChannel { get; }

        #endregion Public Properties

        #region Public Methods

        void Verbose(string message);

        void Verbose(int channel, string message);

        void Info(string message);

        void Info(int channel, string message);

        void Warning(string message);

        void Warning(int channel, string message);

        void Error(string message);

        void Error(int channel, string message);

        #endregion Public Methods
    }
}