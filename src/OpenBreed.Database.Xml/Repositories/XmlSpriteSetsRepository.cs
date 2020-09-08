using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Xml.Items.Sprites;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlSpriteSetsRepository : XmlRepositoryBase<ISpriteSetEntry>
    {
        #region Private Fields

        private readonly XmlDbSpriteSetTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlSpriteSetsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbSpriteSetTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSpriteSetEntry); } }
        public override string Name { get { return "Sprite sets"; } }
        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ISpriteSetEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(ISpriteSetEntry entry)
        {
            return _table.Items.IndexOf((XmlSpriteSetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ISpriteSetEntry newEntry)
        {
            _table.Items[index] = (XmlSpriteSetEntry)newEntry;
        }

        public override void Add(ISpriteSetEntry newEntry)
        {
            _table.Items.Add((XmlSpriteSetEntry)newEntry);
        }

        #endregion Protected Methods
    }
}