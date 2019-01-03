using OpenBreed.Common.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Tiles;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.XmlDatabase.Items.Tiles;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlTileSetsRepository : IRepository<ITileSetEntity>
    {

        #region Private Fields

        private readonly DatabaseTileSetTableDef _table;
        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlTileSetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetTileSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntity> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ITileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public ITileSetEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ITileSetEntity GetByName(string name)
        {
            var tileSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (tileSetDef == null)
                throw new Exception("No TileSet definition found with name: " + name);

            return tileSetDef;
        }

        public ITileSetEntity GetNextTo(ITileSetEntity entry)
        {
            var index = _table.Items.IndexOf((TileSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ITileSetEntity GetPrevTo(ITileSetEntity entry)
        {
            var index = _table.Items.IndexOf((TileSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

        public void Remove(ITileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ITileSetEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}
