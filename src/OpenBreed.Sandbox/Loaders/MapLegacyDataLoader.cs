using OpenBreed.Animation.Interface.Data;
using OpenBreed.Audio.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using OpenBreed.Model.Maps.Blocks;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
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

        void Load(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world);

        #endregion Public Methods
    }

    public interface IMapDataLoader : IDataLoader<World>
    {
        void Register(string templateName, IMapWorldEntityLoader entityLoader);
    }

    internal class MapLegacyDataLoader : IMapDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly MapsDataProvider mapsDataProvider;
        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        //private readonly WorldBlockBuilder worldBlockBuilder;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly IBroadphaseFactory broadphaseGridFactory;
        private readonly ITileGridFactory tileGridFactory;
        private readonly IEntityMan entityMan;
        private readonly ITileMan tileMan;
        private readonly ILogger logger;
        private readonly Dictionary<string, IMapWorldEntityLoader> entityLoaders = new Dictionary<string, IMapWorldEntityLoader>();

        #endregion Private Fields

        #region Public Constructors

        public MapLegacyDataLoader(IDataLoaderFactory dataLoaderFactory,
                                   IRenderableFactory renderableFactory,
                                   IEntityMan entityMan,
                                   IRepositoryProvider repositoryProvider,
                                   MapsDataProvider mapsDataProvider,
                                   ISystemFactory systemFactory,
                                   IWorldMan worldMan,
                                   PalettesDataProvider palettesDataProvider,
                                   IBroadphaseFactory broadphaseGridFactory,
                                   ITileGridFactory tileGridFactory,
                                   ITileMan tileMan,
                                   ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.renderableFactory = renderableFactory;
            this.entityMan = entityMan;
            this.mapsDataProvider = mapsDataProvider;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            //this.worldBlockBuilder = worldBlockBuilder;
            this.palettesDataProvider = palettesDataProvider;
            this.broadphaseGridFactory = broadphaseGridFactory;
            this.tileGridFactory = tileGridFactory;

            this.tileMan = tileMan;
            this.logger = logger;
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

        public void Register(string templateName, IMapWorldEntityLoader entityLoader)
        {
            entityLoaders.Add(templateName, entityLoader);
        }

        public World Load(string entryId, params object[] args)
        {
            var world = worldMan.GetByName(entryId);

            if (world != null)
                return world;

            var dbMap = repositoryProvider.GetRepository<IDbMap>().GetById(entryId);
            if (dbMap == null)
                throw new Exception($"Missing Map: {entryId}");

            var map = mapsDataProvider.GetMap(dbMap.Id);

            if (map is null)
                throw new Exception($"Map model asset '{dbMap.DataRef}' could not be loaded.");

            LoadPalettes(map);
            LoadReferencedTileSet(dbMap);
            LoadReferencedSpriteSets(dbMap);
            LoadReferencedAnimations(dbMap);
            LoadReferencedTileStamps(dbMap);
            LoadReferencedSounds(dbMap);

            var layout = map.Layout;
            var visited = new bool[layout.Width, layout.Height];

            var cellSize = layout.CellSize;

            var worldBuilder = worldMan.Create();
            worldBuilder.SetName(entryId);
            worldBuilder.SetSize(layout.Width, layout.Width);
            worldBuilder.AddModule(broadphaseGridFactory.CreateStatic(layout.Width, layout.Height, cellSize));
            worldBuilder.AddModule(broadphaseGridFactory.CreateDynamic());
            worldBuilder.AddModule(tileGridFactory.CreateGrid(layout.Width, layout.Height, 1, cellSize));
            worldBuilder.AddModule(renderableFactory.CreateRenderableBatch());

            worldBuilder.SetupGameWorldSystems(systemFactory);

            world = worldBuilder.Build();

            var mapper = new MapMapper(dbMap.TileSetRef);

            var gfxLayer = layout.GetLayerIndex(MapLayerType.Gfx);
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            for (int iy = 0; iy < layout.Height; iy++)
            {
                for (int ix = 0; ix < layout.Width; ix++)
                {
                    var cellValues = layout.GetCellValues(ix, iy);
                    var gfxValue = cellValues[gfxLayer];
                    var actionValue = cellValues[actionLayer];

                    var action = map.GetAction(actionValue);

                    if(action != null)
                        LoadCellEntity(mapper, map, visited, ix, iy, world, action, gfxValue);

                    //if (mapper.Map(actionValue, gfxValue, out string templaneName, out string flavor))
                    //    LoadCellEntity(mapper, map, visited, ix, iy, world, templaneName, flavor, gfxValue);
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

                    PutUnknownCodeCell(mapper, map, visited, ix, iy, gfxValue, actionValue, world);
                    //LoadUnknownCell(worldBlockBuilder, layout, newWorld, ix, iy, gfxValue, actionValue, hasBody: false, unknown: false);
                }
            }

            return world;
        }

        private void LoadPalettes(MapModel mapModel)
        {
            foreach (var paletteModel in mapModel.Palettes)
            {
                var paletteEntityTag = $"GameWorld/Palette/{paletteModel.Name}";

                var paletteEntity = entityMan.GetByTag(paletteEntityTag).FirstOrDefault();

                if (paletteEntity is null)
                {
                    paletteEntity = entityMan.Create();
                    paletteEntity.Tag = paletteEntityTag;
                }

                var paletteComponent = paletteEntity.TryGet<PaletteComponent>();

                if (paletteComponent is null)
                {
                    paletteComponent = new PaletteComponent();
                    paletteEntity.Add(paletteComponent);
                }

                for (int i = 0; i < paletteModel.Length; i++)
                {
                    var c = paletteModel[i];
                    paletteComponent.Colors[i] = new PaletteColor(c.R, c.G, c.B);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, World world, ActionModel action, int gfxValue)
        {
            if (visited[ix, iy])
                return;

            if (entityLoaders.TryGetValue(action.Name, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(mapAssets, map, visited, ix, iy, action.Name, "", gfxValue, world);
            else
                logger.Warning($"Missing loader for action '{action}'");
        }

        private void LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, World world, string templateName, string flavor, int gfxValue)
        {
            if (visited[ix, iy])
                return;

            if (entityLoaders.TryGetValue(templateName, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(mapAssets, map, visited, ix, iy, templateName, flavor, gfxValue, world);
        }

        private void PutUnknownCodeCell(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            if (entityLoaders.TryGetValue("Unknown", out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(worldBlockBuilder, map, visited, ix, iy, "Unknown", actionValue.ToString(), gfxValue, world);
        }

        private void LoadReferencedTileSet(IDbMap dbMap)
        {
            var tileAtlasLoader = dataLoaderFactory.GetLoader<ITileAtlasDataLoader>();

            var palette = palettesDataProvider.GetPalette(dbMap.PaletteRefs.First());

            tileAtlasLoader.Load(dbMap.TileSetRef, palette);
        }

        private void LoadReferencedSpriteSets(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            var palette = palettesDataProvider.GetPalette(dbMap.PaletteRefs.First());

            //Load common sprites
            var dbSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbAnim in dbSpriteAtlas)
                loader.Load(dbAnim.Id, palette);

            //Load level specific sprites
            dbSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbAnim in dbSpriteAtlas)
                loader.Load(dbAnim.Id, palette);


            //var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //foreach (var spriteSetRef in dbMap.SpriteSetRefs)
            //    loader.Load(spriteSetRef, palette);
        }

        private void LoadReferencedAnimations(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader<Entity>>();

            //Load common animations
            var dbAnims = repositoryProvider.GetRepository<IDbAnimation>().Entries.Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbAnim in dbAnims)
                loader.Load(dbAnim.Id);

            //Load level specific animations
            dbAnims = repositoryProvider.GetRepository<IDbAnimation>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbAnim in dbAnims)
                loader.Load(dbAnim.Id);
        }

        private void LoadReferencedTileStamps(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ITileStampDataLoader>();

            var dbTileStamps = repositoryProvider.GetRepository<IDbTileStamp>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));

            foreach (var dbTileStamp in dbTileStamps)
                loader.Load(dbTileStamp.Id);
        }

        private void LoadReferencedSounds(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ISoundSampleDataLoader>();

            //Load common sounds
            var dbSounds = repositoryProvider.GetRepository<IDbSound>().Entries.Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbSound in dbSounds)
                loader.Load(dbSound.Id);

            //Load level specific sounds
            dbSounds = repositoryProvider.GetRepository<IDbSound>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbSound in dbSounds)
                loader.Load(dbSound.Id);
        }

        #endregion Private Methods
    }
}