using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Xml;
using OpenBreed.Database.Xml.Repositories;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenBreed.Database.Xml
{
    public class XmlReadonlyDatabaseMan : IRepositoryProvider
    {
        #region Private Fields

        private const string DB_CURRENT_FOLDER_PATH = "Db.Current.FolderPath";
        private const string DB_CURRENT_FILE_NAME = "Db.Current.FileName";

        private readonly IVariableMan variables;
        private readonly Dictionary<Type, IRepository> repositories = new Dictionary<Type, IRepository>();
        private readonly Dictionary<string, XmlDbTableDef> tables = new Dictionary<string, XmlDbTableDef>();

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyDatabaseMan(IVariableMan variableMan, string dbFilePath = null)
        {
            this.variables = variableMan;

            if(dbFilePath != null)
                Open(dbFilePath);

            RegisterTables();
            RegisterRepos();
        }

        private void RegisterTable(XmlDbTableDef dbTable)
        {
            tables.Add(dbTable.Name, dbTable);
        }

        private void RegisterTables()
        {
            RegisterTable(GetTable<XmlDbDataSourceTableDef>());
            RegisterTable(GetTable<XmlDbAssetTableDef>());
            RegisterTable(GetTable<XmlDbTileAtlasTableDef>());
            RegisterTable(GetTable<XmlDbTileStampTableDef>());
            RegisterTable(GetTable<XmlDbSpriteAtlasTableDef>());
            RegisterTable(GetTable<XmlDbActionSetTableDef>());
            RegisterTable(GetTable<XmlDbImageTableDef>());
            RegisterTable(GetTable<XmlDbPaletteTableDef>());
            RegisterTable(GetTable<XmlDbTextTableDef>());
            RegisterTable(GetTable<XmlDbMapTableDef>());
            RegisterTable(GetTable<XmlDbSoundTableDef>());
            RegisterTable(GetTable<XmlDbSongTableDef>());
            RegisterTable(GetTable<XmlDbScriptTableDef>());
            RegisterTable(GetTable<XmlDbAnimationTableDef>());
            RegisterTable(GetTable<XmlDbEntityTemplateTableDef>());
        }

        private void RegisterRepos()
        {
            RegisterRepository(new XmlDataSourcesRepository(GetTable<XmlDbDataSourceTableDef>()));
            RegisterRepository(new XmlAssetsRepository(GetTable<XmlDbAssetTableDef>()));
            RegisterRepository(new XmlTileAtlasRepository(GetTable<XmlDbTileAtlasTableDef>()));
            RegisterRepository(new XmlTileStampsRepository(GetTable<XmlDbTileStampTableDef>()));
            RegisterRepository(new XmlSpriteAtlasRepository(GetTable<XmlDbSpriteAtlasTableDef>()));
            RegisterRepository(new XmlActionSetsRepository(GetTable<XmlDbActionSetTableDef>()));
            RegisterRepository(new XmlImagesRepository(GetTable<XmlDbImageTableDef>()));
            RegisterRepository(new XmlPalettesRepository(GetTable<XmlDbPaletteTableDef>()));
            RegisterRepository(new XmlTextsRepository(GetTable<XmlDbTextTableDef>()));
            RegisterRepository(new XmlMapsRepository(GetTable<XmlDbMapTableDef>()));
            RegisterRepository(new XmlSoundsRepository(GetTable<XmlDbSoundTableDef>()));
            RegisterRepository(new XmlSongsRepository(GetTable<XmlDbSongTableDef>()));
            RegisterRepository(new XmlScriptsRepository(GetTable<XmlDbScriptTableDef>()));
            RegisterRepository(new XmlAnimationsRepository(GetTable<XmlDbAnimationTableDef>()));
            RegisterRepository(new XmlEntityTemplatesRepository(GetTable<XmlDbEntityTemplateTableDef>()));
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return XmlFilePath; } }

        public string XmlFilePath { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal XmlDatabase Data { get; private set; }

        public IEnumerable<IRepository> Repositories { get { return repositories.Values; } }

        protected void RegisterRepository<T>(IRepository<T> repository) where T : IDbEntry
        {
            repositories.Add(typeof(T), repository);
        }

        #endregion Internal Properties

        #region Public Methods

        public void Open(string xmlFilePath)
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

        public IRepository<T> GetRepository<T>() where T : IDbEntry
        {
            return (IRepository<T>)GetRepository(typeof(T));
        }

        public IRepository GetRepository(string name)
        {
            return repositories.Values.FirstOrDefault(item => item.Name == name);
        }

        public IRepository GetRepository(Type type)
        {
            IRepository foundRepo;

            if (!repositories.TryGetValue(type, out foundRepo))
                throw new Exception($"Repository of type {type} not found.");

            return foundRepo;
        }

        #endregion Internal Methods
    }
}