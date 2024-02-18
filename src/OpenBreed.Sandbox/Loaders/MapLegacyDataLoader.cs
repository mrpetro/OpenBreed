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
using OpenBreed.Model.Palettes;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Hud;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Wecs.Components;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;

namespace OpenBreed.Sandbox.Loaders
{
    public interface IMapDataLoader : IDataLoader<IWorld>
    {
        #region Public Methods

        void Register(string templateName, IMapWorldEntityLoader entityLoader);

        #endregion Public Methods
    }

    public interface IMapWorldEntityLoader
    {
        #region Public Methods

        IEntity Load(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, string templateName, string flavor, int gfxValue, IWorld world);

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
        private readonly IEntityFactory entityFactory;
        private readonly IBuilderFactory builderFactory;
        private readonly ISystemFactory systemFactory;
        private readonly ITileGridFactory tileGridFactory;
        private readonly IDataGridFactory dataGridFactory;
        private readonly ITileMan tileMan;
        private readonly IPaletteMan paletteMan;
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
                                   IPaletteMan paletteMan,
                                   ILogger logger,
                                   ITriggerMan triggerMan,
                                   IScriptMan scriptMan,
                                   IEntityFactory entityFactory,
                                   IBuilderFactory builderFactory)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.renderableFactory = renderableFactory;
            this.entityMan = entityMan;
            this.mapsDataProvider = mapsDataProvider;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            this.palettesDataProvider = palettesDataProvider;
            this.broadphaseGridFactory = broadphaseGridFactory;
            this.tileGridFactory = tileGridFactory;
            this.dataGridFactory = dataGridFactory;
            this.tileMan = tileMan;
            this.paletteMan = paletteMan;
            this.logger = logger;
            this.triggerMan = triggerMan;
            this.scriptMan = scriptMan;
            this.entityFactory = entityFactory;
            this.builderFactory = builderFactory;
        }

        #endregion Public Constructors

        #region Public Methods

        public static int GetActionCellValue(MapLayoutModel layout, int ix, int iy)
        {
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            var values = layout.GetCellValues(ix, iy);

            return values[actionLayer];
        }

        public IEntity CreateMapEntity()
        {
            return entityMan.Create($"Maps");
        }

        public IWorld Load(string entryId, params object[] args)
        {
            var world = worldMan.GetByName(entryId);

            if (world != null)
                return world;

            var dbMap = repositoryProvider.GetRepository<IDbMap>().GetById(entryId);
            if (dbMap is null)
                throw new Exception($"Missing Map: {entryId}");

            var map = mapsDataProvider.GetMap(dbMap.Id);

            if (map is null)
                throw new Exception($"Map model asset '{dbMap.DataRef}' could not be loaded.");

            LoadPalettes(map, dbMap.Id);
            LoadReferencedTileSet(dbMap);
            LoadReferencedSpriteSets(dbMap);
            LoadReferencedAnimations(dbMap);
            LoadReferencedTileStamps(dbMap);
            LoadReferencedSounds(dbMap);

            var mapEntity = CreateMapEntity();

            var layout = map.Layout;
            var visited = new bool[layout.Width, layout.Height];

            var cellSize = layout.CellSize;

            var tileGridComponent = builderFactory.GetBuilder<TileGridComponentBuilder>()
                .SetGrid(layout.Width, layout.Height, 1, cellSize)
                .Build();

            var dataGridComponent = builderFactory.GetBuilder<DataGridComponentBuilder>()
                .SetGrid(layout.Width, layout.Height)
            .Build();

            var collisionComponent = builderFactory.GetBuilder<CollisionComponentBuilder>()
                .SetStaticGrid(layout.Width, layout.Height, cellSize)
                .Build();

            mapEntity.Add(new StampPutterComponent());
            mapEntity.Add(tileGridComponent);
            mapEntity.Add(dataGridComponent);
            mapEntity.Add(collisionComponent);

            var worldBuilder = worldMan.Create();
            worldBuilder.SetName(entryId);

            worldBuilder.SetupGameWorldSystems();

            world = worldBuilder.Build();

            var mapper = new MapMapper(dbMap.TileSetRef);

            var atlasId = tileMan.GetByName(mapper.Level).Id;
            var gfxLayer = layout.GetLayerIndex(MapLayerType.Gfx);
            var actionLayer = layout.GetLayerIndex(MapLayerType.Action);

            triggerMan.OnWorldInitialized(world, () =>
            {
                worldMan.RequestAddEntity(mapEntity, world.Id);

                var paletteEntityTag = $"Palettes/{dbMap.Id}";
                var paletteEntity = entityMan.GetByTag(paletteEntityTag).FirstOrDefault();

                if(paletteEntity is not null)
                    worldMan.RequestAddEntity(paletteEntity, world.Id);

                for (int iy = 0; iy < layout.Height; iy++)
                {
                    for (int ix = 0; ix < layout.Width; ix++)
                    {
                        var cellValues = layout.GetCellValues(ix, iy);
                        var gfxValue = cellValues[gfxLayer];
                        var actionValue = cellValues[actionLayer];

                        var action = map.GetAction(actionValue);

                        var indexPos = new Vector2i(ix, iy);

                        tileGridComponent.Grid.ModifyTile(indexPos, atlasId, gfxValue);

                        if (action is null)
                            continue;

                        var cellEntity = LoadCellEntity(mapper, map, visited, ix, iy, world, action, gfxValue);

                        if (cellEntity is null)
                            continue;

                        dataGridComponent.Grid.Set(indexPos, cellEntity.Id);

                        //Check if cell entity has static body
                        //if (cellEntity.Contains<PositionComponent>() &&
                        //    cellEntity.Contains<BodyComponent>() &&
                        //    !cellEntity.Contains<VelocityComponent>())
                        //{
                        //    mapEntity.AddEntityToStatics(cellEntity);
                        //}

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
                            dataGridComponent.Grid.Set(indexPos, cellEntity.Id);
                    }
                }

                AddMission(world);
                AddDirector(world, dbMap.ScriptRef);

                //DEBUG entities
                AddCursor(world);
            }, singleTime: true);

            return world;
        }

        private void AddCursor(IWorld world)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Hud\Cursor")
                .SetParameter("posX", 0.0f)
                .SetParameter("posY", 0.0f)
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);
        }

        private void AddMission(IWorld world)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Mission")
                .SetParameter("scriptId", "Vanilla/Common/Mission")
                .SetTag("Mission")
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);
        }

        private void AddDirector(IWorld world, string scriptId)
        {
            var entity = entityFactory.Create(@"ABTA\Templates\Common\Director")
                .SetParameter("scriptId", scriptId)
                .SetTag("Director")
                .Build();

            worldMan.RequestAddEntity(entity, world.Id);
        }

        public object LoadObject(string entryId) => Load(entryId);

        public void Register(string templateName, IMapWorldEntityLoader entityLoader)
        {
            entityLoaders.Add(templateName, entityLoader);
        }

        #endregion Public Methods

        #region Private Methods

        private IEntity LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, IWorld world, ActionModel action, int gfxValue)
        {
            if (visited[ix, iy])
                return null;

            if (entityLoaders.TryGetValue(action.Name, out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(mapAssets, map, visited, ix, iy, action.Name, "", gfxValue, world);

            logger.Warning($"Missing loader for action '{action}'");
            return null;
        }

        private void LoadCellEntity(MapMapper mapAssets, MapModel map, bool[,] visited, int ix, int iy, IWorld world, string templateName, string flavor, int gfxValue)
        {
            if (visited[ix, iy])
                return;

            if (entityLoaders.TryGetValue(templateName, out IMapWorldEntityLoader entityLoader))
                entityLoader.Load(mapAssets, map, visited, ix, iy, templateName, flavor, gfxValue, world);
        }

        private void LoadPalettes(MapModel mapModel, string mapId)
        {
            var commonPaletteModel = palettesDataProvider.GetPalette("Palettes.COMMON");
            var mapPaletteModel = mapModel.Palettes.First();

            var paletteEntityTag = $"Palettes/{mapId}";

            var paletteEntity = entityMan.GetByTag(paletteEntityTag).FirstOrDefault();

            if (paletteEntity is not null)
            {
                return;
            }   

            paletteEntity = entityMan.Create(tag: paletteEntityTag);

            var paletteComponent = paletteEntity.TryGet<PaletteComponent>();

            if (paletteComponent is null)
            {
                paletteComponent = new PaletteComponent();
                paletteEntity.Add(paletteComponent);
            }

            var builder = paletteMan.CreatePalette()
                .SetName(paletteEntityTag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => PaletteHelper.ToColor4(color)).ToArray())
                .SetColors(mapPaletteModel.Data.Take(64).Select(color => PaletteHelper.ToColor4(color)).ToArray());

            //for (int i = 0; i < 256; i++)
            //{
            //    var c = commonPaletteModel[i];
            //    builder.SetColor(i, new Color4(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f));
            //}


            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

            var palette = builder.Build();

            paletteComponent.PaletteId = palette.Id;

        }

        private void LoadReferencedAnimations(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<IAnimationClipDataLoader<IEntity>>();

            //Load common animations
            var dbAnims = repositoryProvider.GetRepository<IDbAnimation>().Entries.Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbAnim in dbAnims)
                loader.Load(dbAnim.Id);

            //Load level specific animations
            dbAnims = repositoryProvider.GetRepository<IDbAnimation>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbAnim in dbAnims)
                loader.Load(dbAnim.Id);
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

            //Load common sprites
            var dbSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbAnim in dbSpriteAtlas)
                loader.Load(dbAnim.Id);

            //Load level specific sprites
            dbSpriteAtlas = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbAnim in dbSpriteAtlas)
                loader.Load(dbAnim.Id);

            //var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //foreach (var spriteSetRef in dbMap.SpriteSetRefs)
            //    loader.Load(spriteSetRef, palette);
        }

        private void LoadReferencedTileSet(IDbMap dbMap)
        {
            var tileAtlasLoader = dataLoaderFactory.GetLoader<ITileAtlasDataLoader>();

            tileAtlasLoader.Load(dbMap.TileSetRef);
        }

        private void LoadReferencedTileStamps(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ITileStampDataLoader>();

            var dbTileStamps = repositoryProvider.GetRepository<IDbTileStamp>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));

            foreach (var dbTileStamp in dbTileStamps)
                loader.Load(dbTileStamp.Id);
        }

        private IEntity LoadUnknownCodeCell(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, IWorld world)
        {
            if (entityLoaders.TryGetValue("Unknown", out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(worldBlockBuilder, map, visited, ix, iy, "Unknown", actionValue.ToString(), gfxValue, world);

            return null;
        }

        #endregion Private Methods
    }
}