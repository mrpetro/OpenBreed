using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Formats;
using EPF;
using OpenBreed.Common.Logging;
using System.ComponentModel;
using System.Globalization;
using OpenBreed.Common.Assets;
using OpenBreed.Common.XmlDatabase.Tables.Sources;
using OpenBreed.Common.XmlDatabase.Items.Sources;

namespace OpenBreed.Common.XmlDatabase.Repositories
{
    public class XmlAssetsRepository : IRepository<IAssetEntry>
    {
        #region Private Fields

        private readonly DatabaseAssetTableDef _table;
        private XmlDatabase _context;

        #endregion Private Fields

        #region Public Constructors

        public XmlAssetsRepository(IUnitOfWork unitOfWork, XmlDatabase context)
        {
            UnitOfWork = unitOfWork;
            _context = context;

            _table = _context.GetAssetsTable();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get { return "Assets"; } }

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IAssetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IEntry Find(string id)
        {
            return _table.Items.FirstOrDefault(item => item.Id == id);
        }

        public IAssetEntry GetById(string id)
        {
            var assetDef = _table.Items.FirstOrDefault(item => item.Id == id);
            if (assetDef == null)
                throw new Exception("No Asset definition found with Id: " + id);

            return assetDef;
        }

        public IAssetEntry GetNextTo(IAssetEntry entry)
        {
            var index = _table.Items.IndexOf((AssetDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            index++;

            if (index > _table.Items.Count - 1)
                return null;
            else
                return _table.Items[index];
        }

        public IAssetEntry GetPreviousTo(IAssetEntry entry)
        {
            var index = _table.Items.IndexOf((AssetDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            index--;

            if (index < 0)
                return null;
            else
                return _table.Items[index];
        }

        public void Remove(IAssetEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Update(IAssetEntry entry)
        {
            var index = _table.Items.IndexOf((AssetDef)entry);
            if (index < 0)
                throw new InvalidOperationException($"{entry} not found in repository");

            _table.Items[index] = (AssetDef)entry;
        }

        #endregion Public Methods

    }
}
