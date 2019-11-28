using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Sandbox.Entities.Builders;
using OpenBreed.Sandbox.Entities.Door;
using OpenBreed.Sandbox.Entities.Teleport;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Worlds
{
    public static class WorldBuilderHelper
    {
        public const int SETUP_WORLD = 10000;
        public const int PLAYER_SPAWN_POINT = 'P';

        public static void RegisterHandlers(WorldBuilder builder)
        {
            builder.RegisterCode('O', AddObstacleCell);
            builder.RegisterCode('D', AddDoor);
            builder.RegisterCode('>', AddTeleportEntry);
            builder.RegisterCode('<', AddTeleportExit);

            builder.RegisterCode(' ', AddAirCell);

            builder.RegisterCode(SETUP_WORLD, SetupWorld);
            builder.RegisterCode(PLAYER_SPAWN_POINT, AddPlayer);
        }

        private static void AddTeleportEntry(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];


            TeleportHelper.AddTeleportEntry(world, x, y , pairCode);

            world.Core.MessageBus.Enqueue(null, new TileSetMsg(world.Id, 12, new Vector2(x * 16, y * 16)));
            //blockBuilder.SetTileId(ToTileId(gfxCode));
        }

        private static void AddTeleportExit(World world, int code, object[] args)
        {
            var core = world.Core;
            var x = (int)args[0];
            var y = (int)args[1];
            var pairCode = (int)args[2];

            TeleportHelper.AddTeleportExit(world, x, y, pairCode);
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

            if(type == 'H')
            DoorHelper.AddHorizontalDoor(world, x, y);
            else if (type == 'V')
                    DoorHelper.AddVerticalDoor(world, x, y);
        }

        private static void SetupWorld(World world, int code, object[] args)
        {
            GameWorldHelper.SetupSystems(world.Core, world, (int)args[0], (int)args[1]);
        }
    }
}
