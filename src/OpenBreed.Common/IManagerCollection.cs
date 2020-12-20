namespace OpenBreed.Common
{
    public interface IManagerCollection
    {
        #region Public Methods

        TManager GetManager<TManager>();

        void AddSingleton<TInterface>(TInterface instance);

        #endregion Public Methods
    }
}