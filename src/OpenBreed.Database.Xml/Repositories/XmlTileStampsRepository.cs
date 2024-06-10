using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Database.Xml.Items.Tiles;
using OpenBreed.Database.Xml.Items.TileStamps;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyTileStampsRepository : XmlReadonlyRepositoryBase<IDbTileStamp>
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

        public override IEnumerable<IDbEntry> Entries
        { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbTileStamp);
            }
        }

        public override string Name
        { get { return "Tile stamps"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbTileStamp GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbTileStamp entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        #endregion Protected Methods
    }

    public class XmlTileStampsRepository : XmlRepositoryBase<IDbTileStamp>
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

        public override IEnumerable<IDbEntry> Entries
        { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbTileStamp);
            }
        }

        public override string Name
        { get { return "Tile stamps"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbTileStamp newEntry)
        {
            context.Items.Add((XmlDbTileStamp)newEntry);
        }

        public override bool Remove(IDbTileStamp entry)
        {
            return context.Items.Remove((XmlDbTileStamp)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbTileStamp GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbTileStamp entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbTileStamp newEntry)
        {
            context.Items[index] = (XmlDbTileStamp)newEntry;
        }

        #endregion Protected Methods
    }
}