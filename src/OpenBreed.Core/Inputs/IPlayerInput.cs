namespace OpenBreed.Core.Inputs
{
    public interface IPlayerInput
    {
        #region Public Methods

        void Apply(Player player);

        void Reset(Player player);

        #endregion Public Methods
    }
}