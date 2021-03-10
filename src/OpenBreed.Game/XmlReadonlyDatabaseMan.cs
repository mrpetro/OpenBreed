using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Xml;
using OpenBreed.Database.Xml.Repositories;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBreed.Game
{
    public class XmlReadonlyDatabaseMan : IReadonlyRepositoryProvider
    {
        #region Private Fields

        private const string DB_CURRENT_FOLDER_PATH = "Db.Current.FolderPath";
        private const string DB_CURRENT_FILE_NAME = "Db.Current.FileName";

        private readonly IVariableMan variables;
        private readonly Dictionary<Type, IReadonlyRepository> repositories = new Dictionary<Type, IReadonlyRepository>();

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyDatabaseMan(IVariableMan variableMan, string dbFilePath = null)
        {
            this.variables = variableMan;

            if(dbFilePath != null)
                Open(dbFilePath);

            RegisterRepos();
        }

        private void RegisterRepos()
        {
            RegisterRepository(new XmlDataSourcesRepository(GetTable<XmlDbDataSourceTableDef>()));
            RegisterRepository(new XmlAssetsRepository(GetTable<XmlDbAssetTableDef>()));
            RegisterRepository(new XmlTileSetsRepository(GetTable<XmlDbTileSetTableDef>()));
            RegisterRepository(new XmlSpriteSetsRepository(GetTable<XmlDbSpriteSetTableDef>()));
            RegisterRepository(new XmlActionSetsRepository(GetTable<XmlDbActionSetTableDef>()));
            RegisterRepository(new XmlImagesRepository(GetTable<XmlDbImageTableDef>()));
            RegisterRepository(new XmlPalettesRepository(GetTable<XmlDbPaletteTableDef>()));
            RegisterRepository(new XmlTextsRepository(GetTable<XmlDbTextTableDef>()));
            RegisterRepository(new XmlMapsRepository(GetTable<XmlDbMapTableDef>()));
            RegisterRepository(new XmlSoundsRepository(GetTable<XmlDbSoundTableDef>()));
            RegisterRepository(new XmlScriptsRepository(GetTable<XmlDbScriptTableDef>()));
            RegisterRepository(new XmlEntityTemplatesRepository(GetTable<XmlDbEntityTemplateTableDef>()));
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return XmlFilePath; } }

        public string XmlFilePath { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal XmlDatabase Data { get; private set; }

        public IEnumerable<IReadonlyRepository> Repositories { get { return repositories.Values; } }

        protected void RegisterRepository<T>(IReadonlyRepository<T> repository) where T : IEntry
        {
            repositories.Add(typeof(T), repository);
        }

        #endregion Internal Properties

        #region Public Methods

        public void Open(string xmlFilePath, DatabaseMode mode = DatabaseMode.Update)
        {
            XmlFilePath = xmlFilePath;

            Data = XmlDatabase.Load(XmlFilePath);

            var directoryPath = Path.GetDirectoryName(XmlFilePath);
            var fileName = Path.GetFileName(XmlFilePath);

            variables.RegisterVariable(typeof(string), directoryPath, DB_CURRENT_FOLDER_PATH);
            variables.RegisterVariable(typeof(string), fileName, DB_CURRENT_FILE_NAME);
        }

        public void Close()
        {
            Data = null;
            XmlFilePath = null;
            variables.UnregisterVariable(DB_CURRENT_FOLDER_PATH);
            variables.UnregisterVariable(DB_CURRENT_FILE_NAME);
        }

        #endregion Public Methods

        #region Internal Methods

        public T GetTable<T>() where T : XmlDbTableDef, new()
        {
            var table = Data.Tables.OfType<T>().FirstOrDefault();
            if (table == null)
            {
                table = new T();
                Data.Tables.Add(table);
            }
            return table;
        }

        public IReadonlyRepository<T> GetRepository<T>() where T : IEntry
        {
            throw new NotImplementedException();
        }

        public IReadonlyRepository GetRepository(string name)
        {
            throw new NotImplementedException();
        }

        public IReadonlyRepository GetRepository(Type type)
        {
            throw new NotImplementedException();
        }

        #endregion Internal Methods
    }
}