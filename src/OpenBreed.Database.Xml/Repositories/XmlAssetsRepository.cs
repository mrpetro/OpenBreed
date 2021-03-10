using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlAssetsRepository : XmlRepositoryBase<IAssetEntry>
    {
        #region Private Fields

        private readonly XmlDbAssetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlAssetsRepository(XmlDbAssetTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return context.Items; } }
        public override string Name { get { return "Assets"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlAssetEntry);
            }
        }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IAssetEntry newEntry)
        {
            context.Items.Add((XmlAssetEntry)newEntry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IAssetEntry GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IAssetEntry entry)
        {
            return context.Items.IndexOf((XmlAssetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IAssetEntry newEntry)
        {
            context.Items[index] = (XmlAssetEntry)newEntry;
        }

        #endregion Protected Methods
    }
}