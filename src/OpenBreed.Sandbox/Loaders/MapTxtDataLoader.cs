using OpenBreed.Animation.Interface.Data;
using OpenBreed.Common;
using OpenBreed.Common.Data;
using OpenBreed.Common.Interface;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Database.Interface;
using OpenBreed.Database.Interface.Items.Animations;
using OpenBreed.Database.Interface.Items.Maps;
using OpenBreed.Database.Interface.Items.Sprites;
using OpenBreed.Database.Interface.Items.TileStamps;
using OpenBreed.Model.Actions;
using OpenBreed.Model.Maps;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Data;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Loaders
{
    internal class TxtMap
    {
        public string[] PaletteRefs { get; internal set; }
        public string TileSetRef { get; internal set; }
        public string DataRef { get; internal set; }
        public string[] SpriteSetRefs { get; internal set; }
        public string ActionSetRef { get; internal set; }
        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private char[] gfxLayout;
        private char[] actionLayout;
        private readonly Dictionary<char, int> actionCodes = new Dictionary<char, int>();
        private readonly Dictionary<char, int> gfxCodes = new Dictionary<char, int>();

        public void SetAction(int x, int y, char value)
        {
            actionLayout[x + y * Width] = value;
        }

        public char GetAction(int x, int y)
        {
            return actionLayout[x + y * Width];
        }

        public char GetGfx(int x, int y)
        {
            return gfxLayout[x + y * Width];
        }

        public void SetGfx(int x, int y, char value)
        {
            gfxLayout[x + y * Width] = value;
        }

        internal MapModel GetModel()
        {
            var mapBuilder = MapBuilder.NewMapModel();
            var layoutBuilder = mapBuilder.CreateLayout();
            layoutBuilder.SetCellSize(16);
            layoutBuilder.SetSize(Width, Height);

            var gfxLayerBuilder = layoutBuilder.AddLayer(MapLayerType.Gfx);
            gfxLayerBuilder.SetAllValues(2);

            var actionLayerBuilder = layoutBuilder.AddLayer(MapLayerType.Action);
            actionLayerBuilder.SetAllValues(0);

            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    var actionValue = GetAction(i, j);
                    var gfxValue = GetGfx(i, j);

                    actionLayerBuilder.SetValue(i, j, GetActionCode(actionValue));
                    gfxLayerBuilder.SetValue(i, j, GetGfxCode(gfxValue));
                }
            }

            //actionLayerBuilder.SetValue(4, 4, 56);

            return mapBuilder.Build();
        }

        public int GetActionCode(char ch)
        {
            if (actionCodes.TryGetValue(ch, out int code))
                return code;
            else
                return 0;
        }

        public int GetGfxCode(char ch)
        {
            if (gfxCodes.TryGetValue(ch, out int code))
                return code;
            else
                return 0;
        }

        internal static TxtMap Read(string entryId)
        {
            var lines = File.ReadAllLines(entryId);

            var txtMap = new TxtMap
            {
                PaletteRefs = new string[] { "Palettes.MAP.02.CMAP", "Palettes.MAP.02.ALCM" },
                TileSetRef = "Vanilla/L4",
                ActionSetRef = "Vanilla/L4",
                SpriteSetRefs = new string[] { "Vanilla/L4/A", "Vanilla/L4/B", "Vanilla/Common/Hero/1", "Vanilla/Common/Hero/2" }
            };

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (string.IsNullOrEmpty(line))
                    continue;

                var split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 0)
                    continue;

                var cmd = split[0];
                var args = split.Skip(1).ToArray();

                switch (cmd)
                {
                    case "NAME":
                        txtMap.Name = args[0];
                        break;
                    case "SIZE":
                        txtMap.Width = int.Parse(args[0]);
                        txtMap.Height = int.Parse(args[1]);
                        break;
                    case "BODY":
                        ReadBody(txtMap, lines, ref i);
                        break;
                    case "A_CODE":
                        SetActionCode(txtMap, args);
                        break;
                    case "G_CODE":
                        SetGfxCode(txtMap, args);
                        break;
                    default:
                        break;
                }

            }

            return txtMap;
        }

        public static void SetActionCode(TxtMap txtMap, string[] args)
        {
            var code = int.Parse(args[1]);
            txtMap.actionCodes.Add(args[0][0], code);
        }

        public static void SetGfxCode(TxtMap txtMap, string[] args)
        {
            var code = int.Parse(args[1]);
            txtMap.gfxCodes.Add(args[0][0], code);
        }

        public static void ReadBody(TxtMap txtMap, string[] lines, ref int lineNo)
        {
            txtMap.gfxLayout = new char[txtMap.Width * txtMap.Height];
            txtMap.actionLayout = new char[txtMap.Width * txtMap.Height];

            lineNo++;

            for (int j = 0; j < txtMap.Height; j++)
            {
                var row = lines[lineNo + j];

                for (int i = 0; i < txtMap.Width; i++)
                {
                    var actionChar = row[i * 2];
                    var gfxChar = row[i * 2 + 1];

                    txtMap.SetAction(i,j, actionChar);
                    txtMap.SetGfx(i, j, gfxChar);
                }
            }
        }
    }


    public class MapTxtDataLoader : IMapDataLoader
    {
        #region Private Fields

        private readonly IRepositoryProvider repositoryProvider;
        private readonly IDataLoaderFactory dataLoaderFactory;
        private readonly IRenderableFactory renderableFactory;
        private readonly ISystemFactory systemFactory;
        private readonly IWorldMan worldMan;
        //private readonly WorldBlockBuilder worldBlockBuilder;
        private readonly PalettesDataProvider palettesDataProvider;
        private readonly ActionSetsDataProvider actionSetsDataProvider;
        private readonly IBroadphaseFactory broadphaseGridFactory;
        private readonly ITileGridFactory tileGridFactory;
        private readonly ITileMan tileMan;
        private readonly ILogger logger;
        private readonly Dictionary<string, IMapWorldEntityLoader> entityLoaders = new Dictionary<string, IMapWorldEntityLoader>();

        #endregion Private Fields

        #region Public Constructors

        public MapTxtDataLoader(IDataLoaderFactory dataLoaderFactory,
                                IRenderableFactory renderableFactory,
                                  IRepositoryProvider repositoryProvider,
                                  ISystemFactory systemFactory,
                                  IWorldMan worldMan,
                                  PalettesDataProvider palettesDataProvider,
                                  ActionSetsDataProvider actionSetsDataProvider,
                                  IBroadphaseFactory broadphaseGridFactory,
                                  ITileGridFactory tileGridFactory,
                                  ITileMan tileMan,
                                  ILogger logger)
        {
            this.repositoryProvider = repositoryProvider;
            this.dataLoaderFactory = dataLoaderFactory;
            this.renderableFactory = renderableFactory;
            this.systemFactory = systemFactory;
            this.worldMan = worldMan;
            //this.worldBlockBuilder = worldBlockBuilder;
            this.palettesDataProvider = palettesDataProvider;
            this.actionSetsDataProvider = actionSetsDataProvider;
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

        public IWorld Load(string entryId, params object[] args)
        {
            var world = worldMan.GetByName(entryId);

            if (world != null)
                return world;

            var txtMap = TxtMap.Read(entryId);

            LoadReferencedTileSet(txtMap);
            LoadReferencedSpriteSets(txtMap);
            LoadReferencedAnimations(txtMap);
            LoadReferencedTileStamps(txtMap);

            var map = txtMap.GetModel();

            if (map is null)
                throw new Exception($"Map model  asset '{txtMap.DataRef}' could not be loaded.");

            map.ActionSet = actionSetsDataProvider.GetActionSet(txtMap.ActionSetRef);

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

            worldBuilder.SetupGameWorldSystems();
            world = worldBuilder.Build();

            var mapper = new MapMapper(txtMap.TileSetRef);

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

                    if (action != null)
                    {
                        var cellEntity = LoadCellEntity(mapper, map, visited, ix, iy, world, action, gfxValue);

                        if (cellEntity is null)
                            continue;
                    }

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

                    var cellEntity = PutUnknownCodeCell(mapper, map, visited, ix, iy, gfxValue, actionValue, world);

                    if (cellEntity is null)
                        continue;
                }
            }

            return world;
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

        private IEntity PutUnknownCodeCell(MapMapper worldBlockBuilder, MapModel map, bool[,] visited, int ix, int iy, int gfxValue, int actionValue, IWorld world)
        {
            if (entityLoaders.TryGetValue("Unknown", out IMapWorldEntityLoader entityLoader))
                return entityLoader.Load(worldBlockBuilder, map, visited, ix, iy, "Unknown", actionValue.ToString(), gfxValue, world);

            return null;
        }

        private void LoadReferencedTileSet(TxtMap dbMap)
        {
            var tileAtlasLoader = dataLoaderFactory.GetLoader<ITileAtlasDataLoader>();

            tileAtlasLoader.Load(dbMap.TileSetRef);
        }

        private void LoadReferencedSpriteSets(TxtMap dbMap)
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

            foreach (var spriteSetRef in dbMap.SpriteSetRefs)
                loader.Load(spriteSetRef, palette);
        }

        private void LoadReferencedAnimations(TxtMap dbMap)
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

        private void LoadReferencedTileStamps(TxtMap dbMap)
        {
            var loader = dataLoaderFactory.GetLoader<ITileStampDataLoader>();

            var dbTileStamps = repositoryProvider.GetRepository<IDbTileStamp>().Entries.Where(item => item.Id.StartsWith(dbMap.TileSetRef));

            foreach (var dbTileStamp in dbTileStamps)
                loader.Load(dbTileStamp.Id);
        }


        #endregion Private Methods
    }
}
