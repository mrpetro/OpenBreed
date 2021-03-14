using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Commands;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Rendering.Commands;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class IdleState : IState<ButtonState, ButtonImpulse>
    {
        #region Public Fields

        public const string NAME = "Idle";

        #endregion Public Fields

        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;

        #endregion Private Fields

        #region Public Constructors

        public IdleState(IFsmMan fsmMan, ICommandsMan commandsMan)
        {
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)ButtonState.Idle;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            commandsMan.Post(new SpriteOnCommand(entity.Id));
            commandsMan.Post(new PlayAnimCommand(entity.Id, "NotUsedYet", 0));
            commandsMan.Post(new TextSetCommand(entity.Id, 0, "Door - Opening"));

            entity.Subscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<AnimStoppedEventArgs>(OnAnimStopped);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(object sender, AnimStoppedEventArgs eventArgs)
        {
            var entity = sender as Entity;
            commandsMan.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)ButtonImpulse.Press));
        }

        #endregion Private Methods
    }
}