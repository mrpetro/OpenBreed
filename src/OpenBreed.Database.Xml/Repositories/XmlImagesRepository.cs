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

        private readonly XmlDbImageTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlImagesRepository(XmlDbImageTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlImageEntry); } }
        public override string Name { get { return "Images"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IImageEntry newEntry)
        {
            context.Items.Add((XmlImageEntry)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IImageEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IImageEntry entry)
        {
            return context.Items.IndexOf((XmlImageEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IImageEntry newEntry)
        {
            context.Items[index] = (XmlImageEntry)newEntry;
        }

        #endregion Protected Methods
    }
}