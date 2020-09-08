using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Xml.Items.Sounds;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlSoundsRepository : XmlRepositoryBase<ISoundEntry>
    {
        #region Private Fields

        private readonly XmlDbSoundTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlSoundsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbSoundTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSoundEntry); } }
        public override string Name { get { return "Sounds"; } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ISoundEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(ISoundEntry entry)
        {
            return _table.Items.IndexOf((XmlSoundEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ISoundEntry newEntry)
        {
            _table.Items[index] = (XmlSoundEntry)newEntry;
        }

        public override void Add(ISoundEntry newEntry)
        {
            _table.Items.Add((XmlSoundEntry)newEntry);
        }

        #endregion Protected Methods
    }
}