using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Images;
using OpenBreed.Database.Xml.Items.Images;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyImagesRepository : XmlReadonlyRepositoryBase<IDbImage>
    {
        #region Private Fields

        private readonly XmlDbImageTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyImagesRepository(XmlDbImageTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbImage); } }
        public override string Name { get { return "Images"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IDbImage GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbImage entry)
        {
            return context.Items.IndexOf((XmlDbImage)entry);
        }

        #endregion Protected Methods
    }

    public class XmlImagesRepository : XmlRepositoryBase<IDbImage>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override IEnumerable<Type> EntryTypes { get { yield return typeof(XmlDbImage); } }
        public override string Name { get { return "Images"; } }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbImage newEntry)
        {
            context.Items.Add((XmlDbImage)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbImage GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbImage entry)
        {
            return context.Items.IndexOf((XmlDbImage)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbImage newEntry)
        {
            context.Items[index] = (XmlDbImage)newEntry;
        }

        #endregion Protected Methods
    }
}