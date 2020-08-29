using OpenBreed.Common;
using OpenBreed.Common.Actions;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Xml.Items.Actions;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlActionSetsRepository : XmlRepositoryBase, IRepository<IActionSetEntry>
    {

        #region Private Fields

        private readonly XmlDbActionSetTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlActionSetsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetActionSetTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IEnumerable<Type> EntryTypes { get { yield return typeof(XmlActionSetEntry); } }
        public string Name { get { return "Action sets"; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(IActionSetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IActionSetEntry GetById(string id)
        {
            var propSetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (propSetDef == null)
                throw new Exception("No Source definition found with Id: " + id);

            return propSetDef;
        }

        public IActionSetEntry GetNextTo(IActionSetEntry entry)
        {
            var index = _table.Items.IndexOf((XmlActionSetEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IActionSetEntry GetPreviousTo(IActionSetEntry entry)
        {
            var index = _table.Items.IndexOf((XmlActionSetEntry)entry);

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

            var newEntry = Create(entryType) as XmlActionSetEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(IActionSetEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IActionSetEntry entry)
        {
            var index = _table.Items.IndexOf((XmlActionSetEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlActionSetEntry)entry;
        }

        #endregion Public Methods

    }
}
