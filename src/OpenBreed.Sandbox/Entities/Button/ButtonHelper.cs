using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Sandbox.Entities.Button.States;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Sandbox.Entities.Button
{
    public class ButtonHelper
    {
        #region Public Methods

        public static void AddButton(World world, int x, int y)
        {
            //var button = core.GetManager<IEntityMan>().CreateFromTemplate("Button");

            //button.Get<PositionComponent>().Value = new Vector2(0, 0);

            //world.Core.Commands.Post(new AddEntityCommand(world.Id, button.Id));
            //world.AddEntity(button);
        }

        public static void CreateFsm(ICore core)
        {
            var fsmMan = core.GetManager<IFsmMan>();
            var commandsMan = core.GetManager<ICommandsMan>();

            var buttonFsm = fsmMan.Create<ButtonState, ButtonImpulse>("Button");

            buttonFsm.AddState(new IdleState(fsmMan, commandsMan));
            buttonFsm.AddState(new PressedState(fsmMan, commandsMan));

            buttonFsm.AddTransition(ButtonState.Pressed, ButtonImpulse.Unpress, ButtonState.Idle);
            buttonFsm.AddTransition(ButtonState.Idle, ButtonImpulse.Press, ButtonState.Pressed);
        }

        #endregion Public Methods
    }
}