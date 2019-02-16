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
        public string Name { get { return "Tile sets"; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(ITileSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public ITileSetEntry GetById(string id)
        {
            var tileSetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (tileSetDef == null)
                throw new Exception("No TileSet definition found with Id: " + id);

            return tileSetDef;
        }

        public ITileSetEntry GetNextTo(ITileSetEntry entry)
        {
            var index = _table.Items.IndexOf((TileSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ITileSetEntry GetPreviousTo(ITileSetEntry entry)
        {
            var index = _table.Items.IndexOf((TileSetDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

        public IEntry New(string newId)
        {
            if (Find(newId) != null)
                throw new Exception($"Entry with Id '{newId}' already exist.");

            var newEntry = new TileSetDef();
            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }

        public void Remove(ITileSetEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(ITileSetEntry entry)
        {
            var index = _table.Items.IndexOf((TileSetDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (TileSetDef)entry;
        }

        #endregion Public Methods

    }
}
