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
    public class XmlPalettesRepository : XmlRepositoryBase, IRepository<IPaletteEntry>
    {

        #region Private Fields

        private readonly DatabasePaletteTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlPalettesRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetPaletteTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IEnumerable<Type> EntryTypes { get { yield return typeof(XmlPaletteEntry); } }
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
            var index = _table.Items.IndexOf((XmlPaletteEntry)entry);

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
            var index = _table.Items.IndexOf((XmlPaletteEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index--;

            if (index >= 0)
                return _table.Items[index];
            else
                return null;
        }

        public IEntry New(string newId, Type entryType = null)
        {
            if (Find(newId) != null)
                throw new Exception($"Entry with Id '{newId}' already exist.");

            if (entryType == null)
                entryType = EntryTypes.FirstOrDefault();

            var newEntry = Create(entryType) as XmlPaletteEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(IPaletteEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IPaletteEntry entry)
        {
            var index = _table.Items.IndexOf((XmlPaletteEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlPaletteEntry)entry;
        }

        #endregion Public Methods

    }
}
