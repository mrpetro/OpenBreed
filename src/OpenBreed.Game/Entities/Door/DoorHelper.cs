using OpenBreed.Core;
using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Core.Systems.Animation.Components;
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
            var horizontalDoorOpening = core.Animations.Create<int>("HORIZONTAL_DOOR_OPENING");
            horizontalDoorOpening.AddFrame(0, 2.0f);
            var horizontalDoorClosing = core.Animations.Create<int>("HORIZONTAL_DOOR_CLOSING");
            horizontalDoorClosing.AddFrame(1, 2.0f);

            var animation = new Animator<int>(10.0f, true);
            return animation;
        }

        public static Animator<int> CreateVerticalAnimator(ICore core)
        {
            var verticalDoorOpening = core.Animations.Create<int>("VERTICAL_DOOR_OPENING");
            verticalDoorOpening.AddFrame(0, 2.0f);
            var verticalDoorClosing = core.Animations.Create<int>("VERTICAL_DOOR_CLOSING");
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
    }
}
