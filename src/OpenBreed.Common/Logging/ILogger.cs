namespace OpenBreed.Common.Logging
{
    public delegate void Message(LogType type, string msg);

    public interface ILogger
    {
        #region Public Events

        event Message MessageAdded;

        #endregion Public Events

        #region Public Methods

        void Info(string msg);

        void Warning(string msg);

        void Error(string msg);

        void Critical(string msg);

        void Success(string msg);

        #endregion Public Methods
    }
}