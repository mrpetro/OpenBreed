
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Control.Events;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public interface ITriggerMan
    {
    }

    public class StandingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private const string ANIM_PREFIX = "Vanilla/Common";
        private readonly IFsmMan fsmMan;
        private readonly IClipMan clipMan;

        #endregion Private Fields

        #region Public Constructors

        public StandingState(IFsmMan fsmMan, IClipMan clipMan)
        {
            this.fsmMan = fsmMan;
            this.clipMan = clipMan;
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
            var className = entity.Get<ClassComponent>().Name;
            var thrust = entity.Get<ThrustComponent>();

            thrust.Value = Vector2.Zero;

            var stateName = fsmMan.GetStateName(FsmId, Id);
            var clipId = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{stateName}/{animDirName}").Id;
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.PlayAnimation(0, clipId);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            entity.Subscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);


            //triggerMan.OnTrigger<ControlDirectionChangedEventArgs>(entity, OnControlDirectionChanged);
        }

        public void LeaveState(Entity entity)
        {
            entity.Unsubscribe<ControlDirectionChangedEventArgs>(OnControlDirectionChanged);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnControlDirectionChanged(object sender, ControlDirectionChangedEventArgs eventArgs)
        {
            var entity = sender as Entity;

            if (eventArgs.Direction != Vector2.Zero)
            {
                entity.SetState(FsmId, (int)MovementImpulse.Walk);

                var angularThrust = entity.Get<AngularVelocityComponent>();
                angularThrust.Value = new Vector2(eventArgs.Direction.X, eventArgs.Direction.Y);
            }
        }

        #endregion Private Methods
    }
}