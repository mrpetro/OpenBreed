namespace OpenBreed.Common.Interface
{
    public interface IDataLoaderFactory
    {
        #region Public Methods

        TInterface GetLoader<TInterface>() where TInterface : IDataLoader;

        #endregion Public Methods
    }
}