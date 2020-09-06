using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Components;
using OpenBreed.Sandbox.Components.States;
using OpenBreed.Sandbox.Entities.Door.States;
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
        private const string STAMP_DOOR_HORIZONTAL_CLOSED = "Tiles/Stamps/DoorHorizontal/Closed";
        private const string STAMP_DOOR_HORIZONTAL_OPENED = "Tiles/Stamps/DoorHorizontal/Opened";
        private const string STAMP_DOOR_VERTICAL_CLOSED = "Tiles/Stamps/DoorVertical/Closed";
        private const string STAMP_DOOR_VERTICAL_OPENED = "Tiles/Stamps/DoorVertical/Opened";

        public static void CreateAnimations(ICore core)
        {
            var horizontalDoorOpening = core.Animations.Create("Animations/DoorHorizontal/Opening", 5.0f);
            var hdo = horizontalDoorOpening.AddPart<int>(OnFrameUpdate, 0);
            hdo.AddFrame(0, 1.0f);
            hdo.AddFrame(1, 2.0f);
            hdo.AddFrame(2, 3.0f);
            hdo.AddFrame(3, 4.0f);
            hdo.AddFrame(4, 5.0f);
            var horizontalDoorClosing = core.Animations.Create("Animations/DoorHorizontal/Closing", 5.0f);
            var hdc = horizontalDoorClosing.AddPart<int>(OnFrameUpdate, 4);
            hdc.AddFrame(4, 1.0f);
            hdc.AddFrame(3, 2.0f);
            hdc.AddFrame(2, 3.0f);
            hdc.AddFrame(1, 4.0f);
            hdc.AddFrame(0, 5.0f);
            var verticalDoorOpening = core.Animations.Create("Animations/DoorVertical/Opening", 5.0f);
            var vdo = verticalDoorOpening.AddPart<int>(OnFrameUpdate, 0);
            vdo.AddFrame(0, 1.0f);
            vdo.AddFrame(1, 2.0f);
            vdo.AddFrame(2, 3.0f);
            vdo.AddFrame(3, 4.0f);
            vdo.AddFrame(4, 5.0f);
            var verticalDoorClosing = core.Animations.Create("Animations/DoorVertical/Closing", 5.0f);
            var vdc = verticalDoorClosing.AddPart<int>(OnFrameUpdate, 4);
            vdc.AddFrame(4, 1.0f);
            vdc.AddFrame(3, 2.0f);
            vdc.AddFrame(2, 3.0f);
            vdc.AddFrame(1, 4.0f);
            vdc.AddFrame(0, 5.0f);
        }

        private static void OnFrameUpdate(Entity entity, int nextValue)
        {
            entity.Core.Commands.Post(new SpriteSetCommand(entity.Id, nextValue));
        }


        public static void CreateFsm(ICore core)
        {
            var fsm = core.StateMachines.Create<FunctioningState, FunctioningImpulse>("Door.Functioning");

            fsm.AddState(new OpeningState());
            fsm.AddState(new OpenedAwaitClose());
            fsm.AddState(new ClosingState());
            fsm.AddState(new ClosedState(core));

            fsm.AddTransition(FunctioningState.Closed, FunctioningImpulse.Open, FunctioningState.Opening);
            fsm.AddTransition(FunctioningState.Opening, FunctioningImpulse.StopOpening, FunctioningState.Opened);
            fsm.AddTransition(FunctioningState.Opened, FunctioningImpulse.Close, FunctioningState.Closing);
            fsm.AddTransition(FunctioningState.Closing, FunctioningImpulse.StopClosing, FunctioningState.Closed);
        }

        private static void OnOpenningEnding()
        {
            Console.WriteLine("Door -> OpenningEnding");
        }

        private static void OnOpeningStarting()
        {
            Console.WriteLine("Door -> OpenningStarting");
        }

        private static void OnOpen()
        {
            Console.WriteLine("Door -> Open");
        }

        private static void OnOpened()
        {
            Console.WriteLine("Door -> Opened");
        }

        public static void AddVerticalDoor(World world, int x, int y)
        {
            var core = world.Core;

            //var door = core.Entities.Create();
            var door = core.Entities.CreateFromTemplate("DoorVertical");
            door.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            door.Add(new CollisionComponent());

            world.Core.Commands.Post(new AddEntityCommand(world.Id, door.Id));
        }

        public static void AddHorizontalDoor(World world, int x, int y)
        {
            var core = world.Core;

            var door = core.Entities.CreateFromTemplate("DoorHorizontal");
            door.Get<PositionComponent>().Value = new Vector2(16 * x, 16 * y);
            door.Add(new CollisionComponent());

            world.Core.Commands.Post(new AddEntityCommand(world.Id, door.Id));
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
