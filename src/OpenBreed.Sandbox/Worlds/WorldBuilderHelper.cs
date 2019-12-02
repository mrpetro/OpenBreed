using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenBreed.Sandbox.Entities.WorldGate;
using OpenTK;
using System;
using System.Collections.Generic;

namespace OpenBreed.Sandbox.Worlds
{
    public class WorldBuilderHelper
    {
        #region Public Fields

        public const int SETUP_WORLD = 10000;

        public const int PLAYER_SPAWN_POINT = 'P';

        #endregion Public Fields

        #region Private Fields

        private WorldBuilder builder;

        private Dictionary<int, Tuple<string, int>> exits = new Dictionary<int, Tuple<string, int>>();

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

        public void RegisterHandlers()
        {
            builder.RegisterCode('O', AddObstacleCell);
            builder.RegisterCode('D', AddDoor);
            builder.RegisterCode('>', AddTeleportEntry);
            builder.RegisterCode('<', AddTeleportExit);
            builder.RegisterCode('}', AddWorldEntry);
            builder.RegisterCode('{', AddWorldExit);

            builder.RegisterCode(' ', AddAirCell);

            builder.RegisterCode(SETUP_WORLD, SetupWorld);
            builder.RegisterCode(PLAYER_SPAWN_POINT, AddPlayer);
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddTeleportEntry(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            TeleportHelper.AddTeleportEntry(world, x, y, pairCode);
            world.Core.MessageBus.Enqueue(null, new TileSetMsg(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddWorldEntry(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            WorldGateHelper.AddWorldEntry(world, x, y, pairCode);
            world.Core.MessageBus.Enqueue(null, new TileSetMsg(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddTeleportExit(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            TeleportHelper.AddTeleportExit(world, x, y, pairCode);
            world.Core.MessageBus.Enqueue(null, new TileSetMsg(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
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
            world.Core.MessageBus.Enqueue(null, new TileSetMsg(world.Id, 0, 12, new Vector2(x * 16, y * 16)));
        }

        private static void AddPlayer(World world, int code, object[] args)
        {
        }

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            thisEntity.RaiseEvent(new CollisionEvent(otherEntity));
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
            world.AddEntity(blockBuilder.Build());
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
            world.AddEntity(blockBuilder.Build());
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

        private static void SetupWorld(World world, int code, object[] args)
        {
            GameWorldHelper.SetupSystems(world.Core, world, (int)args[0], (int)args[1]);
        }

        #endregion Private Methods
    }
}