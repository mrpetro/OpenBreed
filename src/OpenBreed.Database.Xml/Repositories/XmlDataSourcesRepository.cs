using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.DataSources;
using OpenBreed.Database.Xml.Tables;
using OpenBreed.Database.Xml.Items.DataSources;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlDataSourcesRepository : XmlRepositoryBase, IRepository<IDataSourceEntry>
    {

        #region Private Fields

        private readonly XmlDbDataSourceTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlDataSourcesRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbDataSourceTableDef>();


        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public string Name { get { return "Data sources"; } }

        public IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlFileDataSourceEntry);
                yield return typeof(XmlEPFArchiveFileDataSourceEntry);
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Add(IDataSourceEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IDataSourceEntry GetById(string id)
        {
            var assetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (assetDef == null)
                throw new Exception("No Data source definition found with Id: " + id);

            return assetDef;
        }

        public IDataSourceEntry GetNextTo(IDataSourceEntry entry)
        {
            var index = _table.Items.IndexOf((XmlDataSourceEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            index++;

            if (index > _table.Items.Count - 1)
                return null;
            else
                return _table.Items[index];
        }

        public IDataSourceEntry GetPreviousTo(IDataSourceEntry entry)
        {
            var index = _table.Items.IndexOf((XmlDataSourceEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            index--;

            if (index < 0)
                return null;
            else
                return _table.Items[index];
        }

        public IEntry New(string newId, Type entryType = null)
        {
            if (Find(newId) != null)
                throw new Exception($"Entry with Id '{newId}' already exist.");

            if (entryType == null)
                entryType = EntryTypes.FirstOrDefault();

            var newEntry = Create(entryType) as XmlDataSourceEntry;

            newEntry.Id = newId;
            _table.Items.Add(newEntry);
            return newEntry;
        }
        public void Remove(IDataSourceEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IDataSourceEntry entry)
        {
            var index = _table.Items.IndexOf((XmlDataSourceEntry)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (XmlDataSourceEntry)entry;
        }

        #endregion Public Methods

    }
}
