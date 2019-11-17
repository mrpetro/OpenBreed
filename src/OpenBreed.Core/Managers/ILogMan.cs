namespace OpenBreed.Core.Managers
{
    public interface ILogMan
    {
        #region Public Properties

        int DefaultChannel { get; }

        #endregion Public Properties

        #region Public Methods

        void Verbose(string message);

        void Verbose(int channel, string message);

        void Warning(string message);

        void Warning(int channel, string message);

        #endregion Public Methods
    }
}