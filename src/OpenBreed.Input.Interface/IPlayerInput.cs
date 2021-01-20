using OpenBreed.Core;

namespace OpenBreed.Input.Interface
{
    public interface IPlayerInput
    {
        #region Public Methods

        void Apply(IPlayer player);

        void Reset(IPlayer player);

        #endregion Public Methods
    }
}