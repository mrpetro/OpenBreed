using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Xml.Items.DataSources;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyDataSourcesRepository : XmlReadonlyRepositoryBase<IDbDataSource>
    {
        #region Private Fields

        private readonly XmlDbDataSourceTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyDataSourcesRepository(XmlDbDataSourceTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override int Count => context.Items.Count;

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override string Name { get { return "Data sources"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbFileDataSource);
                yield return typeof(XmlDbEpfArchiveFileDataSource);
            }
        }

        protected override IDbDataSource GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbDataSource entry)
        {
            return context.Items.IndexOf((XmlDbDataSource)entry);
        }

        #endregion Public Properties
    }

    public class XmlDataSourcesRepository : XmlRepositoryBase<IDbDataSource>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override string Name { get { return "Data sources"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbFileDataSource);
                yield return typeof(XmlDbEpfArchiveFileDataSource);
            }
        }

        protected override IDbDataSource GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbDataSource entry)
        {
            return context.Items.IndexOf((XmlDbDataSource)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbDataSource newEntry)
        {
            context.Items[index] = (XmlDbDataSource)newEntry;
        }

        public override void Add(IDbDataSource newEntry)
        {
            context.Items.Add((XmlDbDataSource)newEntry);
        }

        #endregion Public Properties
    }
}