namespace OpenBreed.Input.Interface
{
    public interface IPlayerInput
    {
        #region Public Properties

        bool Changed { get; }

        #endregion Public Properties

        #region Public Methods

        void Reset(IPlayer player);

        #endregion Public Methods
    }
}