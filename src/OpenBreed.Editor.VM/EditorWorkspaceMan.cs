using OpenBreed.Common.Data;
using OpenBreed.Common.Interface.Data;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Editor.VM
{
    public class EditorWorkspaceMan : IWorkspaceMan
    {
        #region Private Fields

        private readonly XmlDatabaseMan databaseMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public EditorWorkspaceMan(IDatabase databaseMan,
                                  ILogger logger)
        {
            this.databaseMan = (XmlDatabaseMan)databaseMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        public IEnumerable<IRepository> Repositories => UnitOfWork is not null ? UnitOfWork.Repositories : Enumerable.Empty<IRepository>();

        #endregion Public Properties

        #region Public Methods

        public void OpenXmlDatabase(string databaseFilePath)
        {
            if (UnitOfWork != null)
                throw new Exception("There is already database opened.");

            databaseMan.Open(databaseFilePath);
            UnitOfWork = databaseMan.CreateUnitOfWork();

            logger.Info($"Database '{UnitOfWork.Name}' opened.");
        }

        public void CloseDatabase()
        {
            if (UnitOfWork == null)
                throw new Exception("Database not opened.");

            var databaseName = UnitOfWork.Name;

            databaseMan.Close();

            UnitOfWork = null;

            logger.Info($"Database '{databaseName}' closed.");
        }

        public void SaveDatabase()
        {
            if (UnitOfWork == null)
                throw new Exception("Database not opened.");

            UnitOfWork.Save();

            logger.Info($"Database '{UnitOfWork.Name}' saved.");
        }

        public IRepository<T> GetRepository<T>() where T : IDbEntry => UnitOfWork.GetRepository<T>();

        public IRepository GetRepository(string name) => UnitOfWork.GetRepository(name);

        public IRepository GetRepository(Type type) => UnitOfWork.GetRepository(type);

        #endregion Public Methods
    }
}