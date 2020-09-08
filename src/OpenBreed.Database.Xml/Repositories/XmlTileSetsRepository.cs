using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Xml.Items.Tiles;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlTileSetsRepository : XmlRepositoryBase<ITileSetEntry>
    {
        #region Private Fields

        private readonly XmlDbTileSetTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlTileSetsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbTileSetTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlTileSetFromBlkEntry);
                yield return typeof(XmlTileSetFromImageEntry);
            }
        }

        public override string Name { get { return "Tile sets"; } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ITileSetEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(ITileSetEntry entry)
        {
            return _table.Items.IndexOf((XmlTileSetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ITileSetEntry newEntry)
        {
            _table.Items[index] = (XmlTileSetEntry)newEntry;
        }

        public override void Add(ITileSetEntry newEntry)
        {
            _table.Items.Add((XmlTileSetEntry)newEntry);
        }

        #endregion Protected Methods
    }
}