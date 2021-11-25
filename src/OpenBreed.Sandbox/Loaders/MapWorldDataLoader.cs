using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    public interface IMapWorldEntityLoader
    {
        #region Public Methods

        void Load(MapAssets worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world);

        #endregion Public Methods
    }

    internal class MapWorldDataLoader : IDataLoader<World>
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly MapsDataProvider mapsDataProvider;
        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        //private readonly WorldBlockBuilder worldBlockBuilder;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IEntityFactoryProvider mapEntityFactory;
        private readonly IBroadphaseFactory broadphaseGridFactory;
        private readonly ITileGridFactory tileGridFactory;
        private readonly ITileMan tileMan;
        private readonly Dictionary<int, IMapWorldEntityLoader> entityLoaders = new Dictionary<int, IMapWorldEntityLoader>();

        #endregion Private Fields

        #region Public Constructors

        public MapWorldDataLoader(IDataLoaderFactory dataLoaderFactory,
                                  IRepositoryProvider repositoryProvider,
                                  MapsDataProvider mapsDataProvider,
                                  ISystemFactory systemFactory,
                                  IWorldMan worldMan,
                                  PalettesDataProvider palettesDataProvider,
                                  IEntityFactoryProvider mapEntityFactory,
                                  IBroadphaseFactory broadphaseGridFactory,
                                  ITileGridFactory tileGridFactory,
                                  ITileMan tileMan)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.mapsDataProvider = mapsDataProvider;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            //this.worldBlockBuilder = worldBlockBuilder;
            this.palettesDataProvider = palettesDataProvider;
            this.mapEntityFactory = mapEntityFactory;
            this.broadphaseGridFactory = broadphaseGridFactory;
            this.tileGridFactory = tileGridFactory;
            this.tileMan = tileMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public static int GetActionCellValue(MapLayoutModel layout, int ix, int iy)
        {
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            var values = layout.GetCellValues(ix, iy);

            return values[actionLayer];
        }

        public object LoadObject(string entryId) => Load(entryId);

        public void Register(int entityType, IMapWorldEntityLoader entityLoader)
        {
            entityLoaders.Add(entityType, entityLoader);
        }

        public World Load(string entryId, params object[] args)
        {
            var world = worldMan.GetByName(entryId);

            if (world != null)
                return world;

            var dbMap = repositoryProvider.GetRepository<IDbMap>().GetById(entryId);
            if (dbMap == null)
                throw new Exception("Map error: " + entryId);

            LoadReferencedTileSet(dbMap);
            LoadReferencedSpriteSets(dbMap);

            var map = mapsDataProvider.GetMap(dbMap.Id);

            if (map is null)
                throw new Exception($"Map model  asset '{dbMap.DataRef}' could not be loaded.");

            var layout = map.Layout;
            var visited = new bool[layout.Width, layout.Height];

            var cellSize = layout.CellSize;

            var worldBuilder = worldMan.Create();
            worldBuilder.SetName(entryId);
            worldBuilder.SetSize(layout.Width, layout.Width);

            worldBuilder.AddModule(broadphaseGridFactory.CreateStatic(layout.Width, layout.Height, cellSize));
            worldBuilder.AddModule(broadphaseGridFactory.CreateDynamic());
            worldBuilder.AddModule(tileGridFactory.CreateGrid(layout.Width, layout.Height, 1, cellSize));

            worldBuilder.SetupGameWorldSystems(systemFactory);
            world = worldBuilder.Build();

            var mapAssets = new MapAssets(tileMan);

            mapAssets.SetTileAtlas(dbMap.TileSetRef);

            var gfxLayer = layout.GetLayerIndex(MapLayerType.Gfx);
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            for (int iy = 0; iy < layout.Height; iy++)
            {
                for (int ix = 0; ix < layout.Width; ix++)
                {
                    var cellValues = layout.GetCellValues(ix, iy);
                    var gfxValue = cellValues[gfxLayer];
                    var actionValue = cellValues[actionLayer];

                    LoadCellEntity(mapAssets, map, visited, ix, iy, gfxValue, actionValue, world);
                }
            }

            //Process trough all not visited
            for (int iy = 0; iy < layout.Height; iy++)
            {
                for (int ix = 0; ix < layout.Width; ix++)
                {
                    if (visited[ix, iy])
                        continue;

                    var cellValues = layout.GetCellValues(ix, iy);
                    var gfxValue = cellValues[gfxLayer];
                    var actionValue = cellValues[actionLayer];

                    PutUnknownCodeCell(mapAssets, map, visited, ix, iy, gfxValue, actionValue, world);
                    //LoadUnknownCell(worldBlockBuilder, layout, newWorld, ix, iy, gfxValue, actionValue, hasBody: false, unknown: false);
                }
            }

            return world;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadCellEntity(MapAssets mapAssets, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            if (visited[ix, iy])
                return;

            if (entityLoaders.TryGetValue(actionValue, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(mapAssets, map, visited, ix, iy, gfxValue, actionValue, world);
        }

        private void PutUnknownCodeCell(MapAssets worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            if (entityLoaders.TryGetValue(UnknownCellEntityLoader.UNKNOWN_CODE, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(worldBlockBuilder, map, visited, ix, iy, gfxValue, actionValue, world);
        }

        private void LoadReferencedTileSet(IDbMap entry)
        {
            var palette = palettesDataProvider.GetPalette(entry.PaletteRefs.First());
            var tileAtlasLoader = dataLoaderFactory.GetLoader<ITileAtlasDataLoader>();
            tileAtlasLoader.Load(entry.TileSetRef, palette);
        }

        private void LoadReferencedSpriteSets(IDbMap entry)
        {
            var palette = palettesDataProvider.GetPalette(entry.PaletteRefs.First());
            var spriteAtlasLoader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            foreach (var spriteSetRef in entry.SpriteSetRefs)
                spriteAtlasLoader.Load(spriteSetRef, palette);
        }

        #endregion Private Methods
    }
}