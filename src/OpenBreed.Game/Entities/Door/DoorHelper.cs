using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Game.Components;
using OpenBreed.Game.Components.States;
using OpenBreed.Game.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Door
{
    public static class DoorHelper
    {
        public static ISpriteAtlas SetupHorizontalDoorSprites(ICore core, ITexture texture)
        {
            return core.Rendering.Sprites.Create(texture.Id, 32, 16, 5, 1, 0, 0);
        }

        public static ISpriteAtlas SetupVerticalDoorSprites(ICore core, ITexture texture)
        {
            return core.Rendering.Sprites.Create(texture.Id, 16, 32, 5, 1, 0, 16);
        }

        public static void CreateHorizontalAnimations(ICore core)
        {
            var horizontalDoorOpening = core.Animations.Anims.Create<int>("HORIZONTAL_DOOR_OPENING");
            horizontalDoorOpening.AddFrame(0, 1.0f);
            horizontalDoorOpening.AddFrame(1, 1.0f);
            horizontalDoorOpening.AddFrame(2, 1.0f);
            horizontalDoorOpening.AddFrame(3, 1.0f);
            horizontalDoorOpening.AddFrame(4, 1.0f);
            var horizontalDoorClosing = core.Animations.Anims.Create<int>("HORIZONTAL_DOOR_CLOSING");
            horizontalDoorClosing.AddFrame(4, 1.0f);
            horizontalDoorClosing.AddFrame(3, 1.0f);
            horizontalDoorClosing.AddFrame(2, 1.0f);
            horizontalDoorClosing.AddFrame(1, 1.0f);
            horizontalDoorClosing.AddFrame(0, 1.0f);
        }

        public static void CreateVerticalAnimations(ICore core)
        {
            var verticalDoorOpening = core.Animations.Anims.Create<int>("VERTICAL_DOOR_OPENING");
            verticalDoorOpening.AddFrame(0, 1.0f);
            verticalDoorOpening.AddFrame(1, 1.0f);
            verticalDoorOpening.AddFrame(2, 1.0f);
            verticalDoorOpening.AddFrame(3, 1.0f);
            verticalDoorOpening.AddFrame(4, 1.0f);
            var verticalDoorClosing = core.Animations.Anims.Create<int>("VERTICAL_DOOR_CLOSING");
            verticalDoorClosing.AddFrame(1, 1.0f);
            verticalDoorClosing.AddFrame(1, 1.0f);
            verticalDoorClosing.AddFrame(2, 1.0f);
            verticalDoorClosing.AddFrame(3, 1.0f);
            verticalDoorClosing.AddFrame(4, 1.0f);
        }

        public static StateMachine CreateHorizontalStateMachine(IEntity entity)
        {
            var stateMachine = entity.AddStateMachine();

            stateMachine.AddState(new OpeningState("Opening", "HORIZONTAL_DOOR_OPENING"));
            stateMachine.AddState(new OpenedState("Opened", 2, 3));
            stateMachine.AddState(new ClosingState("Closing", "HORIZONTAL_DOOR_CLOSING"));
            stateMachine.AddState(new ClosedState("Closed", 0, 1));

            return stateMachine;
        }

        public static StateMachine CreateVerticalStateMachine(IEntity entity)
        {
            var stateMachine = entity.AddStateMachine();

            stateMachine.AddState(new OpeningState("Opening", "VERTICAL_DOOR_OPENING"));
            stateMachine.AddState(new OpenedState("Opened", 5, 9));
            stateMachine.AddState(new ClosingState("Closing", "VERTICAL_DOOR_CLOSING"));
            stateMachine.AddState(new ClosedState("Closed", 4, 8));

            return stateMachine;
        }

        public static void AddVerticalDoor(World world, int x, int y, ISpriteAtlas spriteAtlas, ITileAtlas tileAtlas)
        {
            var core = world.Core;

            var door = core.Entities.Create();

            var doorPart1 = core.Entities.Create();
            doorPart1.Add(GridPosition.Create(x, y));
            doorPart1.Add(new Body(1.0f, 1.0f));
            doorPart1.Add(new AxisAlignedBoxShape(0, 0, 16, 16));
            doorPart1.Add(new GroupPart(door.Id));
            doorPart1.Add(core.Rendering.CreateTile(tileAtlas.Id));

            var doorPart2 = core.Entities.Create();
            doorPart2.Add(GridPosition.Create(x, y + 1));
            doorPart2.Add(new Body(1.0f, 1.0f));
            doorPart2.Add(new AxisAlignedBoxShape(0, 0, 16, 16));
            doorPart2.Add(core.Rendering.CreateTile(tileAtlas.Id));
            doorPart2.Add(new GroupPart(door.Id));


            door.Add(new Animator<int>(5.0f, false));
            door.Add(core.Rendering.CreateSprite(spriteAtlas.Id));
            door.Add(Position.Create(x * 16, y * 16));
            door.Add(new AxisAlignedBoxShape(0, 0, 16, 32));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = DoorHelper.CreateVerticalStateMachine(door);
            doorSm.SetInitialState("Closed");

            world.AddEntity(doorPart1);
            world.AddEntity(doorPart2);
            world.AddEntity(door);
        }

        public static void AddHorizontalDoor(World world, int x, int y, ISpriteAtlas spriteAtlas, ITileAtlas tileAtlas)
        {
            var core = world.Core;

            var door = core.Entities.Create();

            var doorPart1 = core.Entities.Create();
            doorPart1.Add(GridPosition.Create(x, y));
            doorPart1.Add(new Body(1.0f, 1.0f));
            doorPart1.Add(new AxisAlignedBoxShape(0, 0, 16, 16));
            doorPart1.Add(new GroupPart(door.Id));
            doorPart1.Add(core.Rendering.CreateTile(tileAtlas.Id));

            var doorPart2 = core.Entities.Create();
            doorPart2.Add(GridPosition.Create(x + 1, y));
            doorPart2.Add(new Body(1.0f, 1.0f));
            doorPart2.Add(new AxisAlignedBoxShape(0, 0, 16, 16));
            doorPart2.Add(core.Rendering.CreateTile(tileAtlas.Id));
            doorPart2.Add(new GroupPart(door.Id));


            door.Add(new Animator<int>(5.0f, false));
            door.Add(core.Rendering.CreateSprite(spriteAtlas.Id));
            door.Add(Position.Create(x * 16, y * 16));
            door.Add(new AxisAlignedBoxShape(0, 0, 32, 16));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = DoorHelper.CreateHorizontalStateMachine(door);
            doorSm.SetInitialState("Closed");

            world.AddEntity(doorPart1);
            world.AddEntity(doorPart2);
            world.AddEntity(door);
        }
    }
}
