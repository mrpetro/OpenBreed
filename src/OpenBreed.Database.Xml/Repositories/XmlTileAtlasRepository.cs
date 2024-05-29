using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Texts;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Database.Xml.Items.Texts;
using OpenBreed.Database.Xml.Items.Tiles;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyTileAtlasRepository : XmlReadonlyRepositoryBase<IDbTileAtlas>
    {
        #region Private Fields

        private readonly XmlDbTileAtlasTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyTileAtlasRepository(XmlDbTileAtlasTableDef context)
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
                yield return typeof(XmlDbTileAtlasFromBlk);
                yield return typeof(XmlDbTileAtlasFromImage);
            }
        }

        public override string Name
        { get { return "Tile atlases"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbTileAtlas GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbTileAtlas entry)
        {
            return context.Items.IndexOf((XmlDbTileAtlas)entry);
        }

        #endregion Protected Methods
    }

    public class XmlTileAtlasRepository : XmlRepositoryBase<IDbTileAtlas>
    {
        #region Private Fields

        private readonly XmlDbTileAtlasTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlTileAtlasRepository(XmlDbTileAtlasTableDef context)
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
                yield return typeof(XmlDbTileAtlasFromBlk);
                yield return typeof(XmlDbTileAtlasFromImage);
            }
        }

        public override string Name
        { get { return "Tile atlases"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbTileAtlas newEntry)
        {
            context.Items.Add((XmlDbTileAtlas)newEntry);
        }

        public override bool Remove(IDbTileAtlas entry)
        {
            return context.Items.Remove((XmlDbTileAtlas)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbTileAtlas GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbTileAtlas entry)
        {
            return context.Items.IndexOf((XmlDbTileAtlas)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbTileAtlas newEntry)
        {
            context.Items[index] = (XmlDbTileAtlas)newEntry;
        }

        #endregion Protected Methods
    }
}