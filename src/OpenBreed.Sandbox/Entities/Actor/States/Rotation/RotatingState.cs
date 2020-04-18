using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class RotatingState : IState<RotationState, RotationImpulse>
    {
        #region Private Fields

        private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public RotatingState()
        {
            this.animPrefix = "Animations";
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)RotationState.Rotating;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(IEntity entity)
        {
            var direction = entity.GetComponent<DirectionComponent>();
            var movement = entity.GetComponent<MotionComponent>();
            entity.GetComponent<ThrustComponent>().Value = direction.Value * movement.Acceleration;

            var animDirName = AnimHelper.ToDirectionName(direction.Value);
            var className = entity.GetComponent<ClassComponent>().Name;
            var movementFsm = entity.Core.StateMachines.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);
            entity.PostCommand(new PlayAnimCommand(entity.Id, $"{animPrefix}/{className}/{movementStateName}/{animDirName}", 0));

            var currentStateNames = entity.Core.StateMachines.GetStateNames(entity);
            entity.PostCommand(new TextSetCommand(entity.Id, 0, String.Join(", ", currentStateNames.ToArray())));

            entity.PostCommand(new SetStateCommand(entity.Id, FsmId, (int)RotationImpulse.Stop));
        }

        public void LeaveState(IEntity entity)
        {
        }

        #endregion Public Methods
    }
}