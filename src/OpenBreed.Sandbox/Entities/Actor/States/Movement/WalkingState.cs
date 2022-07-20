using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;
using OpenTK.Mathematics;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class WalkingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private const string ANIM_PREFIX = "Vanilla/Common";
        private readonly IFsmMan fsmMan;
        private readonly IClipMan<Entity> clipMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState(IFsmMan fsmMan, IClipMan<Entity> clipMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.clipMan = clipMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)MovementState.Walking;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            var control = entity.Get<WalkingControlComponent>();
            entity.Get<ThrustComponent>().Value = control.Direction * movement.Acceleration;

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var className = entity.Get<MetadataComponent>().Name;
            var clip = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{stateName}/{animDirPostfix}");
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.PlayAnimation(0, clip.Id);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            triggerMan.OnEntityControlDirectionChanged(entity, OnControlDirectionChanged, singleTime: true);
            triggerMan.OnEntityDirectionChanged(entity, OnDirectionChanged, singleTime: true);
        }

        public void LeaveState(Entity entity)
        {

        }

        #endregion Public Methods

        #region Private Methods

        private void OnDirectionChanged(Entity entity, DirectionChangedEventArgs args)
        {
            var direction = entity.Get<AngularPositionComponent>();
            var movement = entity.Get<MotionComponent>();
            var animDirName = AnimHelper.ToDirectionName(direction.Value);
            var className = entity.Get<MetadataComponent>().Name;
            var movementFsm = fsmMan.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);

            var clip = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{movementStateName}/{animDirName}");

            entity.PlayAnimation(0, clip.Id);
        }

        private void OnControlDirectionChanged(Entity entity, ControlDirectionChangedEventArgs e)
        {
            if (e.Direction != Vector2.Zero)
            {
                entity.SetState(FsmId, (int)MovementImpulse.Walk);

                var angularVelocity = entity.Get<AngularVelocityComponent>();
                angularVelocity.Value = new Vector2(e.Direction.X, e.Direction.Y);

                triggerMan.OnEntityControlDirectionChanged(entity, OnControlDirectionChanged, singleTime: true);
                triggerMan.OnEntityDirectionChanged(entity, OnDirectionChanged, singleTime: true);
            }
            else
            {
                entity.SetState(FsmId, (int)MovementImpulse.Stop);

                triggerMan.OnEntityControlDirectionChanged(entity, OnControlDirectionChanged, singleTime: true);
                triggerMan.OnEntityDirectionChanged(entity, OnDirectionChanged, singleTime: true);
            }
        }

        #endregion Private Methods
    }
}