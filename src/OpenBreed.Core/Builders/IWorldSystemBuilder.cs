using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Builders
{
    public interface IWorldSystemBuilder<T> where T : IWorldSystem
    {
        #region Public Methods

        T Build();

        #endregion Public Methods
    }
}