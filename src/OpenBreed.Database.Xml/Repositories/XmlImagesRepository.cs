using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Xml.Items.Images;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlImagesRepository : XmlRepositoryBase<IImageEntry>
    {
        #region Private Fields

        private readonly XmlDbImageTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlImagesRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbImageTableDef>();
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlImageEntry); } }
        public override string Name { get { return "Images"; } }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Protected Methods

        protected override IImageEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(IImageEntry entry)
        {
            return _table.Items.IndexOf((XmlImageEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IImageEntry newEntry)
        {
            _table.Items[index] = (XmlImageEntry)newEntry;
        }

        public override void Add(IImageEntry newEntry)
        {
            _table.Items.Add((XmlImageEntry)newEntry);
        }

        #endregion Protected Methods
    }
}