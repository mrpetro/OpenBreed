using OpenBreed.Core;
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
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Components.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Door
{
    public static class DoorHelper
    {
        private const string TILE_ATLAS = "Atlases/Tiles/16/Test";
        private const string STAMP_DOOR_HORIZONTAL_CLOSED = "Tiles/Stamps/Door/Horizontal/Closed";
        private const string STAMP_DOOR_HORIZONTAL_OPENED = "Tiles/Stamps/Door/Horizontal/Opened";
        private const string STAMP_DOOR_VERTICAL_CLOSED = "Tiles/Stamps/Door/Vertical/Closed";
        private const string STAMP_DOOR_VERTICAL_OPENED = "Tiles/Stamps/Door/Vertical/Opened";

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

            var openedStamp = entity.Core.Rendering.Stamps.GetByName(STAMP_DOOR_HORIZONTAL_OPENED);
            var closedStamp = entity.Core.Rendering.Stamps.GetByName(STAMP_DOOR_HORIZONTAL_CLOSED);

            stateMachine.AddState(new OpeningState("Opening", "Animations/Door/Horizontal/Opening"));
            stateMachine.AddState(new OpenedState("Opened", openedStamp.Id));
            stateMachine.AddState(new ClosingState("Closing", "Animations/Door/Horizontal/Closing"));
            stateMachine.AddState(new ClosedState("Closed", closedStamp.Id));

            return stateMachine;
        }

        public static StateMachine CreateVerticalFSM(IEntity entity)
        {
            var stateMachine = entity.AddFSM("Functioning");

            var openedStamp = entity.Core.Rendering.Stamps.GetByName(STAMP_DOOR_VERTICAL_OPENED);
            var closedStamp = entity.Core.Rendering.Stamps.GetByName(STAMP_DOOR_VERTICAL_CLOSED);

            stateMachine.AddState(new OpeningState("Opening", "Animations/Door/Vertical/Opening"));
            stateMachine.AddState(new OpenedState("Opened", openedStamp.Id));
            stateMachine.AddState(new ClosingState("Closing", "Animations/Door/Vertical/Closing"));
            stateMachine.AddState(new ClosedState("Closed", closedStamp.Id));

            return stateMachine;
        }

        public static void AddVerticalDoor(World world, int x, int y)
        {
            var core = world.Core;

            //var door = core.Entities.Create();
            var door = core.Entities.CreateFromTemplate("DoorVertical");

            door.Add(AxisAlignedBoxShape.Create(0, 0, 16, 32));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            door.Components.OfType<Position>().First().Value = new Vector2(16 * x, 16 * y);

            var doorSm = DoorHelper.CreateVerticalFSM(door);
            doorSm.SetInitialState("Closed");

            world.AddEntity(door);
        }

        public static void AddHorizontalDoor(World world, int x, int y)
        {
            var core = world.Core;

            var door = core.Entities.CreateFromTemplate("DoorHorizontal");

            door.Add(AxisAlignedBoxShape.Create(0, 0, 32, 16));
            door.Add(TextHelper.Create(core, new Vector2(-10, 10), "Door"));

            door.Components.OfType<Position>().First().Value = new Vector2(16 * x, 16 * y);

            var doorSm = DoorHelper.CreateHorizontalFSM(door);
            doorSm.SetInitialState("Closed");

            world.AddEntity(door);
        }

        public static void CreateStamps(ICore core)
        {
            var stampBuilder = core.Rendering.Stamps.Create();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_HORIZONTAL_CLOSED);
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 0);
            stampBuilder.AddTile(1, 0, 1);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_HORIZONTAL_OPENED);
            stampBuilder.SetSize(2, 1);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 12);
            stampBuilder.AddTile(1, 0, 12);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_VERTICAL_CLOSED);
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 4);
            stampBuilder.AddTile(0, 1, 8);
            stampBuilder.Build();

            stampBuilder.ClearTiles();
            stampBuilder.SetName(STAMP_DOOR_VERTICAL_OPENED);
            stampBuilder.SetSize(1, 2);
            stampBuilder.SetOrigin(0, 0);
            stampBuilder.AddTile(0, 0, 12);
            stampBuilder.AddTile(0, 1, 12);
            stampBuilder.Build();

        }
    }
}
