using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Maps;
using OpenBreed.Common.XmlDatabase.Items.Palettes;
using OpenBreed.Common.XmlDatabase.Items.Actions;
using OpenBreed.Common.XmlDatabase.Items.Assets;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.XmlDatabase.Items.Tiles;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.XmlDatabase.Tables.Maps;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Actions;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.Images;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Actions;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.XmlDatabase.Repositories;

namespace OpenBreed.Common.XmlDatabase
{
    internal class XmlUnitOfWork : IUnitOfWork
    {

        #region Private Fields

        private readonly XmlDatabaseMan _context;

        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        #endregion Private Fields

        #region Internal Constructors

        internal XmlUnitOfWork(XmlDatabaseMan context)
        {
            _context = context;

            RegisterRepos();
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Name { get { return _context.Name; } }

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
            _context.Save();
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
            RegisterRepository(new XmlAssetsRepository(_context));
            RegisterRepository(new XmlTileSetsRepository(_context));
            RegisterRepository(new XmlSpriteSetsRepository(_context));
            RegisterRepository(new XmlActionSetsRepository(_context));
            RegisterRepository(new XmlImagesRepository(_context));
            RegisterRepository(new XmlPalettesRepository( _context));
            RegisterRepository(new XmlMapsRepository(_context));
            RegisterRepository(new XmlSoundsRepository(_context));
        }

        #endregion Private Methods

    }
}