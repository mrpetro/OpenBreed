using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Model.Maps;
using System;

namespace OpenBreed.Common.Data
{
    public class MapsDataProvider
    {
        #region Private Fields

        private readonly TileSetsDataProvider tileSets;
        private readonly PalettesDataProvider palettes;
        private readonly ActionSetsDataProvider actionSets;

        private readonly IDataProvider provider;

        private readonly IWorkspaceMan workspaceMan;

        #endregion Private Fields

        #region Public Constructors

        public MapsDataProvider(IDataProvider provider, IWorkspaceMan workspaceMan, TileSetsDataProvider tileSets, PalettesDataProvider palettes, ActionSetsDataProvider actionSets)
        {
            this.provider = provider;
            this.workspaceMan = workspaceMan;
            this.tileSets = tileSets;
            this.palettes = palettes;
            this.actionSets = actionSets;
        }

        #endregion Public Constructors

        #region Public Methods

        public MapModel GetMap(string id)
        {
            var entry = workspaceMan.UnitOfWork.GetRepository<IMapEntry>().GetById(id);
            if (entry == null)
                throw new Exception("Map error: " + id);

            if (entry.DataRef == null)
                return null;

            var map = provider.GetData<MapModel>(entry.DataRef);

            if (entry.TileSetRef != null)
                map.TileSet = tileSets.GetTileSet(entry.TileSetRef);

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