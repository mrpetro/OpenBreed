using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Images;
using OpenBreed.Common.XmlDatabase.Tables;
using OpenBreed.Common.Sounds;
using OpenBreed.Common.XmlDatabase.Items.Sounds;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlSoundsRepository : XmlRepositoryBase, IRepository<ISoundEntry>
    {

        #region Private Fields

        private readonly DatabaseSoundTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlSoundsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetSoundTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSoundEntry); } }
        public string Name { get { return "Sounds"; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(ISoundEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public ISoundEntry GetById(string id)
        {
            var entry = _table.Items.FirstOrDefault(item => item.Id == id);
            if (entry == null)
                throw new Exception("No Sound entry found with Id: " + id);

            return entry;
        }

        public ISoundEntry GetNextTo(ISoundEntry entry)
        {
            var index = _table.Items.IndexOf((XmlSoundEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ISoundEntry GetPreviousTo(ISoundEntry entry)
        {
            var index = _table.Items.IndexOf((XmlSoundEntry)entry);

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

            var newEntry = Create(entryType) as XmlSoundEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(ISoundEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(ISoundEntry entry)
        {
            var index = _table.Items.IndexOf((XmlSoundEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlSoundEntry)entry;
        }

        #endregion Public Methods

    }
}
