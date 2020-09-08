using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Xml.Items.Maps;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlMapsRepository : XmlRepositoryBase<IMapEntry>
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

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public override string Name { get { return "Maps"; } }

        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlMapEntry); } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IMapEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(IMapEntry entry)
        {
            return _table.Items.IndexOf((XmlMapEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IMapEntry newEntry)
        {
            _table.Items[index] = (XmlMapEntry)newEntry;
        }

        public override void Add(IMapEntry newEntry)
        {
            _table.Items.Add((XmlMapEntry)newEntry);
        }

        #endregion Protected Methods
    }
}