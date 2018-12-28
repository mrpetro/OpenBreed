using OpenBreed.Common.Database;
using OpenBreed.Common.Database.Items.Levels;
using OpenBreed.Common.Database.Items.Palettes;
using OpenBreed.Common.Database.Items.Props;
using OpenBreed.Common.Database.Items.Sources;
using OpenBreed.Common.Database.Items.Sprites;
using OpenBreed.Common.Database.Items.Tiles;
using OpenBreed.Common.Database.Tables;
using OpenBreed.Common.Database.Tables.Levels;
using OpenBreed.Common.Database.Tables.Palettes;
using OpenBreed.Common.Database.Tables.Props;
using OpenBreed.Common.Database.Tables.Sources;
using OpenBreed.Common.Database.Tables.Sprites;
using OpenBreed.Common.Database.Tables.Tiles;
using OpenBreed.Common.Sources;
using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common
{
    public class XmlUnitOfWork : IUnitOfWork
    {
        private readonly XmlDatabase _context;

        #region Private Fields

        private readonly Dictionary<Type, IRepository> _repositories = new Dictionary<Type, IRepository>();

        #endregion Private Fields

        #region Public Constructors

        public XmlUnitOfWork(XmlDatabase context)
        {
            _context = context;

            RegisterRepos();
        }

        private void RegisterRepos()
        {
            RegisterRepository(new SourcesRepository(this, _context));
            RegisterRepository(new TileSetsRepository(this, _context));
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        protected void RegisterRepository<T>(IRepository<T> repository) where T : EntityBase
        {
            _repositories.Add(typeof(T), repository);
        }

        public IRepository<T> GetRepository<T>() where T : EntityBase
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

        public LevelDef GetLevelDef(string levelName)
        {
            var levelDef = _context.Data.Tables.OfType<DatabaseLevelTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == levelName);

            if (levelDef == null)
                throw new InvalidOperationException("Level '" + levelName + "' not found!");

            return levelDef;
        }

        public PaletteDef GetPaletteDef(string paletteName)
        {
            var paletteDef = _context.Data.Tables.OfType<DatabasePaletteTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == paletteName);

            if (paletteDef == null)
                throw new InvalidOperationException("Palette '" + paletteName + "' not found!");

            return paletteDef;
        }

        public PropertySetDef GetPropSetDef(string propertySetName)
        {
            var propertySetDef = _context.Data.Tables.OfType<DatabasePropertySetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == propertySetName);

            if (propertySetDef == null)
                throw new InvalidOperationException("Property set '" + propertySetName + "' not found!");

            return propertySetDef;
        }

        public SpriteSetDef GetSpriteSetDef(string spriteSetName)
        {
            var spriteSetDef = _context.Data.Tables.OfType<DatabaseSpriteSetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == spriteSetName);

            if (spriteSetDef == null)
                throw new InvalidOperationException("Sprite set '" + spriteSetName + "' not found!");

            return spriteSetDef;
        }

        public TileSetDef GetTileSetDef(string tileSetName)
        {
            var tileSetDef = _context.Data.Tables.OfType<DatabaseTileSetTableDef>().FirstOrDefault().Items.FirstOrDefault(item => item.Name == tileSetName);

            if (tileSetDef == null)
                throw new InvalidOperationException("Tile set '" + tileSetName + "' not found!");

            return tileSetDef;
        }

        public IEnumerable<DatabaseTableDef> GetTables()
        {
            return _context.Data.Tables;
        }

        #endregion Public Methods

    }
}