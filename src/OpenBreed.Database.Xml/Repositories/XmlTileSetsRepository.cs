using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Xml.Items.Tiles;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyTileSetsRepository : XmlReadonlyRepositoryBase<ITileSetEntry>
    {
        #region Private Fields

        private readonly XmlDbTileSetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyTileSetsRepository(XmlDbTileSetTableDef context)
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
                yield return typeof(XmlTileSetFromBlkEntry);
                yield return typeof(XmlTileSetFromImageEntry);
            }
        }

        public override string Name { get { return "Tile sets"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ITileSetEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ITileSetEntry entry)
        {
            return context.Items.IndexOf((XmlTileSetEntry)entry);
        }

        #endregion Protected Methods
    }

    public class XmlTileSetsRepository : XmlRepositoryBase<ITileSetEntry>
    {
        #region Private Fields

        private readonly XmlDbTileSetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlTileSetsRepository(XmlDbTileSetTableDef context)
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
                yield return typeof(XmlTileSetFromBlkEntry);
                yield return typeof(XmlTileSetFromImageEntry);
            }
        }

        public override string Name { get { return "Tile sets"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ITileSetEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ITileSetEntry entry)
        {
            return context.Items.IndexOf((XmlTileSetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ITileSetEntry newEntry)
        {
            context.Items[index] = (XmlTileSetEntry)newEntry;
        }

        public override void Add(ITileSetEntry newEntry)
        {
            context.Items.Add((XmlTileSetEntry)newEntry);
        }

        #endregion Protected Methods
    }
}