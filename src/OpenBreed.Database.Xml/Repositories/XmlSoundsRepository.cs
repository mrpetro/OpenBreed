using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Xml.Items.Sounds;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlySoundsRepository : XmlReadonlyRepositoryBase<IDbSound>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbSound); } }
        public override string Name { get { return "Sounds"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbSound GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbSound entry)
        {
            return context.Items.IndexOf((XmlDbSound)entry);
        }

        #endregion Protected Methods
    }

    public class XmlSoundsRepository : XmlRepositoryBase<IDbSound>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbSound); } }
        public override string Name { get { return "Sounds"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbSound GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbSound entry)
        {
            return context.Items.IndexOf((XmlDbSound)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbSound newEntry)
        {
            context.Items[index] = (XmlDbSound)newEntry;
        }

        public override void Add(IDbSound newEntry)
        {
            context.Items.Add((XmlDbSound)newEntry);
        }

        #endregion Protected Methods
    }
}