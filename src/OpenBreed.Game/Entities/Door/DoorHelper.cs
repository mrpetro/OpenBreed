using OpenBreed.Core;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.States;
using OpenBreed.Game.Components;
using OpenBreed.Game.Components.States;
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

        public static Animator<int> CreateHorizontalAnimator(ICore core)
        {
            var horizontalDoorOpening = core.Animations.Anims.Create<int>("HORIZONTAL_DOOR_OPENING");
            horizontalDoorOpening.AddFrame(0, 2.0f);
            var horizontalDoorClosing = core.Animations.Anims.Create<int>("HORIZONTAL_DOOR_CLOSING");
            horizontalDoorClosing.AddFrame(1, 2.0f);

            var animation = new Animator<int>(10.0f, true);
            return animation;
        }

        public static Animator<int> CreateVerticalAnimator(ICore core)
        {
            var verticalDoorOpening = core.Animations.Anims.Create<int>("VERTICAL_DOOR_OPENING");
            verticalDoorOpening.AddFrame(0, 2.0f);
            var verticalDoorClosing = core.Animations.Anims.Create<int>("VERTICAL_DOOR_CLOSING");
            verticalDoorClosing.AddFrame(1, 2.0f);

            var animation = new Animator<int>(10.0f, true);
            return animation;
        }

        public static StateMachine CreateHorizontalStateMachine(IEntity entity)
        {
            var stateMachine = new StateMachine(entity);

            stateMachine.AddState(new OpeningState("Opening", "HORIZONTAL_DOOR_OPENING"));
            stateMachine.AddState(new OpenedState("Opened", 0));
            stateMachine.AddState(new ClosingState("Closing", "HORIZONTAL_DOOR_CLOSING"));
            stateMachine.AddState(new ClosedState("Closed", 0));

            return stateMachine;
        }

        public static StateMachine CreateVerticalStateMachine(IEntity entity)
        {
            var stateMachine = new StateMachine(entity);

            stateMachine.AddState(new OpeningState("Opening", "HORIZONTAL_DOOR_OPENING"));
            stateMachine.AddState(new OpenedState("Opened", 0));
            stateMachine.AddState(new ClosingState("Closing", "HORIZONTAL_DOOR_CLOSING"));
            stateMachine.AddState(new ClosedState("Closed", 0));

            return stateMachine;
        }



        public static List<IEntity> CreateHorizontalDoor(ICore core, int x, int y, ISpriteAtlas spriteAtlas, ITileAtlas tileAtlas)
        {
            var collection = new List<IEntity>();

            var door = core.Entities.Create();

            var doorPart1 = core.Entities.Create();
            doorPart1.Add(new Position(x * 16, y * 16));
            doorPart1.Add(new GridBoxBody(16));
            doorPart1.Add(new GroupPart(door.Id));
            doorPart1.Add(core.Rendering.CreateTile(tileAtlas.Id, 2));

            var doorPart2 = core.Entities.Create();
            doorPart2.Add(new Position((x + 1) * 16, y * 16));
            doorPart2.Add(new GridBoxBody(16));
            doorPart2.Add(core.Rendering.CreateTile(tileAtlas.Id, 2));
            doorPart2.Add(new GroupPart(door.Id));

            //door.Add(new Animator<int>(10.0f, true));
            //door.Add(core.Rendering.CreateSprite(atlas.Id));
            //door.Add(new Position(pos));
            //door.Add(new AxisAlignedBoxShape(32, 32));
            //door.Add(new StaticBody());

            var doorSm = DoorHelper.CreateHorizontalStateMachine(door);
            //doorSm.Initialize("Closed");

            collection.Add(door);
            collection.Add(doorPart1);
            collection.Add(doorPart2);

            return collection;
        }
    }
}
