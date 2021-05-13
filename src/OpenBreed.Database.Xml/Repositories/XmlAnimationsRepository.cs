using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Xml.Items.Animations;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyAnimationsRepository : XmlReadonlyRepositoryBase<IAnimationEntry>
    {
        #region Private Fields

        private readonly XmlDbAnimationTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyAnimationsRepository(XmlDbAnimationTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlAnimationEntry); } }
        public override string Name { get { return "Animations"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IAnimationEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IAnimationEntry entry)
        {
            return context.Items.IndexOf((XmlAnimationEntry)entry);
        }

        #endregion Protected Methods
    }

    public class XmlAnimationsRepository : XmlRepositoryBase<IAnimationEntry>
    {
        #region Private Fields

        private readonly XmlDbAnimationTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlAnimationsRepository(XmlDbAnimationTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlAnimationEntry); } }
        public override string Name { get { return "Animations"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IAnimationEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IAnimationEntry entry)
        {
            return context.Items.IndexOf((XmlAnimationEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IAnimationEntry newEntry)
        {
            context.Items[index] = (XmlAnimationEntry)newEntry;
        }

        public override void Add(IAnimationEntry newEntry)
        {
            context.Items.Add((XmlAnimationEntry)newEntry);
        }

        #endregion Protected Methods
    }
}