using OpenBreed.Core;
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

            world.AddEntity(button);
        }

        public static void CreateFSM(ICore core)
        {
            var buttonFsm = core.StateMachines.Create<ButtonState, ButtonImpulse>("Button");

            buttonFsm.AddState(new IdleState());
            buttonFsm.AddState(new PressedState());

            buttonFsm.AddTransition(ButtonState.Pressed, ButtonImpulse.Unpress, ButtonState.Idle);
            buttonFsm.AddTransition(ButtonState.Idle, ButtonImpulse.Press, ButtonState.Pressed);
        }
    }
}
