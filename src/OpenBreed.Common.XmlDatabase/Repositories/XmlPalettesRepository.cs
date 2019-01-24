using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Palettes;
using OpenBreed.Common.XmlDatabase.Items.Palettes;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlPalettesRepository : IRepository<IPaletteEntry>
    {
        #region Private Fields

        private readonly DatabasePaletteTableDef _table;

        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlPalettesRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetPaletteTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public IUnitOfWork UnitOfWork { get; }

        public string Name { get { return "Palettes"; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(IPaletteEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IPaletteEntry GetById(string id)
        {
            var paletteDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (paletteDef == null)
                throw new Exception("No Palette definition found with Id: " + id);

            return paletteDef;
        }

        public IPaletteEntry GetNextTo(IPaletteEntry entry)
        {
            var index = _table.Items.IndexOf((PaletteDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IPaletteEntry GetPreviousTo(IPaletteEntry entry)
        {
            var index = _table.Items.IndexOf((PaletteDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

        public void Remove(IPaletteEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IPaletteEntry entry)
        {
            var index = _table.Items.IndexOf((PaletteDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (PaletteDef)entry;
        }

        #endregion Public Methods

    }
}
