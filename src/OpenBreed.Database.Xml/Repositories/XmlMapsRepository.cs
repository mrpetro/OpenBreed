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

        private readonly XmlDbMapTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlMapsRepository(XmlDbMapTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }

        public override string Name { get { return "Maps"; } }

        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlMapEntry); } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IMapEntry newEntry)
        {
            context.Items.Add((XmlMapEntry)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IMapEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IMapEntry entry)
        {
            return context.Items.IndexOf((XmlMapEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IMapEntry newEntry)
        {
            context.Items[index] = (XmlMapEntry)newEntry;
        }

        #endregion Protected Methods
    }
}