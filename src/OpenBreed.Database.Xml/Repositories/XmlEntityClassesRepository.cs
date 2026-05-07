using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.DataSources;
using OpenBreed.Database.Interface.Items.EntityClasses;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.EntityClass;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyEntityClassesRepository : XmlReadonlyRepositoryBase<IDbEntityClass>
    {
        #region Private Fields

        private readonly XmlDbEntityClassTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyEntityClassesRepository(XmlDbEntityClassTableDef context)
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
                yield return typeof(XmlDbEntityClass);
            }
        }

        public override string Name { get { return "Entity classes"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IDbEntityClass GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbEntityClass entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        #endregion Protected Methods
    }

    public class XmlEntityClassesRepository : XmlRepositoryBase<IDbEntityClass>
    {
        #region Private Fields

        private readonly XmlDbEntityClassTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlEntityClassesRepository(XmlDbEntityClassTableDef context)
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
                yield return typeof(XmlDbEntityClass);
            }
        }

        public override string Name { get { return "Entity classes"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbEntityClass newEntry)
        {
            context.Items.Add((XmlDbEntityClass)newEntry);
        }

        public override bool Remove(IDbEntityClass entry)
        {
            return context.Items.Remove((XmlDbEntityClass)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbEntityClass GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbEntityClass entry)
        {
            return context.Items.FindIndex(item => item.Id == entry.Id);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbEntityClass newEntry)
        {
            context.Items[index] = (XmlDbEntityClass)newEntry;
        }

        #endregion Protected Methods
    }
}