using OpenBreed.Common.Interface.Data;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Model.Maps;
using System;

namespace OpenBreed.Common.Data
{
    public class MapsDataProvider
    {
        #region Private Fields

        private readonly TileAtlasDataProvider tileAtlasDataProvider;
        private readonly PalettesDataProvider palettes;
        private readonly ActionSetsDataProvider actionSets;

        private readonly IModelsProvider provider;

        private readonly IRepositoryProvider repositoryProvider;

        #endregion Private Fields

        #region Public Constructors

        public MapsDataProvider(IModelsProvider provider, IRepositoryProvider repositoryProvider, TileAtlasDataProvider tileAtlasDataProvider, PalettesDataProvider palettes, ActionSetsDataProvider actionSets)
        {
            this.provider = provider;
            this.repositoryProvider = repositoryProvider;
            this.tileAtlasDataProvider = tileAtlasDataProvider;
            this.palettes = palettes;
            this.actionSets = actionSets;
        }

        #endregion Public Constructors

        #region Public Methods

        public MapModel GetMap(string id)
        {
            var entry = repositoryProvider.GetRepository<IDbMap>().GetById(id);
            if (entry == null)
                throw new Exception("Map error: " + id);

            if (entry.DataRef == null)
                return null;

            var map = provider.GetModel<MapModel>(entry.DataRef);

            if (entry.TileSetRef != null)
                map.TileSet = tileAtlasDataProvider.GetTileAtlas(entry.TileSetRef);

            map.Palettes.Clear();
            foreach (var paletteRef in entry.PaletteRefs)
                map.Palettes.Add(palettes.GetPalette(paletteRef));

            if (entry.ActionSetRef != null)
                map.ActionSet = actionSets.GetActionSet(entry.ActionSetRef);

            return map;
        }

        #endregion Public Methods
    }
}