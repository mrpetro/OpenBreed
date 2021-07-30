using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Database.Xml.Items.TileStamps;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyTileStampsRepository : XmlReadonlyRepositoryBase<ITileStampEntry>
    {
        #region Private Fields

        private readonly XmlDbTileStampTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyTileStampsRepository(XmlDbTileStampTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlTileStampEntry);
            }
        }

        public override string Name { get { return "Tile stamps"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ITileStampEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ITileStampEntry entry)
        {
            return context.Items.IndexOf((XmlTileStampEntry)entry);
        }

        #endregion Protected Methods
    }

    public class XmlTileStampsRepository : XmlRepositoryBase<ITileStampEntry>
    {
        #region Private Fields

        private readonly XmlDbTileStampTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlTileStampsRepository(XmlDbTileStampTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlTileStampEntry);
            }
        }

        public override string Name { get { return "Tile stamps"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ITileStampEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ITileStampEntry entry)
        {
            return context.Items.IndexOf((XmlTileStampEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ITileStampEntry newEntry)
        {
            context.Items[index] = (XmlTileStampEntry)newEntry;
        }

        public override void Add(ITileStampEntry newEntry)
        {
            context.Items.Add((XmlTileStampEntry)newEntry);
        }

        #endregion Protected Methods
    }
}