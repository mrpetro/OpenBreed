using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class IdleState : IState<RotationState, RotationImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public IdleState(
            IFsmMan fsmMan,
            ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)RotationState.Idle;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            Console.WriteLine("RotIdle -> Enter");

            var currentStateNames = fsmMan.GetStateNames(entity);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            triggerMan.OnEntityDirectionChanged(entity, OnDirectionChanged);
        }

        private bool OnDirectionChanged(IEntity entity, DirectionChangedEventArgs args)
        {
            var angularVelocity = entity.Get<AngularPositionTargetComponent>();
            var angularPosition = entity.Get<AngularPositionComponent>();

            if (angularVelocity.Value == angularPosition.Value)
                return false;

            entity.SetState(FsmId, (int)RotationImpulse.Rotate);
            return true;
        }

        public void LeaveState(IEntity entity)
        {
            Console.WriteLine("RotIdle -> Leave");

        }

        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}