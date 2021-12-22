namespace OpenBreed.Common
{
    public interface IDataLoaderFactory
    {
        #region Public Methods

        TInterface GetLoader<TInterface>() where TInterface : IDataLoader;

        #endregion Public Methods
    }
}