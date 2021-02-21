using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Tiles;
using System;

namespace OpenBreed.Common.Data
{
    public class TileSetsDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TileSetsDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public TileSetModel GetTileSet(string id)
        {
            var entry = repositoryProvider.GetRepository<ITileSetEntry>().GetById(id);
            if (entry == null)
                throw new Exception("TileSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private TileSetModel GetModelImpl(ITileSetFromBlkEntry entry)
        {
            return TileSetsDataHelper.FromBlkModel(dataProvider, entry);
        }

        private TileSetModel GetModelImpl(ITileSetFromImageEntry entry)
        {
            return TileSetsDataHelper.FromImageModel(dataProvider, entry);
        }

        private TileSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}