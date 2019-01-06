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

        public IEnumerable<IEntry> Entries { get { return _table.Items; } }
        public IUnitOfWork UnitOfWork { get; }

        #endregion Public Properties

        #region Public Methods

        public void Add(IAssetEntry entity)
        {
            throw new NotImplementedException();
        }

        public IAssetEntry GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IAssetEntry GetByName(string name)
        {
            var assetDef = _table.Items.FirstOrDefault(item => item.Name == name);
            if (assetDef == null)
                throw new Exception("No Asset definition found with name: " + name);

            return assetDef;
        }

        public IAssetEntry GetNextTo(IAssetEntry entry)
        {
            throw new NotImplementedException();
        }

        public IAssetEntry GetPrevTo(IAssetEntry entry)
        {
            throw new NotImplementedException();
        }

        public void Remove(IAssetEntry entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IAssetEntry entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}
