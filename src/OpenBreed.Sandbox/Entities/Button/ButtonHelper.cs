using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities.Button.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Button
{
    public class ButtonHelper
    {
        public static void AddButton(World world, int x, int y)
        {
            var core = world.Core;

            var button = core.Entities.CreateFromTemplate("Button");

            button.GetComponent<PositionComponent>().Value = new Vector2(0, 0);

            var buttonSm = CreateFSM(button);
            buttonSm.SetInitialState(ButtonState.Idle);

            world.AddEntity(button);
        }

        public static StateMachine<ButtonState, ButtonImpulse> CreateFSM(IEntity entity)
        {
            var stateMachine = entity.AddFsm<ButtonState, ButtonImpulse>();

            stateMachine.AddState(new IdleState());
            stateMachine.AddState(new PressedState());

            stateMachine.AddTransition(ButtonState.Pressed, ButtonImpulse.Unpress, ButtonState.Idle);
            stateMachine.AddTransition(ButtonState.Idle, ButtonImpulse.Press, ButtonState.Pressed);

            return stateMachine;
        }
    }
}
