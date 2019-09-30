using OpenBreed.Core;
using OpenBreed.Core.Blueprints;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
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

        public static void CreateAnimations(ICore core)
        {
            var horizontalDoorOpening = core.Animations.Anims.Create<int>("Animations/Door/Horizontal/Opening");
            horizontalDoorOpening.AddFrame(0, 1.0f);
            horizontalDoorOpening.AddFrame(1, 1.0f);
            horizontalDoorOpening.AddFrame(2, 1.0f);
            horizontalDoorOpening.AddFrame(3, 1.0f);
            horizontalDoorOpening.AddFrame(4, 1.0f);
            var horizontalDoorClosing = core.Animations.Anims.Create<int>("Animations/Door/Horizontal/Closing");
            horizontalDoorClosing.AddFrame(4, 1.0f);
            horizontalDoorClosing.AddFrame(3, 1.0f);
            horizontalDoorClosing.AddFrame(2, 1.0f);
            horizontalDoorClosing.AddFrame(1, 1.0f);
            horizontalDoorClosing.AddFrame(0, 1.0f);
            var verticalDoorOpening = core.Animations.Anims.Create<int>("Animations/Door/Vertical/Opening");
            verticalDoorOpening.AddFrame(0, 1.0f);
            verticalDoorOpening.AddFrame(1, 1.0f);
            verticalDoorOpening.AddFrame(2, 1.0f);
            verticalDoorOpening.AddFrame(3, 1.0f);
            verticalDoorOpening.AddFrame(4, 1.0f);
            var verticalDoorClosing = core.Animations.Anims.Create<int>("Animations/Door/Vertical/Closing");
            verticalDoorClosing.AddFrame(1, 1.0f);
            verticalDoorClosing.AddFrame(1, 1.0f);
            verticalDoorClosing.AddFrame(2, 1.0f);
            verticalDoorClosing.AddFrame(3, 1.0f);
            verticalDoorClosing.AddFrame(4, 1.0f);
        }

        public static StateMachine CreateHorizontalFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Functioning");

            var openedStamp = entity.Core.Rendering.Stamps.GetByName("Tiles/Stamps/Door/Horizontal/Opened");
            var closedStamp = entity.Core.Rendering.Stamps.GetByName("Tiles/Stamps/Door/Horizontal/Closed");

            stateMachine.AddState(new OpeningState("Opening", "Animations/Door/Horizontal/Opening"));
            stateMachine.AddState(new OpenedState("Opened", openedStamp.Id));
            stateMachine.AddState(new ClosingState("Closing", "Animations/Door/Horizontal/Closing"));
            stateMachine.AddState(new ClosedState("Closed", closedStamp.Id));

            return stateMachine;
        }

        public static StateMachine CreateVerticalFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Functioning");

            var openedStamp = entity.Core.Rendering.Stamps.GetByName("Tiles/Stamps/Door/Vertical/Opened");
            var closedStamp = entity.Core.Rendering.Stamps.GetByName("Tiles/Stamps/Door/Vertical/Closed");

            stateMachine.AddState(new OpeningState("Opening", "Animations/Door/Vertical/Opening"));
            stateMachine.AddState(new OpenedState("Opened", openedStamp.Id));
            stateMachine.AddState(new ClosingState("Closing", "Animations/Door/Vertical/Closing"));
            stateMachine.AddState(new ClosedState("Closed", closedStamp.Id));

            return stateMachine;
        }

        public static void AddVerticalDoor(ICore core, World world, int x, int y)
        {
            var door = core.Entities.Create();

            door.Add(new Animator<int>(5.0f, false));
            door.Add(Body.Create(1.0f, 1.0f, "Static", (e, c) => OnCollision(door, e, c)));
            door.Add(core.Rendering.CreateSprite("Atlases/Sprites/Door/Vertical"));
            door.Add(Position.Create(x * 16, y * 16));
            door.Add(AxisAlignedBoxShape.Create(0, 0, 16, 32));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = DoorHelper.CreateVerticalFSM(door);
            doorSm.SetInitialState("Closed");

            //world.AddEntity(doorPart1);
            //world.AddEntity(doorPart2);
            world.AddEntity(door);
        }

        private static void OnCollision(IEntity thisEntity, IEntity otherEntity, Vector2 projection)
        {
            thisEntity.RaiseEvent(new CollisionEvent(otherEntity));
        }

        public static void AddHorizontalDoor(World world, int x, int y)
        {
            var core = world.Core;

            //var doorBlueprint = core.Blueprints.GetByName("HorizontalDoor");

            var states = new Dictionary<string, IComponentState>();


            //var doorAlt = core.Entities.CreateFromBlueprint(doorBlueprint, states);


            var door = core.Entities.Create();

            door.Add(new Animator<int>(5.0f, false));
            door.Add(Body.Create(1.0f, 1.0f, "Static", (e, c) => OnCollision(door, e, c)));
            door.Add(core.Rendering.CreateSprite("Atlases/Sprites/Door/Horizontal"));
            door.Add(Position.Create(x * 16, y * 16));
            door.Add(AxisAlignedBoxShape.Create(0, 0, 32, 16));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            var doorSm = DoorHelper.CreateHorizontalFSM(door);
            doorSm.SetInitialState("Closed");

            //world.AddEntity(doorPart1);
            //world.AddEntity(doorPart2);
            world.AddEntity(door);
        }

        public static void CreateStamps(ICore core)
        {
            var stampBuilder = core.Rendering.Stamps.Create();

            stampBuilder.ClearTiles();
            stampBuilder.SetName("Tiles/Stamps/Door/Horizontal/Closed");
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 0);
            stampBuilder.AddTile(1, 0, 1);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName("Tiles/Stamps/Door/Horizontal/Opened");
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 2);
            stampBuilder.AddTile(1, 0, 3);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName("Tiles/Stamps/Door/Vertical/Closed");
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 4);
            stampBuilder.AddTile(0, 1, 8);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName("Tiles/Stamps/Door/Vertical/Opened");
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 5);
            stampBuilder.AddTile(0, 1, 9);
            stampBuilder.Build();

        }
    }
}
