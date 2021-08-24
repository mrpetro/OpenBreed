using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Model.Maps;
using OpenBreed.Rendering.Interface;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Commands;
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

        void Load(MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world);

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
        private readonly WorldBlockBuilder worldBlockBuilder;
        private readonly ICommandsMan commandsMan;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IEntityFactoryProvider mapEntityFactory;

        private readonly Dictionary<int, IMapWorldEntityLoader> entityLoaders = new Dictionary<int, IMapWorldEntityLoader>();

        #endregion Private Fields

        #region Public Constructors

        public MapWorldDataLoader(IDataLoaderFactory dataLoaderFactory,
                                          IRepositoryProvider repositoryProvider,
                                  MapsDataProvider mapsDataProvider,
                                  ISystemFactory systemFactory,
                                  IWorldMan worldMan,
                                  WorldBlockBuilder worldBlockBuilder,
                                  ICommandsMan commandsMan,
                                  PalettesDataProvider palettesDataProvider,
                                  IEntityFactoryProvider mapEntityFactory)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.mapsDataProvider = mapsDataProvider;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.worldBlockBuilder = worldBlockBuilder;
            this.commandsMan = commandsMan;
            this.palettesDataProvider = palettesDataProvider;
            this.mapEntityFactory = mapEntityFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public static int GetActionCellValue(MapLayoutModel layout, int ix, int iy)
        {
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            var values = layout.GetCellValues(ix, layout.Height - iy - 1);

            return values[actionLayer];
        }

        public object LoadObject(string entryId) => Load(entryId);

        public void Register(int entityType, IMapWorldEntityLoader entityLoader)
        {
            entityLoaders.Add(entityType, entityLoader);
        }

        public World Load(string entryId, params object[] args)
        {
            var entry = repositoryProvider.GetRepository<IDbMap>().GetById(entryId);
            if (entry == null)
                throw new Exception("Map error: " + entryId);

            LoadReferencedTileSet(entry);
            LoadReferencedSpriteSets(entry);

            var map = mapsDataProvider.GetMap(entry.Id);

            if (map is null)
                throw new Exception($"Map model  asset '{entry.DataRef}' could not be loaded.");

            var layout = map.Layout;
            var visited = new bool[layout.Width, layout.Height];

            var cellSize = layout.CellSize;

            var worldBuilder = worldMan.Create();
            worldBuilder.SetupGameWorldSystems(systemFactory);

            worldBuilder.SetName(entryId);
            worldBuilder.SetSize(layout.Width, layout.Width);

            var newWorld = worldBuilder.Build();

            worldBlockBuilder.SetTileAtlas(entry.TileSetRef);

            var gfxLayer = layout.GetLayerIndex(MapLayerType.Gfx);
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            for (int iy = 0; iy < layout.Height; iy++)
            {
                for (int ix = 0; ix < layout.Width; ix++)
                {
                    var cellValues = layout.GetCellValues(ix, layout.Height - iy - 1);
                    var gfxValue = cellValues[gfxLayer];
                    var actionValue = cellValues[actionLayer];

                    PutCell(layout, visited, ix, iy, gfxValue, actionValue, newWorld);
                }
            }

            return newWorld;
        }

        #endregion Public Methods

        #region Private Methods

        private void PutCell(MapLayoutModel layout, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            if (visited[ix, iy])
                return;

            if (entityLoaders.TryGetValue(actionValue, out IMapWorldEntityLoader entityLoader))
            {
                entityLoader.Load(layout, visited, ix, iy, gfxValue, actionValue, world);

                if (visited[ix, iy])
                    return;
            }

            switch (actionValue)
            {
                case 63:
                    PutGenericCell(layout, visited, world, ix, iy, gfxValue, actionValue, hasBody: true, unknown: false);
                    break;
                case 0:
                    PutGenericCell(layout, visited, world, ix, iy, gfxValue, actionValue, hasBody: false, unknown: false);
                    break;
                default:
                    PutGenericCell(layout, visited, world, ix, iy, gfxValue, actionValue, hasBody: false, unknown: true);
                    break;
            }
        }

        private void PutGenericCell(MapLayoutModel layout, bool[,] visited, World world, int ix, int iy, int gfxValue, int actionValue, bool hasBody, bool unknown)
        {
            worldBlockBuilder.SetPosition(ix * layout.CellSize, iy * layout.CellSize);
            worldBlockBuilder.SetTileId(gfxValue);
            worldBlockBuilder.HasBody = hasBody;

            var cellEntity = worldBlockBuilder.Build();

            if (unknown)
                cellEntity.Tag = actionValue;

            commandsMan.Post(new AddEntityCommand(world.Id, cellEntity.Id));

            visited[ix, iy] = true;
        }

        private void LoadReferencedTileSet(IDbMap entry)
        {
            var palette = palettesDataProvider.GetPalette(entry.PaletteRefs.First());
            var tileAtlasLoader = dataLoaderFactory.GetLoader<ITileAtlas>();
            tileAtlasLoader.Load(entry.TileSetRef, palette);
        }

        private void LoadReferencedSpriteSets(IDbMap entry)
        {
            var palette = palettesDataProvider.GetPalette(entry.PaletteRefs.First());
            var spriteAtlasLoader = dataLoaderFactory.GetLoader<ISpriteAtlas>();

            foreach (var spriteSetRef in entry.SpriteSetRefs)
                spriteAtlasLoader.Load(spriteSetRef, palette);
        }

        #endregion Private Methods
    }
}