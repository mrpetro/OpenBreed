using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
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
            var horizontalDoorOpening = core.Animations.Create<int>("Animations/DoorHorizontal/Opening", OnFrameUpdate);
            horizontalDoorOpening.AddFrame(0, 1.0f);
            horizontalDoorOpening.AddFrame(1, 1.0f);
            horizontalDoorOpening.AddFrame(2, 1.0f);
            horizontalDoorOpening.AddFrame(3, 1.0f);
            horizontalDoorOpening.AddFrame(4, 1.0f);
            var horizontalDoorClosing = core.Animations.Create<int>("Animations/DoorHorizontal/Closing", OnFrameUpdate);
            horizontalDoorClosing.AddFrame(4, 1.0f);
            horizontalDoorClosing.AddFrame(3, 1.0f);
            horizontalDoorClosing.AddFrame(2, 1.0f);
            horizontalDoorClosing.AddFrame(1, 1.0f);
            horizontalDoorClosing.AddFrame(0, 1.0f);
            var verticalDoorOpening = core.Animations.Create<int>("Animations/DoorVertical/Opening", OnFrameUpdate);
            verticalDoorOpening.AddFrame(0, 1.0f);
            verticalDoorOpening.AddFrame(1, 1.0f);
            verticalDoorOpening.AddFrame(2, 1.0f);
            verticalDoorOpening.AddFrame(3, 1.0f);
            verticalDoorOpening.AddFrame(4, 1.0f);
            var verticalDoorClosing = core.Animations.Create<int>("Animations/DoorVertical/Closing", OnFrameUpdate);
            verticalDoorClosing.AddFrame(1, 1.0f);
            verticalDoorClosing.AddFrame(1, 1.0f);
            verticalDoorClosing.AddFrame(2, 1.0f);
            verticalDoorClosing.AddFrame(3, 1.0f);
            verticalDoorClosing.AddFrame(4, 1.0f);
        }

        private static void OnFrameUpdate(IEntity entity, int nextValue)
        {
            entity.PostCommand(new SpriteSetCommand(entity.Id, nextValue));
        }


        public static void CreateHorizontalFSM(ICore core)
        {
            var fsm = core.StateMachines.Create<FunctioningState, FunctioningImpulse>("Door.Horizontal.Functioning");
            //var stateMachine = entity.AddFsm<FunctioningState, FunctioningImpulse>();

            var openedStamp = core.Rendering.Stamps.GetByName(STAMP_DOOR_HORIZONTAL_OPENED);
            var closedStamp = core.Rendering.Stamps.GetByName(STAMP_DOOR_HORIZONTAL_CLOSED);

            fsm.AddState(new OpeningState());
            fsm.AddState(new OpenedState());
            fsm.AddState(new ClosingState());
            fsm.AddState(new ClosedState());

            fsm.AddTransition(FunctioningState.Closed, FunctioningImpulse.Open, FunctioningState.Opening);
            fsm.AddTransition(FunctioningState.Opening, FunctioningImpulse.StopOpening, FunctioningState.Opened);
            fsm.AddTransition(FunctioningState.Opened, FunctioningImpulse.Close, FunctioningState.Closing);
            fsm.AddTransition(FunctioningState.Closing, FunctioningImpulse.StopClosing, FunctioningState.Closed);
        }

        public static void CreateVerticalFSM(ICore core)
        {
            var stateMachine = core.StateMachines.Create<FunctioningState,FunctioningImpulse>("Door.Vertical.Functioning");

            //var stateMachine = entity.AddFsm<FunctioningState, FunctioningImpulse>();

            stateMachine.AddState(new OpeningState());
            stateMachine.AddState(new OpenedState());
            stateMachine.AddState(new ClosingState());
            stateMachine.AddState(new ClosedState());

            stateMachine.AddTransition(FunctioningState.Closed, FunctioningImpulse.Open, FunctioningState.Opening);
            stateMachine.AddTransition(FunctioningState.Opening, FunctioningImpulse.StopOpening, FunctioningState.Opened);
            stateMachine.AddTransition(FunctioningState.Opened, FunctioningImpulse.Close, FunctioningState.Closing);
            stateMachine.AddTransition(FunctioningState.Closing, FunctioningImpulse.StopClosing, FunctioningState.Closed);


            stateMachine.AddOnLeaveState(FunctioningState.Closed, FunctioningImpulse.Open, OnOpen);
            stateMachine.AddOnEnterState(FunctioningState.Opening, FunctioningImpulse.Open, OnOpeningStarting);
            stateMachine.AddOnLeaveState(FunctioningState.Opening, FunctioningImpulse.StopOpening, OnOpenningEnding);
            stateMachine.AddOnEnterState(FunctioningState.Opened, FunctioningImpulse.StopOpening, OnOpened);
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
            door.Add(new ClassComponent("DoorVertical"));
            door.Add(new FsmComponent());

            door.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            //var fsmComponent = new FsmComponent();
            //fsmComponent.States.Add(new MachineState() {MachineId = world.

            var doorFsm = world.Core.StateMachines.GetByName("Door.Vertical.Functioning");
            doorFsm.SetInitialState(door, (int)FunctioningState.Closed);

            world.AddEntity(door);

            //door.Subscribe<EntityEnteredWorldEventArgs>((s, a) =>
            //{
            //    door.PostCommand(new SetStateCommand(door.Id, doorSm.Id, (int)FunctioningState.Closed));
            //});

            //doorSm.SetInitialState(FunctioningState.Closed);
        }

        public static void AddHorizontalDoor(World world, int x, int y)
        {
            var core = world.Core;

            var door = core.Entities.CreateFromTemplate("DoorHorizontal");
            door.Add(new ClassComponent("DoorHorizontal"));
            door.Add(new FsmComponent());

            door.GetComponent<PositionComponent>().Value = new Vector2(16 * x, 16 * y);

            var doorFsm = world.Core.StateMachines.GetByName("Door.Horizontal.Functioning");
            doorFsm.SetInitialState(door, (int)FunctioningState.Closed);

            //door.Subscribe<EntityEnteredWorldEventArgs>((s, a) =>
            //{
            //    door.PostCommand(new SetStateCommand(door.Id, doorSm.Id, FunctioningState.Closed));
            //});

            world.AddEntity(door);


            //doorSm.SetInitialState(FunctioningState.Closed);
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
