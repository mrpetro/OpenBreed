using OpenBreed.Core;
using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Game.Components;
using OpenBreed.Game.Components.States;
using OpenBreed.Game.Helpers;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Door
{
    public static class DoorHelper
    {
        private const string TILE_ATLAS = "Atlases/Tiles/16/Test";

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

        public static StateMachine CreateHorizontalFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Functioning");

            stateMachine.AddState(new OpeningState("Opening", "HORIZONTAL_DOOR_OPENING"));
            stateMachine.AddState(new OpenedState("Opened", 2, 3));
            stateMachine.AddState(new ClosingState("Closing", "HORIZONTAL_DOOR_CLOSING"));
            stateMachine.AddState(new ClosedState("Closed", 0, 1));

            return stateMachine;
        }

        public static StateMachine CreateVerticalFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Functioning");

            stateMachine.AddState(new OpeningState("Opening", "VERTICAL_DOOR_OPENING"));
            stateMachine.AddState(new OpenedState("Opened", 5, 9));
            stateMachine.AddState(new ClosingState("Closing", "VERTICAL_DOOR_CLOSING"));
            stateMachine.AddState(new ClosedState("Closed", 4, 8));

            return stateMachine;
        }

        public static void AddVerticalDoor(ICore core, World world, int x, int y)
        {



            var door = core.Entities.Create();

            var doorPart1 = core.Entities.Create();
            doorPart1.Add(Position.Create(x * 16, y * 16));
            doorPart1.Add(GroupPart.Create(door.Id));
            doorPart1.Add(core.Rendering.CreateTile(TILE_ATLAS));

            var doorPart2 = core.Entities.Create();
            doorPart2.Add(Position.Create(x * 16, y * 16 + 16));
            doorPart2.Add(core.Rendering.CreateTile(TILE_ATLAS));
            doorPart2.Add(GroupPart.Create(door.Id));

            door.Add(new Animator<int>(5.0f, false));
            door.Add(Body.Create(1.0f, 1.0f));
            door.Add(core.Rendering.CreateSprite("Atlases/Sprites/Door/Vertical"));
            door.Add(Position.Create(x * 16, y * 16));
            door.Add(AxisAlignedBoxShape.Create(0, 0, 16, 32));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = DoorHelper.CreateVerticalFSM(door);
            doorSm.SetInitialState("Closed");

            world.AddEntity(doorPart1);
            world.AddEntity(doorPart2);
            world.AddEntity(door);
        }

        public static void AddHorizontalDoor(ICore core, World world, int x, int y)
        {
            //var doorBlueprint = core.Blueprints.GetByName("HorizontalDoor");

            var states = new Dictionary<string, IComponentState>();


            //var doorAlt = core.Entities.CreateFromBlueprint(doorBlueprint, states);


            var door = core.Entities.Create();

            var doorPart1 = core.Entities.Create();
            doorPart1.Add(Position.Create(x * 16, y * 16));
            doorPart1.Add(GroupPart.Create(door.Id));
            doorPart1.Add(core.Rendering.CreateTile(TILE_ATLAS));

            var doorPart2 = core.Entities.Create();
            doorPart2.Add(Position.Create(x * 16 + 16, y * 16));
            doorPart2.Add(core.Rendering.CreateTile(TILE_ATLAS));
            doorPart2.Add(GroupPart.Create(door.Id));


            door.Add(new Animator<int>(5.0f, false));
            door.Add(Body.Create(1.0f, 1.0f));
            door.Add(core.Rendering.CreateSprite("Atlases/Sprites/Door/Horizontal"));
            door.Add(Position.Create(x * 16, y * 16));
            door.Add(AxisAlignedBoxShape.Create(0, 0, 32, 16));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = DoorHelper.CreateHorizontalFSM(door);
            doorSm.SetInitialState("Closed");

            world.AddEntity(doorPart1);
            world.AddEntity(doorPart2);
            world.AddEntity(door);
        }
    }
}
