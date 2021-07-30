using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Xml.Items.Actions;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyActionSetsRepository : XmlReadonlyRepositoryBase<IDbActionSet>
    {
        #region Private Fields

        private readonly XmlDbActionSetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyActionSetsRepository(XmlDbActionSetTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbActionSet); } }
        public override string Name { get { return "Action sets"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IDbActionSet GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbActionSet entry)
        {
            return context.Items.IndexOf((XmlDbActionSet)entry);
        }

        #endregion Protected Methods
    }

    public class XmlActionSetsRepository : XmlRepositoryBase<IDbActionSet>
    {
        #region Private Fields

        private readonly XmlDbActionSetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlActionSetsRepository(XmlDbActionSetTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbActionSet); } }
        public override string Name { get { return "Action sets"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbActionSet newEntry)
        {
            context.Items.Add((XmlDbActionSet)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbActionSet GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbActionSet entry)
        {
            return context.Items.IndexOf((XmlDbActionSet)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbActionSet newEntry)
        {
            context.Items[index] = (XmlDbActionSet)newEntry;
        }

        #endregion Protected Methods
    }
}