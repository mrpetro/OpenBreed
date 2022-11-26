
using OpenBreed.Sandbox.Helpers;
using OpenTK;
using System;
using System.Linq;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Fsm;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Animation.Interface;
using OpenTK.Mathematics;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Systems.Physics.Events;

namespace OpenBreed.Sandbox.Entities.Actor.States.Movement
{
    public class StandingState : IState<MovementState, MovementImpulse>
    {
        #region Private Fields

        private const string ANIM_PREFIX = "Vanilla/Common";
        private readonly IFsmMan fsmMan;
        private readonly IClipMan<IEntity> clipMan;
        private readonly ITriggerMan triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public StandingState(IFsmMan fsmMan, IClipMan<IEntity> clipMan, ITriggerMan triggerMan)
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

        public void EnterState(IEntity entity)
        {
            Console.WriteLine("Standing -> Enter");

            var direction = entity.Get<AngularPositionComponent>().Value;

            var animDirName = AnimHelper.ToDirectionName(direction);
            var className = entity.Get<MetadataComponent>().Name;

            var stateName = fsmMan.GetStateName(FsmId, Id);
            var clipId = clipMan.GetByName($"{ANIM_PREFIX}/{className}/{stateName}/{animDirName}").Id;
            var currentStateNames = fsmMan.GetStateNames(entity);

            entity.StopAnimation(0);
            //entity.PlayAnimation(0, clipId);
            entity.SetText(0, string.Join(", ", currentStateNames.ToArray()));

            triggerMan.OnEntityVelocityChanged(entity, OnVelocityChanged);
        }

        public void LeaveState(IEntity entity)
        {
            Console.WriteLine("Standing -> Leave");
        }

        private bool OnVelocityChanged(IEntity entity, VelocityChangedEventArgs args)
        {
            var velocityComponent = entity.Get<VelocityComponent>();

            if (velocityComponent.Value == Vector2.Zero)
                return false;

            entity.SetState(FsmId, (int)MovementImpulse.Walk);
            return true;
        }

        #endregion Public Methods

        #region Private Methods


        #endregion Private Methods
    }
}