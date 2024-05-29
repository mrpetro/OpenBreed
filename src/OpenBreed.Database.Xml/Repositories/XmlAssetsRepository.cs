using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Actions;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Xml.Items.Actions;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Database.Xml.Tables;
using System;
using System.Collections.Generic;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlReadonlyAssetsRepository : XmlReadonlyRepositoryBase<IDbAsset>
    {
        #region Private Fields

        private readonly XmlDbAssetTableDef context;

        #endregion Private Fields

        #region Public Constructors

        public XmlReadonlyAssetsRepository(XmlDbAssetTableDef context)
        {
            this.context = context;
        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override string Name { get { return "Assets"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbAsset);
            }
        }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override IDbAsset GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbAsset entry)
        {
            return context.Items.IndexOf((XmlDbAsset)entry);
        }

        #endregion Protected Methods
    }

    public class XmlAssetsRepository : XmlRepositoryBase<IDbAsset>
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

        public override IEnumerable<IDbEntry> Entries { get { return context.Items; } }
        public override string Name { get { return "Assets"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlDbAsset);
            }
        }

        public override int Count => context.Items.Count;

        #endregion Public Properties

        #region Public Methods

        public override void Add(IDbAsset newEntry)
        {
            context.Items.Add((XmlDbAsset)newEntry);
        }

        public override bool Remove(IDbAsset entry)
        {
            return context.Items.Remove((XmlDbAsset)entry);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override IDbAsset GetEntryWithIndex(int index)
        {
            return context.Items[index];
        }

        protected override int GetIndexOf(IDbAsset entry)
        {
            return context.Items.IndexOf((XmlDbAsset)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IDbAsset newEntry)
        {
            context.Items[index] = (XmlDbAsset)newEntry;
        }

        #endregion Protected Methods
    }
}