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

        private readonly XmlDbDataSourceTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlDataSourcesRepository(XmlDbDataSourceTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override int Count => context.Items.Count;

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }

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
            return context.Items[index];
        }

        protected override int GetIndexOf(IDataSourceEntry entry)
        {
            return context.Items.IndexOf((XmlDataSourceEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDataSourceEntry newEntry)
        {
            context.Items[index] = (XmlDataSourceEntry)newEntry;
        }

        public override void Add(IDataSourceEntry newEntry)
        {
            context.Items.Add((XmlDataSourceEntry)newEntry);
        }

        #endregion Public Properties
    }
}