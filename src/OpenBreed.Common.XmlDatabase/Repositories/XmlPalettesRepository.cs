using OpenBreed.Common.XmlDatabase.Tables.Images;
using OpenBreed.Common.XmlDatabase.Tables.Palettes;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Tiles;
using OpenBreed.Common.Sources;
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

        #endregion Public Properties

        #region Public Methods

        public void Add(IPaletteEntry entity)
        {
            throw new NotImplementedException();
        }

        public IPaletteEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IPaletteEntry GetByName(string name)
        {
            var paletteDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (paletteDef == null)
                throw new Exception("No Palette definition found with name: " + name);

            return paletteDef;
        }

        public IPaletteEntry GetNextTo(IPaletteEntry entry)
        {
            var index = _table.Items.IndexOf((PaletteDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IPaletteEntry GetPrevTo(IPaletteEntry entry)
        {
            var index = _table.Items.IndexOf((PaletteDef)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Name} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

        public void Remove(IPaletteEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IPaletteEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}
