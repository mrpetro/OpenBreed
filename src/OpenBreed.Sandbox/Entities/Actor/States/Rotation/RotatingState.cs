
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Animation.Components;
using OpenBreed.Core.Modules.Animation.Events;
using OpenBreed.Core.Modules.Animation.Commands;
using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Core.Commands;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class RotatingState : IState<RotationState, RotationImpulse>
    {
        #region Private Fields

    private readonly string animPrefix;

        #endregion Private Fields

        #region Public Constructors

        public RotatingState(string animPrefix)
        {
            this.animPrefix = animPrefix;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public RotationState Id => RotationState.Rotating;

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            var direction = Entity.GetComponent<DirectionComponent>();
            var movement = Entity.GetComponent<MotionComponent>();
            Entity.GetComponent<ThrustComponent>().Value = direction.Value * movement.Acceleration;

            var animDirName = AnimHelper.ToDirectionName(direction.Value);
            var movementSm = Entity.FsmList.First(item => item.Name == "MovementState");
            Entity.PostCommand(new PlayAnimCommand(Entity.Id, $"{animPrefix}/{movementSm.CurrentStateName}/{animDirName}"));
            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));

            //Entity.Impulse<RotationState, RotationImpulse>(RotationImpulse.Stop);
            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "RotationState", "Stop"));
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {
        }

        #endregion Public Methods

    }
}
