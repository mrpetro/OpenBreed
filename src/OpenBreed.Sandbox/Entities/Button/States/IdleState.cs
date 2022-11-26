using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using System;

namespace OpenBreed.Sandbox.Entities.Button.States
{
    public class IdleState : IState<ButtonState, ButtonImpulse>
    {
        #region Public Fields

        public const string NAME = "Idle";

        #endregion Public Fields

        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public IdleState(IFsmMan fsmMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)ButtonState.Idle;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            entity.SetSpriteOn();

            //entity.PlayAnimation(0, "NotUsedYet");
            //commandsMan.Post(new PlayAnimCommand(entity.Id, "NotUsedYet", 0));

            entity.SetText(0, "Door - Opening");

            triggerMan.OnEntityAnimFinished(entity, OnAnimStopped, singleTime: true);
        }

        public void LeaveState(IEntity entity)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAnimStopped(IEntity entity, AnimFinishedEventArgs e)
        {
            entity.SetState(FsmId, (int)ButtonImpulse.Press);
        }

        #endregion Private Methods
    }
}