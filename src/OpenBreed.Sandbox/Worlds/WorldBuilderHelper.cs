using OpenBreed.Core;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.Turret;
using OpenBreed.Sandbox.Entities.Viewport;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Sandbox.Worlds
{
    public class WorldBuilderHelper
    {
        #region Public Fields

        public const int PLAYER_SPAWN_POINT = 'P';

        #endregion Public Fields

        #region Private Fields

        private WorldBuilder builder;
        private readonly IEntityMan entityMan;
        private readonly ViewportCreator viewportCreator;
        private Dictionary<int, Tuple<string, int>> exits = new Dictionary<int, Tuple<string, int>>();

        private Dictionary<int, (int, int, string)> viewports = new Dictionary<int, (int, int, string)>();


        #endregion Private Fields

        #region Public Constructors

        public WorldBuilderHelper(WorldBuilder builder, IEntityMan entityMan, ViewportCreator viewportCreator)
        {
            this.builder = builder;
            this.entityMan = entityMan;
            this.viewportCreator = viewportCreator;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RegisterExit(int exitNo, string mapName, int mapEntryId)
        {
            exits.Add(exitNo, new Tuple<string, int>(mapName, mapEntryId));
        }

        public void RegisterViewport(int viewportNo, int width, int height, string cameraName)
        {
            viewports.Add(viewportNo, (width, height, cameraName));
        }

        public void RegisterHandlers()
        {
            builder.RegisterCode('O', AddObstacleCell);
            builder.RegisterCode('D', AddDoor);
            builder.RegisterCode('>', AddTeleportEntry);
            builder.RegisterCode('<', AddTeleportExit);
            builder.RegisterCode('}', AddWorldEntry);
            builder.RegisterCode('{', AddWorldExit);
            builder.RegisterCode('V', AddViewport);
            builder.RegisterCode('T', AddTurret);
            builder.RegisterCode('A', AddAnimTest);
            builder.RegisterCode(' ', AddAirCell);

            //builder.RegisterCodeModule(' ', new AirBuilderModule(commandsMan, worldBlockBuilder);

            builder.RegisterCode(PLAYER_SPAWN_POINT, AddPlayer);
        }

        #endregion Public Methods

        #region Private Methods

        private void AddViewport(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            if (!viewports.TryGetValue(pairCode, out (int Width, int Height, string CameraName ) viewportData))
                return;

            var vp = viewportCreator.CreateViewportEntity($"TV{pairCode}" , x * 16, y * 16, viewportData.Width, viewportData.Height, ScreenWorldHelper.GAME_VIEWPORT);

            vp.Get<ViewportComponent>().CameraEntityId = entityMan.GetByTag(viewportData.CameraName).FirstOrDefault().Id;
            vp.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //GameWorldHelper.SetPreserveAspectRatio(vp);
            core.Commands.Post(new AddEntityCommand(world.Id, vp.Id));
            //world.AddEntity(vp);
        }

        private static void AddTeleportEntry(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            var teleportEntry = TeleportHelper.AddTeleportEntry(core, world, x, y, pairCode);
            core.Commands.Post(new TileSetCommand(teleportEntry.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddWorldEntry(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            var worldGateHelper = core.GetManager<WorldGateHelper>();
            var worldEntry = worldGateHelper.AddWorldEntry(world, x, y, pairCode);
            core.Commands.Post(new TileSetCommand(worldEntry.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddAnimTest(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            var crazyMover = Misc.AddToWorld(core, world);
            core.Commands.Post(new TileSetCommand(crazyMover.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddTurret(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            var turret = TurretHelper.Create(core, new Vector2(x * 16, y * 16));
            core.Commands.Post(new AddEntityCommand(world.Id, turret.Id));
            core.Commands.Post(new TileSetCommand(turret.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddTeleportExit(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            var teleportEntity = TeleportHelper.AddTeleportExit(core, world, x, y, pairCode);
            core.Commands.Post(new TileSetCommand(teleportEntity.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private void AddWorldExit(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var exitNo = (int)args[2];

            Tuple<string, int> exitInfo;
            if (!exits.TryGetValue(exitNo, out exitInfo))
                return;

            var worldGateHelper = core.GetManager<WorldGateHelper>();

            var worldExit = worldGateHelper.AddWorldExit(world, x, y, exitInfo.Item1, exitInfo.Item2);
            core.Commands.Post(new TileSetCommand(worldExit.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddPlayer(ICore core, World world, int code, object[] args)
        {
        }

        public static int ToTileId(int gfxCode)
        {
            switch (gfxCode)
            {
                case '1':
                    return 14;

                case '2':
                    return 10;

                case '3':
                    return 15;

                case '4':
                    return 11;

                case '5':
                    return 12;

                case '6':
                    return 11;

                case '7':
                    return 6;

                case '8':
                    return 10;

                case '9':
                    return 7;

                default:
                    return 0;
            }
        }

        private static void AddAirCell(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var gfxCode = (int)args[2];

            var blockBuilder = core.GetManager<WorldBlockBuilder>();
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");
            blockBuilder.HasBody = false;
            blockBuilder.SetPosition(x * 16, y * 16);
            blockBuilder.SetTileId(ToTileId(gfxCode));

            var entity = blockBuilder.Build();
            core.Commands.Post(new AddEntityCommand(world.Id, entity.Id));
            //world.AddEntity(blockBuilder.Build());
        }

        private static void AddObstacleCell(ICore core, World world, int code, object[] args)
        {
            var x = (int)args[0];
            var y = (int)args[1];
            var gfxCode = (int)args[2];

            var blockBuilder = core.GetManager<WorldBlockBuilder>();
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");
            blockBuilder.HasBody = true;
            blockBuilder.SetPosition(x * 16, y * 16);
            blockBuilder.SetTileId(ToTileId(gfxCode));

            var entity = blockBuilder.Build();
            core.Commands.Post(new AddEntityCommand(world.Id, entity.Id));
            //world.AddEntity(blockBuilder.Build());
        }

        private static void AddDoor(ICore core, World world, int code, object[] args)
        {
            var doorHelper = core.GetManager<DoorHelper>();

            var x = (int)args[0];
            var y = (int)args[1];
            var type = (int)args[2];

            if (type == 'H')
                doorHelper.AddHorizontalDoor(world, x, y);
            else if (type == 'V')
                doorHelper.AddVerticalDoor(world, x, y);
        }

        #endregion Private Methods
    }
}