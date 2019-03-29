using OpenBreed.Common.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Data
{
    public class TileSetsDataProvider
    {

        #region Public Constructors

        public TileSetsDataProvider(DataProvider provider)
        {
            Provider = provider;
        }

        #endregion Public Constructors

        #region Public Properties

        public DataProvider Provider { get; }

        #endregion Public Properties

        #region Public Methods

        private TileSetModel GetModelImpl(ITileSetFromBlkEntry entry)
        {
            return TileSetsDataHelper.FromBlkModel(Provider, entry);
        }

        private TileSetModel GetModelImpl(ITileSetFromImageEntry entry)
        {
            return TileSetsDataHelper.FromImageModel(Provider, entry);
        }

        private TileSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        public TileSetModel GetTileSet(string id)
        {
            var entry = Provider.UnitOfWork.GetRepository<ITileSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("TileSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

    }
}
