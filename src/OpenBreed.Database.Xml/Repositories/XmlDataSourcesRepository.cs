using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Xml.Items.DataSources;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlDataSourcesRepository : XmlRepositoryBase<IDataSourceEntry>
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

        public override int Count => _table.Items.Count;

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public override string Name { get { return "Data sources"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlFileDataSourceEntry);
                yield return typeof(XmlEPFArchiveFileDataSourceEntry);
            }
        }

        protected override IDataSourceEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(IDataSourceEntry entry)
        {
            return _table.Items.IndexOf((XmlDataSourceEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDataSourceEntry newEntry)
        {
            _table.Items[index] = (XmlDataSourceEntry)newEntry;
        }

        public override void Add(IDataSourceEntry newEntry)
        {
            _table.Items.Add((XmlDataSourceEntry)newEntry);
        }

        #endregion Public Properties
    }
}