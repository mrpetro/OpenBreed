using OpenBreed.Database.Interface;

namespace OpenBreed.Common.Data
{
    public interface IWorkspaceMan
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