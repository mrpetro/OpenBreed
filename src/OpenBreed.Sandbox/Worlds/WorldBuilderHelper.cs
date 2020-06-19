using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.WorldGate;
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

        private Dictionary<int, Tuple<string, int>> exits = new Dictionary<int, Tuple<string, int>>();

        private Dictionary<int, (int, int, string)> viewports = new Dictionary<int, (int, int, string)>();


        #endregion Private Fields

        #region Public Constructors

        public WorldBuilderHelper(WorldBuilder builder)
        {
            this.builder = builder;
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

            builder.RegisterCode(' ', AddAirCell);

            builder.RegisterCode(PLAYER_SPAWN_POINT, AddPlayer);
        }

        #endregion Public Methods

        #region Private Methods

        private void AddViewport(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            if (!viewports.TryGetValue(pairCode, out (int Width, int Height, string CameraName ) viewportData))
                return;

            var vp = ScreenWorldHelper.CreateViewportEntity(core, $"TV{pairCode}" , x * 16, y * 16, viewportData.Width, viewportData.Height, true);

            vp.Get<ViewportComponent>().CameraEntityId = core.Entities.GetByTag(viewportData.CameraName).FirstOrDefault().Id;
            vp.Get<ViewportComponent>().ScalingType = ViewportScalingType.FitBothPreserveAspectRatio;
            //GameWorldHelper.SetPreserveAspectRatio(vp);
            world.Core.Commands.Post(new AddEntityCommand(world.Id, vp.Id));
            //world.AddEntity(vp);
        }

        private static void AddTeleportEntry(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            TeleportHelper.AddTeleportEntry(world, x, y, pairCode);
            world.Core.Commands.Post(new TileSetCommand(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddWorldEntry(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            WorldGateHelper.AddWorldEntry(world, x, y, pairCode);
            world.Core.Commands.Post(new TileSetCommand(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddTeleportExit(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            TeleportHelper.AddTeleportExit(world, x, y, pairCode);
            world.Core.Commands.Post(new TileSetCommand(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private void AddWorldExit(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var exitNo = (int)args[2];

            Tuple<string, int> exitInfo;
            if (!exits.TryGetValue(exitNo, out exitInfo))
                return;

            WorldGateHelper.AddWorldExit(world, x, y, exitInfo.Item1, exitInfo.Item2);
            world.Core.Commands.Post(new TileSetCommand(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddPlayer(World world, int code, object[] args)
        {
        }

        private static int ToTileId(int gfxCode)
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

        private static void AddAirCell(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var gfxCode = (int)args[2];

            var blockBuilder = new WorldBlockBuilder(world.Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");
            blockBuilder.HasBody = false;
            blockBuilder.SetPosition(new Vector2(x * 16, y * 16));
            blockBuilder.SetTileId(ToTileId(gfxCode));

            var entity = blockBuilder.Build();
            world.Core.Commands.Post(new AddEntityCommand(world.Id, entity.Id));
            //world.AddEntity(blockBuilder.Build());
        }

        private static void AddObstacleCell(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var gfxCode = (int)args[2];

            var blockBuilder = new WorldBlockBuilder(world.Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");
            blockBuilder.HasBody = true;
            blockBuilder.SetPosition(new Vector2(x * 16, y * 16));
            blockBuilder.SetTileId(ToTileId(gfxCode));

            var entity = blockBuilder.Build();
            world.Core.Commands.Post(new AddEntityCommand(world.Id, entity.Id));
            //world.AddEntity(blockBuilder.Build());
        }

        private static void AddDoor(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var type = (int)args[2];

            if (type == 'H')
                DoorHelper.AddHorizontalDoor(world, x, y);
            else if (type == 'V')
                DoorHelper.AddVerticalDoor(world, x, y);
        }

        #endregion Private Methods
    }
}