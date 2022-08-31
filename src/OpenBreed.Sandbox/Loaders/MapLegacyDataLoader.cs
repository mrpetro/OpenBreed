using OpenBreed.Animation.Interface.Data;
using OpenBreed.Audio.Interface.Data;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Scripts;
using OpenBreed.Database.Interface.Items.Sounds;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    public interface IMapDataLoader : IDataLoader<World>
    {
        #region Public Methods

        void Register(string templateName, IMapWorldEntityLoader entityLoader);

        #endregion Public Methods
    }

    public interface IMapWorldEntityLoader
    {
        #region Public Methods

        Entity Load(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, World world);

        #endregion Public Methods
    }

    internal class MapLegacyDataLoader : IMapDataLoader
    {
        #region Private Fields

        private readonly IBroadphaseFactory broadphaseGridFactory;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly Dictionary<string, IMapWorldEntityLoader> entityLoaders = new Dictionary<string, IMapWorldEntityLoader>();
        private readonly IEntityMan entityMan;
        private readonly ILogger logger;
        private readonly MapsDataProvider mapsDataProvider;

        //private readonly WorldBlockBuilder worldBlockBuilder;
        private readonly PalettesDataProvider palettesDataProvider;

        private readonly IRenderableFactory renderableFactory;
        private readonly IRepositoryProvider repositoryProvider;
        private readonly IScriptMan scriptMan;
        private readonly ISystemFactory systemFactory;
        private readonly ITileGridFactory tileGridFactory;
        private readonly IDataGridFactory dataGridFactory;
        private readonly ITileMan tileMan;
        private readonly ITriggerMan triggerMan;
        private readonly IWorldMan worldMan;

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
                                   IDataGridFactory dataGridFactory,
                                   ITileMan tileMan,
                                   ILogger logger,
                                   ITriggerMan triggerMan,
                                   IScriptMan scriptMan)
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
            this.dataGridFactory = dataGridFactory;

            this.tileMan = tileMan;
            this.logger = logger;
            this.triggerMan = triggerMan;
            this.scriptMan = scriptMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public static int GetActionCellValue(MapLayoutModel layout, int ix, int iy)
        {
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            var values = layout.GetCellValues(ix, iy);

            return values[actionLayer];
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
            var mapScript = LoadReferencedScripts(dbMap, world);

            var layout = map.Layout;
            var visited = new bool[layout.Width, layout.Height];

            var cellSize = layout.CellSize;

            var worldBuilder = worldMan.Create();
            worldBuilder.SetName(entryId);
            worldBuilder.SetSize(layout.Width, layout.Width);
            worldBuilder.AddModule(dataGridFactory.Create<Entity>(layout.Width, layout.Height));
            worldBuilder.AddModule(broadphaseGridFactory.CreateStatic(layout.Width, layout.Height, cellSize));
            worldBuilder.AddModule(broadphaseGridFactory.CreateDynamic());
            worldBuilder.AddModule(tileGridFactory.CreateGrid(layout.Width, layout.Height, 1, cellSize));
            worldBuilder.AddModule(renderableFactory.CreateRenderableBatch());

            worldBuilder.SetupGameWorldSystems(systemFactory);

            world = worldBuilder.Build();

            var mapper = new MapMapper(dbMap.TileSetRef);

            var atlasId = tileMan.GetByName(mapper.Level).Id;
            var tileGrid = world.GetModule<ITileGrid>();
            var entityGrid = world.GetModule<IDataGrid<Entity>>();

            var gfxLayer = layout.GetLayerIndex(MapLayerType.Gfx);
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            triggerMan.OnWorldInitialized(world, () =>
            {
                if(mapScript != null)
                    mapScript.Invoke();

                for (int iy = 0; iy < layout.Height; iy++)
                {
                    for (int ix = 0; ix < layout.Width; ix++)
                    {
                        var cellValues = layout.GetCellValues(ix, iy);
                        var gfxValue = cellValues[gfxLayer];
                        var actionValue = cellValues[actionLayer];

                        var action = map.GetAction(actionValue);

                        var indexPos = new Vector2i(ix, iy);

                        tileGrid.ModifyTile(indexPos, atlasId, gfxValue);

                        if (action != null)
                        {
                            var cellEntity = LoadCellEntity(mapper, map, visited, ix, iy, world, action, gfxValue);

                            if (cellEntity is not null)
                                entityGrid.Set(indexPos, cellEntity);
                        }

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
                        var indexPos = new Vector2i(ix, iy);

                        var cellEntity = LoadUnknownCodeCell(mapper, map, visited, ix, iy, gfxValue, actionValue, world);
                    
                        if(cellEntity is not null)
                            entityGrid.Set(indexPos, cellEntity);
                    }
                }

                scriptMan.TryInvokeFunction("MapLoaded", world.Id);
            }, singleTime: true);

            return world;
        }

        public object LoadObject(string entryId) => Load(entryId);

        public void Register(string templateName, IMapWorldEntityLoader entityLoader)
        {
            entityLoaders.Add(templateName, entityLoader);
        }

        #endregion Public Methods

        #region Private Methods

        private Entity LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, World world, ActionModel action, int gfxValue)
        {
            if (visited[ix, iy])
                return null;

            if (entityLoaders.TryGetValue(action.Name, out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(mapAssets, map, visited, ix, iy, action.Name, "", gfxValue, world);

            logger.Warning($"Missing loader for action '{action}'");
            return null;
        }

        private void LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, World world, string templateName, string flavor, int gfxValue)
        {
            if (visited[ix, iy])
                return;

            if (entityLoaders.TryGetValue(templateName, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(mapAssets, map, visited, ix, iy, templateName, flavor, gfxValue, world);
        }

        private void LoadPalettes(MapModel mapModel)
        {
            foreach (var paletteModel in mapModel.Palettes)
            {
                var paletteEntityTag = $"GameWorld/Palette/{paletteModel.Name}";

                var paletteEntity = entityMan.GetByTag(paletteEntityTag).FirstOrDefault();

                if (paletteEntity is null)
                {
                    paletteEntity = entityMan.Create(tag: paletteEntityTag);
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

        private IScriptFunc LoadReferencedScripts(IDbMap dbMap, World world)
        {
            var loader = dataLoaderFactory.GetLoader<IScriptDataLoader>();

            if (dbMap.ScriptRef is null)
                return null;

            var dbScript = repositoryProvider.GetRepository<IDbScript>().GetById(dbMap.ScriptRef);

            return loader.Load(dbScript.Id);
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

        private void LoadReferencedTileSet(IDbMap dbMap)
        {
            var tileAtlasLoader = dataLoaderFactory.GetLoader<ITileAtlasDataLoader>();

            var palette = palettesDataProvider.GetPalette(dbMap.PaletteRefs.First());

            tileAtlasLoader.Load(dbMap.TileSetRef, palette);
        }

        private void LoadReferencedTileStamps(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ITileStampDataLoader>();

            var dbTileStamps = repositoryProvider.GetRepository<IDbTileStamp>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));

            foreach (var dbTileStamp in dbTileStamps)
                loader.Load(dbTileStamp.Id);
        }

        private Entity LoadUnknownCodeCell(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, World world)
        {
            if (entityLoaders.TryGetValue("Unknown", out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(worldBlockBuilder, map, visited, ix, iy, "Unknown", actionValue.ToString(), gfxValue, world);

            return null;
        }

        #endregion Private Methods
    }
}