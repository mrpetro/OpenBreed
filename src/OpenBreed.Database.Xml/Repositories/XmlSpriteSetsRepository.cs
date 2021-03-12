using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Xml.Items.Sprites;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlySpriteSetsRepository : XmlReadonlyRepositoryBase<ISpriteSetEntry>
    {
        #region Private Fields

        private readonly XmlDbSpriteSetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlySpriteSetsRepository(XmlDbSpriteSetTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSpriteSetEntry); } }
        public override string Name { get { return "Sprite sets"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ISpriteSetEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ISpriteSetEntry entry)
        {
            return context.Items.IndexOf((XmlSpriteSetEntry)entry);
        }

        #endregion Protected Methods
    }

    public class XmlSpriteSetsRepository : XmlRepositoryBase<ISpriteSetEntry>
    {
        #region Private Fields

        private readonly XmlDbSpriteSetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSpriteSetsRepository(XmlDbSpriteSetTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSpriteSetEntry); } }
        public override string Name { get { return "Sprite sets"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ISpriteSetEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ISpriteSetEntry entry)
        {
            return context.Items.IndexOf((XmlSpriteSetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ISpriteSetEntry newEntry)
        {
            context.Items[index] = (XmlSpriteSetEntry)newEntry;
        }

        public override void Add(ISpriteSetEntry newEntry)
        {
            context.Items.Add((XmlSpriteSetEntry)newEntry);
        }

        #endregion Protected Methods
    }
}