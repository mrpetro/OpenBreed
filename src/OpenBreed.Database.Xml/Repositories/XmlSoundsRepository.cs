using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Xml.Items.Sounds;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlySoundsRepository : XmlReadonlyRepositoryBase<ISoundEntry>
    {
        #region Private Fields

        private readonly XmlDbSoundTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlySoundsRepository(XmlDbSoundTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSoundEntry); } }
        public override string Name { get { return "Sounds"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ISoundEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ISoundEntry entry)
        {
            return context.Items.IndexOf((XmlSoundEntry)entry);
        }

        #endregion Protected Methods
    }

    public class XmlSoundsRepository : XmlRepositoryBase<ISoundEntry>
    {
        #region Private Fields

        private readonly XmlDbSoundTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlSoundsRepository(XmlDbSoundTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlSoundEntry); } }
        public override string Name { get { return "Sounds"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override ISoundEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(ISoundEntry entry)
        {
            return context.Items.IndexOf((XmlSoundEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, ISoundEntry newEntry)
        {
            context.Items[index] = (XmlSoundEntry)newEntry;
        }

        public override void Add(ISoundEntry newEntry)
        {
            context.Items.Add((XmlSoundEntry)newEntry);
        }

        #endregion Protected Methods
    }
}