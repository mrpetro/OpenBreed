using Microsoft.Extensions.Logging;
using OpenBreed.Animation.Abstractions.Data;
using OpenBreed.Audio.Abstractions.Data;
using OpenBreed.Common.Data;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Common.Game.Wecs.Systems.Hud;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Managers;
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
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Sandbox.Systems;
using OpenBreed.Sandbox.Systems.Actor;
using OpenBreed.Sandbox.Systems.Mission;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Physics.Components;
using OpenBreed.Wecs.Physics.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Documents;
using System.Xml.Linq;
using OpenBreed.Sandbox.Extensions;

namespace OpenBreed.Sandbox.Loaders
{
    public interface IMapDataLoader : IDataLoader<IWorld>
    {
        #region Public Methods

        void Register(string templateName, IMapWorldEntityLoader entityLoader);

        bool TryGetEntityLoader(string templateName, out IMapWorldEntityLoader entityLoader);

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

        public IWorld Load(string entryId)
        {
            var world = worldMan.GetByName(entryId);

            if (world != null)
            {
                return world;
            }

            var dbMap = repositoryProvider.GetRepository<IDbMap>().GetById(entryId);

            if (dbMap is null)
            {
                throw new Exception($"Missing Map: {entryId}");
            }

            return Load(dbMap);
        }

        public void Register(string templateName, IMapWorldEntityLoader entityLoader)
        {
            entityLoaders.Add(templateName, entityLoader);
        }

        public bool TryGetEntityLoader(string templateName, out IMapWorldEntityLoader entityLoader)
        {
            return entityLoaders.TryGetValue(templateName, out entityLoader);
        }

        #endregion Public Methods

        #region Private Methods

        private IWorld Load(IDbMap dbMap)
        {
            var world = worldMan.CreateGameWorld(dbMap.Id);

            var map = mapsDataProvider.GetMap(dbMap.Id);

            if (map is null)
            {
                throw new Exception($"Map model asset '{dbMap.DataRef}' could not be loaded.");
            }

            LoadPalettes(map, dbMap.Id);
            LoadReferencedTileSet(dbMap);
            LoadReferencedSpriteSets(dbMap);
            LoadReferencedAnimations(dbMap);
            LoadReferencedTileStamps(dbMap);
            LoadReferencedSounds(dbMap);

            return world;
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

            var builder = paletteMan.CreatePalette()
                .SetName(paletteEntityTag)
                .SetLength(256)
                .SetColors(commonPaletteModel.Data.Select(color => color.ToColor4()).ToArray())
                .SetColors(mapPaletteModel.Data.Take(64).Select(color => color.ToColor4()).ToArray());

            //for (int i = 0; i < 256; i++)
            //{
            //    var c = commonPaletteModel[i];
            //    builder.SetColor(i, new Color4(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f));
            //}

            var cb = commonPaletteModel[0];
            builder.SetColor(0, new Color4<Rgba>(cb.R / 255.0f, cb.G / 255.0f, cb.B / 255.0f, 0.0f));

            var palette = builder.Build();

            var paletteComponent = new PaletteComponent();
            paletteComponent.PaletteId = palette.Id;

            paletteEntity = entityMan.Create()
                .SetTag(paletteEntityTag)
                .AddComponent(paletteComponent)
                .Build();
        }

        private void LoadReferencedAnimations(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<IReadOnlyClipDataLoader<IEntity>>();

            //Load common animations
            var dbAnims = repositoryProvider.GetRepository<IDbAnimation>().Entries.OfType<IDbAnimation>().Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbAnim in dbAnims)
            {
                loader.Load(dbAnim);
            }

            //Load level specific animations
            dbAnims = repositoryProvider.GetRepository<IDbAnimation>().Entries.OfType<IDbAnimation>().Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbAnim in dbAnims)
            {
                loader.Load(dbAnim);
            }
        }

        private void LoadReferencedSounds(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ISoundSampleDataLoader>();

            //Load common sounds
            var dbSounds = repositoryProvider.GetRepository<IDbSound>().Entries.OfType<IDbSound>().Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbSound in dbSounds)
            {
                loader.Load(dbSound);
            }

            //Load level specific sounds
            dbSounds = repositoryProvider.GetRepository<IDbSound>().Entries.OfType<IDbSound>().Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbSound in dbSounds)
            {
                loader.Load(dbSound);
            }
        }

        private void LoadReferencedSpriteSets(IDbMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ISpriteAtlasDataLoader>();

            //Load common sprites
            var dbSpriteAtlases = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.OfType<IDbSpriteAtlas>().Where(item => item.Id.StartsWith("Vanilla/Common"));
            foreach (var dbStriteAtlas in dbSpriteAtlases)
            {
                loader.Load(dbStriteAtlas);
            }

            //Load level specific sprites
            dbSpriteAtlases = repositoryProvider.GetRepository<IDbSpriteAtlas>().Entries.OfType<IDbSpriteAtlas>().Where(item => item.Id.StartsWith(dbMap.TileSetRef));
            foreach (var dbStriteAtlas in dbSpriteAtlases)
            {
                loader.Load(dbStriteAtlas);
            }

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

            var dbTileStamps = repositoryProvider.GetRepository<IDbTileStamp>().Entries.OfType<IDbTileStamp>().Where(item => item.Id.StartsWith(dbMap.TileSetRef));

            foreach (var dbTileStamp in dbTileStamps)
            {
                loader.Load(dbTileStamp);
            }
        }

        #endregion Private Methods
    }
}