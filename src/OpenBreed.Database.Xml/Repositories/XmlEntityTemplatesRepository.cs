using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.EntityTemplates;
using OpenBreed.Database.Xml.Items.EntityTemplates;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlEntityTemplatesRepository : XmlRepositoryBase<IEntityTemplateEntry>
    {
        #region Private Fields

        private readonly XmlDbEntityTemplateTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlEntityTemplatesRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbEntityTemplateTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlEntityTemplateFromFileEntry);
            }
        }

        public override string Name { get { return "Entity templates"; } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IEntityTemplateEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(IEntityTemplateEntry entry)
        {
            return _table.Items.IndexOf((XmlEntityTemplateEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IEntityTemplateEntry newEntry)
        {
            _table.Items[index] = (XmlEntityTemplateEntry)newEntry;
        }

        public override void Add(IEntityTemplateEntry newEntry)
        {
            _table.Items.Add((XmlEntityTemplateEntry)newEntry);
        }

        #endregion Protected Methods
    }
}