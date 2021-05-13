using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Xml.Repositories;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Database.Xml
{
    internal class XmlUnitOfWork : IUnitOfWork
    {
        #region Private Fields

        private readonly XmlDatabaseMan context;

        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        #endregion Private Fields

        #region Internal Constructors

        internal XmlUnitOfWork(XmlDatabaseMan context)
        {
            this.context = context;

            RegisterRepos();
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name { get { return context.Name; } }

        public IEnumerable<IRepository> Repositories { get { return _repositories.Values; } }

        #endregion Public Properties

        #region Public Methods

        public IRepository<T> GetRepository<T>() where T : IEntry
        {
            return (IRepository<T>)GetRepository(typeof(T));
        }

        public IRepository GetRepository(string name)
        {
            return _repositories.Values.FirstOrDefault(item => item.Name == name);
        }

        public IRepository GetRepository(Type type)
        {
            IRepository foundRepo;

            if (!_repositories.TryGetValue(type, out foundRepo))
                throw new Exception($"Repository of type {type} not found.");

            return foundRepo;
        }

        public void Save()
        {
            context.Save();
        }

        #endregion Public Methods

        #region Protected Methods

        protected void RegisterRepository<T>(IRepository<T> repository) where T : IEntry
        {
            _repositories.Add(typeof(T), repository);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RegisterRepos()
        {
            RegisterRepository(new XmlDataSourcesRepository(context.GetTable<XmlDbDataSourceTableDef>()));
            RegisterRepository(new XmlAssetsRepository(context.GetTable<XmlDbAssetTableDef>()));
            RegisterRepository(new XmlTileSetsRepository(context.GetTable<XmlDbTileSetTableDef>()));
            RegisterRepository(new XmlSpriteSetsRepository(context.GetTable<XmlDbSpriteSetTableDef>()));
            RegisterRepository(new XmlActionSetsRepository(context.GetTable<XmlDbActionSetTableDef>()));
            RegisterRepository(new XmlImagesRepository(context.GetTable<XmlDbImageTableDef>()));
            RegisterRepository(new XmlPalettesRepository(context.GetTable<XmlDbPaletteTableDef>()));
            RegisterRepository(new XmlTextsRepository(context.GetTable<XmlDbTextTableDef>()));
            RegisterRepository(new XmlMapsRepository(context.GetTable<XmlDbMapTableDef>()));
            RegisterRepository(new XmlSoundsRepository(context.GetTable<XmlDbSoundTableDef>()));
            RegisterRepository(new XmlScriptsRepository(context.GetTable<XmlDbScriptTableDef>()));
            RegisterRepository(new XmlAnimationsRepository(context.GetTable<XmlDbAnimationTableDef>()));
            RegisterRepository(new XmlEntityTemplatesRepository(context.GetTable<XmlDbEntityTemplateTableDef>()));
        }

        #endregion Private Methods
    }
}