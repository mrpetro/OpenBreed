using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Database.Interface.Items.Tiles;
using OpenBreed.Model.Palettes;
using OpenBreed.Model.Tiles;
using System;

namespace OpenBreed.Common.Data
{
    public class TileAtlasDataProvider
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;

        private readonly IModelsProvider modelsProvider;

        #endregion Private Fields

        #region Public Constructors

        public TileAtlasDataProvider(IModelsProvider modelsProvider, IRepositoryProvider repositoryProvider)
        {
            this.modelsProvider = modelsProvider;
            this.repositoryProvider = repositoryProvider;
        }

        #endregion Public Constructors

        #region Public Methods

        public TileSetModel GetTileAtlas(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbTileAtlas>().GetById(id);
            if (entry == null)
                throw new Exception("TileSet error: " + id);

            return GetTileAtlas(entry);
        }

        #endregion Public Methods

        #region Private Methods

        public TileSetModel GetTileAtlas(IDbTileAtlas dbTileAtlas, bool refresh = false)
        {
            switch (dbTileAtlas)
            {
                case IDbTileAtlasFromBlk dbTileAtlasFromBlk:
                    return modelsProvider.GetModel<IDbTileAtlasFromBlk, TileSetModel>(dbTileAtlasFromBlk, refresh);
                case IDbTileAtlasFromAcbm dbTileAtlasFromImage:
                    return modelsProvider.GetModel<IDbTileAtlasFromAcbm, TileSetModel>(dbTileAtlasFromImage, refresh);
                default:
                    throw new NotImplementedException(dbTileAtlas.GetType().ToString());
            }
        }

        #endregion Private Methods
    }
}