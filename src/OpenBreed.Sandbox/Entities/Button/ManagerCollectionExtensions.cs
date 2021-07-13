using OpenBreed.Common;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Button.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Entities.Button
{
    public static class ManagerCollectionExtensions
    {
        public static void SetupButtonStates(this IManagerCollection managerCollection)
        {
            var fsmMan = managerCollection.GetManager<IFsmMan>();
            var commandsMan = managerCollection.GetManager<ICommandsMan>();

            var buttonFsm = fsmMan.Create<ButtonState, ButtonImpulse>("Button");

            buttonFsm.AddState(new IdleState(fsmMan, commandsMan));
            buttonFsm.AddState(new PressedState(fsmMan, commandsMan));

            buttonFsm.AddTransition(ButtonState.Pressed, ButtonImpulse.Unpress, ButtonState.Idle);
            buttonFsm.AddTransition(ButtonState.Idle, ButtonImpulse.Press, ButtonState.Pressed);
        }
    }
}
