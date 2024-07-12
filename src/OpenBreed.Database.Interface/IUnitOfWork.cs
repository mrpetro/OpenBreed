namespace OpenBreed.Database.Interface
{
    public interface IUnitOfWork : IRepositoryProvider
    {
        #region Public Properties

        string Name { get; }

        #endregion Public Properties

        #region Public Methods

        void Save();

        #endregion Public Methods
    }
}