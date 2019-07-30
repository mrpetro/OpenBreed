namespace OpenBreed.Core
{
    public interface ILogMan
    {
        #region Public Properties

        int DefaultChannel { get; }

        #endregion Public Properties

        #region Public Methods

        void Warning(string message);

        void Warning(int channel, string message);

        #endregion Public Methods
    }
}