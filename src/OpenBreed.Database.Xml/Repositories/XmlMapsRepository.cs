using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Xml.Items.Maps;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlMapsRepository : XmlRepositoryBase, IRepository<IMapEntry>
    {

        #region Private Fields

        private readonly XmlDbMapTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlMapsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbMapTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public string Name { get { return "Maps"; } }

        public IEnumerable<Type> EntryTypes { get { yield return typeof(XmlMapEntry); } }
        #endregion Public Properties

        #region Public Methods

        public void Add(IMapEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IMapEntry GetById(string id)
        {
            var levelDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (levelDef == null)
                throw new Exception("No Level definition found with Id: " + id);

            return levelDef;
        }

        public IMapEntry GetNextTo(IMapEntry entry)
        {
            var index = _table.Items.IndexOf((XmlMapEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IMapEntry GetPreviousTo(IMapEntry entry)
        {
            var index = _table.Items.IndexOf((XmlMapEntry)entry);

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

            var newEntry = Create(entryType) as XmlMapEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(IMapEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IMapEntry entry)
        {
            var index = _table.Items.IndexOf((XmlMapEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlMapEntry)entry;
        }

        #endregion Public Methods

    }
}
