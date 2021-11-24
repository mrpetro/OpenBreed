using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private const string STAMP_PREFIX = "Vanilla/L4";

        private readonly IFsmMan fsmMan;
        private readonly ICollisionMan collisionMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(IFsmMan fsmMan, ICollisionMan collisionMan, IStampMan stampMan)
        {
            this.fsmMan = fsmMan;
            this.collisionMan = collisionMan;
            this.stampMan = stampMan;

            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.DoorOpenTrigger, DoorOpenTriggerCallback);
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Closed;

        /// <summary>
        /// TODO: Insecure, encapsulate this somehow
        /// </summary>
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            entity.SetSpriteOff();

            var pos = entity.Get<PositionComponent>();

            var metadata = entity.Get<ClassComponent>();
            var className = metadata.Name;
            var flavor = metadata.Flavor;

            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{STAMP_PREFIX}/{className}/{flavor}/{stateName}").Id;

            entity.PutStamp(stampId, 0, pos.Value);
            //entity.SetText(0, "Door - Closed");

            var bodyCmp = entity.Get<BodyComponent>();

            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.Clear();
            fixture.GroupIds.Add(ColliderTypes.FullObstacle);
            fixture.GroupIds.Add(ColliderTypes.DoorOpenTrigger);
        }

        public void LeaveState(Entity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();
            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.RemoveAll(id => id == ColliderTypes.DoorOpenTrigger);
        }

        #endregion Public Methods

        #region Private Methods

        private void DoorOpenTriggerCallback(BodyFixture colliderTypeA, Entity entityA, BodyFixture colliderTypeB, Entity entityB, Vector2 projection)
        {
            entityB.SetState(FsmId, (int)FunctioningImpulse.Open);
        }

        #endregion Private Methods
    }
}