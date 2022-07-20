
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Systems.Control.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;
using OpenTK.Mathematics;
using OpenBreed.Wecs.Extensions;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class StandingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private const string ANIM_PREFIX = "Vanilla/Common";
        private readonly IFsmMan fsmMan;
        private readonly IClipMan<Entity> clipMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public StandingState(IFsmMan fsmMan, IClipMan<Entity> clipMan, ITriggerMan triggerMan)
        {
            this.fsmMan = fsmMan;
            this.clipMan = clipMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)MovementState.Standing;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var direction = entity.Get<AngularPositionComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);
            var className = entity.Get<MetadataComponent>().Name;
            var thrust = entity.Get<ThrustComponent>();

            thrust.Value = Vector2.Zero;

            var stateName = fsmMan.GetStateName(FsmId, Id);
            var clipId = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{stateName}/{animDirName}").Id;
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.PlayAnimation(0, clipId);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            //triggerMan.ForEntity(entity).OnControlDirectionChanged().RunFunction;
            //triggerMan.OnEntity(entity).OnControlDirectionChanged().RunAction(OnControlDirectionChanged, singleTime: true);


            triggerMan.OnEntityControlDirectionChanged(entity, OnControlDirectionChanged, singleTime: true);
        }

        public void LeaveState(Entity entity)
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlDirectionChanged(Entity entity, ControlDirectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Direction != Vector2.Zero)
            {
                entity.SetState(FsmId, (int)MovementImpulse.Walk);

                var angularVelocity = entity.Get<AngularVelocityComponent>();
                angularVelocity.Value = new Vector2(eventArgs.Direction.X, eventArgs.Direction.Y);
            }
        }

        #endregion Private Methods
    }
}