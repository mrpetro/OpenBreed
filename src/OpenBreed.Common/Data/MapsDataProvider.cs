using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Palettes;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Palettes;
using System;

namespace OpenBreed.Common.Data
{
    public class MapsDataProvider
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileAtlasDataProvider;
        private readonly ActionSetsDataProvider actionSets;
        private readonly IModelsProvider modelsProvider;
        private readonly IRepositoryProvider repositoryProvider;

        #endregion Private Fields

        #region Public Constructors

        public MapsDataProvider(IModelsProvider modelsProvider, IRepositoryProvider repositoryProvider, TileAtlasDataProvider tileAtlasDataProvider, ActionSetsDataProvider actionSets)
        {
            this.modelsProvider = modelsProvider;
            this.repositoryProvider = repositoryProvider;
            this.tileAtlasDataProvider = tileAtlasDataProvider;
            this.actionSets = actionSets;
        }

        #endregion Public Constructors

        #region Public Methods

        public MapModel GetMap(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbMap>().GetById(id);

            if (entry is null)
            {
                throw new Exception("Map error: " + id);
            }

            return GetMap(entry);
        }

        public MapModel GetMap(IDbMap dbMap, bool refresh = false)
        {
            var map = modelsProvider.GetModel<IDbMap, MapModel>(dbMap, refresh);

            if (dbMap.TileSetRef != null)
            {
                map.TileSet = tileAtlasDataProvider.GetTileAtlas(dbMap.TileSetRef);
            }

            if (dbMap.ActionSetRef != null)
            {
                map.ActionSet = actionSets.GetActionSet(dbMap.ActionSetRef);
            }

            return map;
        }

        #endregion Public Methods
    }
}