using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.EntityTemplates;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyEntityTemplatesRepository : XmlReadonlyRepositoryBase<IEntityTemplateEntry>
    {
        #region Private Fields

        private readonly XmlDbEntityTemplateTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyEntityTemplatesRepository(XmlDbEntityTemplateTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlEntityTemplateFromFileEntry);
            }
        }

        public override string Name { get { return "Entity templates"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IEntityTemplateEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IEntityTemplateEntry entry)
        {
            return context.Items.IndexOf((XmlEntityTemplateEntry)entry);
        }

        #endregion Protected Methods
    }

    public class XmlEntityTemplatesRepository : XmlRepositoryBase<IEntityTemplateEntry>
    {
        #region Private Fields

        private readonly XmlDbEntityTemplateTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlEntityTemplatesRepository(XmlDbEntityTemplateTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlEntityTemplateFromFileEntry);
            }
        }

        public override string Name { get { return "Entity templates"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IEntityTemplateEntry newEntry)
        {
            context.Items.Add((XmlEntityTemplateEntry)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IEntityTemplateEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IEntityTemplateEntry entry)
        {
            return context.Items.IndexOf((XmlEntityTemplateEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IEntityTemplateEntry newEntry)
        {
            context.Items[index] = (XmlEntityTemplateEntry)newEntry;
        }

        #endregion Protected Methods
    }
}