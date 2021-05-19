namespace OpenBreed.Common
{
    public interface IDataLoaderFactory
    {
        #region Public Methods

        IDataLoader<TInterface> GetLoader<TInterface>();

        #endregion Public Methods
    }
}