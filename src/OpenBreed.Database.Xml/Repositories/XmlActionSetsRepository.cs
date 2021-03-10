using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Xml.Items.Actions;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlActionSetsRepository : XmlRepositoryBase<IActionSetEntry>
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

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlActionSetEntry); } }
        public override string Name { get { return "Action sets"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IActionSetEntry newEntry)
        {
            context.Items.Add((XmlActionSetEntry)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IActionSetEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IActionSetEntry entry)
        {
            return context.Items.IndexOf((XmlActionSetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IActionSetEntry newEntry)
        {
            context.Items[index] = (XmlActionSetEntry)newEntry;
        }

        #endregion Protected Methods
    }
}