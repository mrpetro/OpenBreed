using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Xml.Items.Scripts;
using OpenBreed.Database.Xml.Items.Texts;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlScriptsRepository : XmlRepositoryBase, IRepository<IScriptEntry>
    {

        #region Private Fields

        private readonly XmlDbScriptTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlScriptsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetScriptsTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlScriptEmbeddedEntry);
            }
        }
        public string Name { get { return "Scripts"; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(IScriptEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IScriptEntry GetById(string id)
        {
            var entryDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (entryDef == null)
                throw new Exception("No Text definition found with Id: " + id);

            return entryDef;
        }

        public IScriptEntry GetNextTo(IScriptEntry entry)
        {
            var index = _table.Items.IndexOf((XmlScriptEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public IScriptEntry GetPreviousTo(IScriptEntry entry)
        {
            var index = _table.Items.IndexOf((XmlScriptEntry)entry);

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

            var newEntry = Create(entryType) as XmlScriptEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(IScriptEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IScriptEntry entry)
        {
            var index = _table.Items.IndexOf((XmlScriptEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlScriptEntry)entry;
        }

        #endregion Public Methods

    }
}
