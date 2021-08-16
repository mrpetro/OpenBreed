using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class IdleState : IState<RotationState, RotationImpulse>
    {
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

        public int Id => (int)RotationState.Idle;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            var currentStateNames = fsmMan.GetStateNames(entity);
            commandsMan.Post(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            //entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        public void LeaveState(Entity entity)
        {
            //entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs e)
        {
            var entity = sender as Entity;

            if (e.Direction != Vector2.Zero)
            {
                var angularPos = entity.Get<AngularPositionComponent>();

                if (angularPos.Value != e.Direction)
                {
                    //var aPos3 = new Vector3(angularPos.GetDirection());
                    //var dPos3 = new Vector3(e.Direction);
                    //var newVec = Vector3Extension.RotateTowards(aPos3, dPos3, 0.4f, 1.0f);
                    var angularThrust = entity.Get<AngularVelocityComponent>();
                    angularThrust.Value = new Vector2(e.Direction.X, e.Direction.Y);
                    //dir.SetDirection(e.Direction);
                    commandsMan.Post(new SetEntityStateCommand(entity.Id, FsmId, (int)RotationImpulse.Rotate));
                }
            }
        }

        #endregion Private Methods
    }
}