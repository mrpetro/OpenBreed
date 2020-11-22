using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Xml.Tables;
using System;
using System.IO;
using System.Linq;

namespace OpenBreed.Database.Xml
{
    public class XmlDatabaseMan : IDatabase
    {
        #region Private Fields

        private const string DB_CURRENT_FOLDER_PATH = "Db.Current.FolderPath";
        private const string DB_CURRENT_FILE_NAME = "Db.Current.FileName";

        private readonly VariableMan variables;

        #endregion Private Fields

        #region Public Constructors

        public XmlDatabaseMan(VariableMan variableMan)
        {
            this.variables = variableMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public DatabaseMode Mode { get; private set; }

        public string Name { get { return XmlFilePath; } }

        public string XmlFilePath { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal XmlDatabase Data { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public void Open(string xmlFilePath, DatabaseMode mode = DatabaseMode.Update)
        {
            XmlFilePath = xmlFilePath;
            Mode = mode;

            if (Mode == DatabaseMode.Create)
                Data = new XmlDatabase();
            else
                Data = XmlDatabase.Load(XmlFilePath);

            var directoryPath = Path.GetDirectoryName(XmlFilePath);
            var fileName = Path.GetFileName(XmlFilePath);

            variables.RegisterVariable(typeof(string), directoryPath, DB_CURRENT_FOLDER_PATH);
            variables.RegisterVariable(typeof(string), fileName, DB_CURRENT_FILE_NAME);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new XmlUnitOfWork(this);
        }

        public void Save()
        {
            if (Mode == DatabaseMode.Update)
                Data.Save(XmlFilePath);
            else
                throw new InvalidOperationException("Database opened in 'ReadOnly' mode.");
        }

        public void Close()
        {
            Data = null;
            XmlFilePath = null;
            variables.UnregisterVariable(DB_CURRENT_FOLDER_PATH);
            variables.UnregisterVariable(DB_CURRENT_FILE_NAME);
            Mode = DatabaseMode.Unset;
        }

        #endregion Public Methods

        #region Internal Methods

        internal T GetTable<T>() where T : XmlDbTableDef, new()
        {
            var table = Data.Tables.OfType<T>().FirstOrDefault();
            if (table == null)
            {
                table = new T();
                Data.Tables.Add(table);
            }
            return table;
        }

        #endregion Internal Methods
    }
}