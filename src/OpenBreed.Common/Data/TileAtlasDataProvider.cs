using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Tiles;
using System;

namespace OpenBreed.Common.Data
{
    public class TileAtlasDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider dataProvider;

        #endregion Private Fields

        #region Public Constructors

        public TileAtlasDataProvider(IModelsProvider dataProvider, IRepositoryProvider repositoryProvider)
        {
            this.dataProvider = dataProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public TileSetModel GetTileAtlas(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbTileAtlas>().GetById(id);
            if (entry == null)
                throw new Exception("TileSet error: " + id);

            return GetModel(entry);
        }

        #endregion Public Methods

        #region Private Methods

        private TileSetModel GetModelImpl(IDbTileAtlasFromBlk entry)
        {
            return TileAtlasDataHelper.FromBlkModel(dataProvider, entry);
        }

        private TileSetModel GetModelImpl(IDbTileAtlasFromImage entry)
        {
            return TileAtlasDataHelper.FromImageModel(dataProvider, entry);
        }

        private TileSetModel GetModel(dynamic entry)
        {
            return GetModelImpl(entry);
        }

        #endregion Private Methods
    }
}