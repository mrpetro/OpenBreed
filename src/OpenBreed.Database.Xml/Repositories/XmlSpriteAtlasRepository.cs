using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Xml.Items.Sounds;
using OpenBreed.Database.Xml.Items.Sprites;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlySpriteAtlasRepository : XmlReadonlyRepositoryBase<IDbSpriteAtlas>
    {
        #region Private Fields

        private readonly XmlDbSpriteAtlasTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlySpriteAtlasRepository(XmlDbSpriteAtlasTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries
        { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes
        { get { yield return typeof(XmlDbSpriteAtlas); } }
        public override string Name
        { get { return "Sprite sets"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbSpriteAtlas GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbSpriteAtlas entry)
        {
            return context.Items.IndexOf((XmlDbSpriteAtlas)entry);
        }

        #endregion Protected Methods
    }

    public class XmlSpriteAtlasRepository : XmlRepositoryBase<IDbSpriteAtlas>
    {
        #region Private Fields

        private readonly XmlDbSpriteAtlasTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSpriteAtlasRepository(XmlDbSpriteAtlasTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries
        { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes
        { get { yield return typeof(XmlDbSpriteAtlas); } }
        public override string Name
        { get { return "Sprite sets"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbSpriteAtlas newEntry)
        {
            context.Items.Add((XmlDbSpriteAtlas)newEntry);
        }

        public override bool Remove(IDbSpriteAtlas entry)
        {
            return context.Items.Remove((XmlDbSpriteAtlas)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbSpriteAtlas GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbSpriteAtlas entry)
        {
            return context.Items.IndexOf((XmlDbSpriteAtlas)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbSpriteAtlas newEntry)
        {
            context.Items[index] = (XmlDbSpriteAtlas)newEntry;
        }

        #endregion Protected Methods
    }
}