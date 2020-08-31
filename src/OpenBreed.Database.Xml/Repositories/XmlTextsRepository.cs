using OpenBreed.Common;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Xml.Items.Texts;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlTextsRepository : XmlRepositoryBase, IRepository<ITextEntry>
    {

        #region Private Fields

        private readonly XmlDbTextTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlTextsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTextTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlTextEmbeddedEntry);
                yield return typeof(XmlTextFromMapEntry);
            }
        }
        public string Name { get { return "Texts"; } }

        #endregion Public Properties

        #region Public Methods

        public void Add(ITextEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public ITextEntry GetById(string id)
        {
            var entryDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (entryDef == null)
                throw new Exception("No Text definition found with Id: " + id);

            return entryDef;
        }

        public ITextEntry GetNextTo(ITextEntry entry)
        {
            var index = _table.Items.IndexOf((XmlTextEntry)entry);

            if (index < 0)
                throw new InvalidOperationException($"Entry {entry.Id} index not found in repository.");

            index++;

            if (index < _table.Items.Count)
                return _table.Items[index];
            else
                return null;
        }

        public ITextEntry GetPreviousTo(ITextEntry entry)
        {
            var index = _table.Items.IndexOf((XmlTextEntry)entry);

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

            var newEntry = Create(entryType) as XmlTextEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(ITextEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(ITextEntry entry)
        {
            var index = _table.Items.IndexOf((XmlTextEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlTextEntry)entry;
        }

        #endregion Public Methods

    }
}
