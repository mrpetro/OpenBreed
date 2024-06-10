using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Xml.Items.Animations;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyAnimationsRepository : XmlReadonlyRepositoryBase<IDbAnimation>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbAnimation); } }
        public override string Name { get { return "Animations"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbAnimation GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbAnimation entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        #endregion Protected Methods
    }

    public class XmlAnimationsRepository : XmlRepositoryBase<IDbAnimation>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbAnimation); } }
        public override string Name { get { return "Animations"; } }
        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IDbAnimation GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbAnimation entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbAnimation newEntry)
        {
            context.Items[index] = (XmlDbAnimation)newEntry;
        }

        public override void Add(IDbAnimation newEntry)
        {
            context.Items.Add((XmlDbAnimation)newEntry);
        }

        public override bool Remove(IDbAnimation entry)
        {
            return context.Items.Remove((XmlDbAnimation)entry);
        }

        #endregion Protected Methods
    }
}