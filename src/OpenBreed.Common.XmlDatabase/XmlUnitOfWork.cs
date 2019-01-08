using OpenBreed.Common.XmlDatabase;
using OpenBreed.Common.XmlDatabase.Items.Levels;
using OpenBreed.Common.XmlDatabase.Items.Palettes;
using OpenBreed.Common.XmlDatabase.Items.Props;
using OpenBreed.Common.XmlDatabase.Items.Sources;
using OpenBreed.Common.XmlDatabase.Items.Sprites;
using OpenBreed.Common.XmlDatabase.Items.Tiles;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.XmlDatabase.Tables.Levels;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Props;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.Images;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.Props;
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

        private readonly XmlDatabase _context;

        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        #endregion Private Fields

        #region Public Constructors

        internal XmlUnitOfWork(XmlDatabase context)
        {
            _context = context;

            RegisterRepos();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IRepository> Repositories { get { return _repositories.Values; } }

        #endregion Public Properties

        #region Public Methods

        public IRepository<T> GetRepository<T>() where T : IEntry
        {
            IRepository foundRepo;

            if (!_repositories.TryGetValue(typeof(T), out foundRepo))
                throw new Exception($"Repository of type {typeof(T)} not found.");

            return (IRepository<T>)foundRepo;
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
            RegisterRepository(new XmlAssetsRepository(this, _context));
            RegisterRepository(new XmlTileSetsRepository(this, _context));
            RegisterRepository(new XmlSpriteSetsRepository(this, _context));
            RegisterRepository(new XmlPropSetsRepository(this, _context));
            RegisterRepository(new XmlImagesRepository(this, _context));
            RegisterRepository(new XmlPalettesRepository(this, _context));
            RegisterRepository(new XmlLevelsRepository(this, _context));
            RegisterRepository(new XmlSoundsRepository(this, _context));
        }

        #endregion Private Methods
    }
}