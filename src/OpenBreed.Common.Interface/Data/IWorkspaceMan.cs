using OpenBreed.Database.Interface;

namespace OpenBreed.Common.Interface.Data
{
    public interface IWorkspaceMan : IRepositoryProvider
    {
        #region Public Properties

        IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        void OpenXmlDatabase(string databaseFilePath);

        void CloseDatabase();

        void SaveDatabase();

        #endregion Public Methods
    }
}