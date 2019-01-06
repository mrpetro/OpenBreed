using OpenBreed.Common.Assets;
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
    public class XmlTileSetsRepository : IRepository<ITileSetEntry>
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

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ITileSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public ITileSetEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public ITileSetEntry GetByName(string name)
        {
            var tileSetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (tileSetDef == null)
                throw new Exception("No TileSet definition found with name: " + name);

            return tileSetDef;
        }

        public ITileSetEntry GetNextTo(ITileSetEntry entry)
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

        public ITileSetEntry GetPrevTo(ITileSetEntry entry)
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

        public void Remove(ITileSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ITileSetEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}
