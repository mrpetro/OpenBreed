using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.EntityTemplates;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyEntityTemplatesRepository : XmlReadonlyRepositoryBase<IDbEntityTemplate>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbEntityTemplateFromFile);
            }
        }

        public override string Name { get { return "Entity templates"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IDbEntityTemplate GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbEntityTemplate entry)
        {
            return context.Items.IndexOf((XmlDbEntityTemplate)entry);
        }

        #endregion Protected Methods
    }

    public class XmlEntityTemplatesRepository : XmlRepositoryBase<IDbEntityTemplate>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbEntityTemplateFromFile);
            }
        }

        public override string Name { get { return "Entity templates"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbEntityTemplate newEntry)
        {
            context.Items.Add((XmlDbEntityTemplate)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbEntityTemplate GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbEntityTemplate entry)
        {
            return context.Items.IndexOf((XmlDbEntityTemplate)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbEntityTemplate newEntry)
        {
            context.Items[index] = (XmlDbEntityTemplate)newEntry;
        }

        #endregion Protected Methods
    }
}