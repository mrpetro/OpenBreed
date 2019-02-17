using OpenBreed.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Sprites;
using OpenBreed.Common.XmlDatabase.Tables.Sprites;
using OpenBreed.Common.XmlDatabase.Items.Sprites;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSpriteSetsRepository : XmlRepositoryBase, IRepository<ISpriteSetEntry>
    {

        #region Private Fields

        private readonly DatabaseSpriteSetTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlSpriteSetsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetSpriteSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSpriteSetEntry); } }
        public string Name { get { return "Sprite sets"; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISpriteSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public ISpriteSetEntry GetById(string id)
        {
            var spriteSetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (spriteSetDef == null)
                throw new Exception("No Source definition found with Id: " + id);

            return spriteSetDef;
        }

        public ISpriteSetEntry GetNextTo(ISpriteSetEntry entry)
        {
            var index = _table.Items.IndexOf((XmlSpriteSetEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ISpriteSetEntry GetPreviousTo(ISpriteSetEntry entry)
        {
            var index = _table.Items.IndexOf((XmlSpriteSetEntry)entry);

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

            var newEntry = Create(entryType) as XmlSpriteSetEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(ISpriteSetEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(ISpriteSetEntry entry)
        {
            var index = _table.Items.IndexOf((XmlSpriteSetEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlSpriteSetEntry)entry;
        }

        #endregion Public Methods

    }
}
