namespace OpenBreed.Core.Common.Systems
{
    public interface IWorldSystemBuilder<T> where T : IWorldSystem
    {
        #region Public Methods

        T Build();

        #endregion Public Methods
    }
}