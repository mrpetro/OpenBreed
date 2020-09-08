using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Formats;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.Assets;
using OpenBreed.Database.Xml.Tables;
using OpenBreed.Database.Xml.Items.Assets;
using OpenBreed.Common;
using OpenBreed.Database.Interface.Items;
using OpenBreed.Database.Interface.Items.Assets;
using OpenBreed.Database.Interface;

namespace OpenBreed.Database.Xml.Repositories
{
    public class XmlAssetsRepository : XmlRepositoryBase<IAssetEntry>
    {

        #region Private Fields

        private readonly XmlDbAssetTableDef _table;

        #endregion Private Fields

        #region Public Constructors

        public XmlAssetsRepository(XmlDatabaseMan context) : base(context)
        {
            _table = context.GetTable<XmlDbAssetTableDef>();


        }

        #endregion Public Constructors

        #region Public Properties

        public override IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public override string Name { get { return "Assets"; } }

        public override IEnumerable<Type> EntryTypes
        {
            get
            {
                yield return typeof(XmlAssetEntry);
            }
        }

        public override int Count => _table.Items.Count;

        #endregion Public Properties

        #region Public Methods

        protected override IAssetEntry GetEntryWithIndex(int index)
        {
            return _table.Items[index];
        }

        protected override int GetIndexOf(IAssetEntry entry)
        {
            return _table.Items.IndexOf((XmlAssetEntry)entry);
        }

        protected override void ReplaceEntryWithIndex(int index, IAssetEntry newEntry)
        {
            _table.Items[index] = (XmlAssetEntry)newEntry;
        }

        public override void Add(IAssetEntry newEntry)
        {
            _table.Items.Add((XmlAssetEntry)newEntry);
        }
        #endregion Public Methods

    }
}
