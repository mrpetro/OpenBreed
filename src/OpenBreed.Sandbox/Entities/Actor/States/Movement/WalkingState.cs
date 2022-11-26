using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Control;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Systems.Physics.Events;
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
        private readonly IClipMan<IEntity> clipMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public WalkingState(IFsmMan fsmMan, IClipMan<IEntity> clipMan, ITriggerMan triggerMan)
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

        public void EnterState(IEntity entity)
        {
            Console.WriteLine("Walking -> Enter");

            var direction = entity.Get<AngularPositionComponent>();

            var animDirPostfix = AnimHelper.ToDirectionName(direction.Value);
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var className = entity.Get<MetadataComponent>().Name;
            var clip = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{stateName}/{animDirPostfix}");
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.PlayAnimation(0, clip.Id);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            triggerMan.OnEntityVelocityChanged(entity, OnVelocityChanged);
        }

        public void LeaveState(IEntity entity)
        {
            Console.WriteLine("Leave -> Enter");

        }

        #endregion Public Methods

        #region Private Methods

        private bool OnVelocityChanged(IEntity entity, VelocityChangedEventArgs args)
        {
            var velocityComponent = entity.Get<VelocityComponent>();

            if (velocityComponent.Value != Vector2.Zero)
                return false;

            entity.SetState(FsmId, (int)MovementImpulse.Stop);
            return true;
        }

        #endregion Private Methods
    }
}