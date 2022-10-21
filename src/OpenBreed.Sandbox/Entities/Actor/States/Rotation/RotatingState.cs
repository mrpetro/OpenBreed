using OpenBreed.Animation.Interface;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Sandbox.Helpers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Actor.States.Rotation
{
    public class RotatingState : IState<RotationState, RotationImpulse>
    {
        #region Private Fields

        private const string ANIM_PREFIX = "Vanilla/Common";
        private readonly IClipMan<Entity> clipMan;
        private readonly IFsmMan fsmMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public RotatingState(
            IClipMan<Entity> clipMan,
            IFsmMan fsmMan,
            ITriggerMan triggerMan)
        {
            this.clipMan = clipMan;
            this.fsmMan = fsmMan;
            this.triggerMan = triggerMan;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)RotationState.Rotating;
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            Console.WriteLine("Rot -> Enter");
            var currentStateNames = fsmMan.GetStateNames(entity);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            var direction = entity.Get<AngularPositionComponent>();
            var animDirName = AnimHelper.ToDirectionName(direction.Value);
            var className = entity.Get<MetadataComponent>().Name;
            var movementFsm = fsmMan.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);

            var clip = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{movementStateName}/{animDirName}");

            entity.PlayAnimation(0, clip.Id);

            triggerMan.OnEntityDirectionChanged(entity, OnDirectionChanged);
        }

        private bool OnDirectionChanged(Entity entity, DirectionChangedEventArgs args)
        {
            var angularVelocity = entity.Get<AngularVelocityComponent>();
            var angularPosition = entity.Get<AngularPositionComponent>();


            var animDirName = AnimHelper.ToDirectionName(angularPosition.Value);
            var className = entity.Get<MetadataComponent>().Name;
            var movementFsm = fsmMan.GetByName("Actor.Movement");
            var movementStateName = movementFsm.GetCurrentStateName(entity);

            var clip = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{movementStateName}/{animDirName}");

            entity.PlayAnimation(0, clip.Id);


            if (angularVelocity.Value != angularPosition.Value)
                return false;

            entity.SetState(FsmId, (int)RotationImpulse.Stop);
            return true;
        }

        public void LeaveState(Entity entity)
        {
            Console.WriteLine("Rot -> Leave");
        }

        #endregion Public Methods
    }
}