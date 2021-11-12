using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Pickable.States
{
    public class LyingState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private const string STAMP_PREFIX = "Tiles/Stamps/Pickable/L4";
        private readonly IFsmMan fsmMan;

        private readonly ICollisionMan collisionMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public LyingState(IFsmMan fsmMan, ICollisionMan collisionMan, IStampMan stampMan)
        {
            this.fsmMan = fsmMan;
            this.collisionMan = collisionMan;
            this.stampMan = stampMan;

            collisionMan.RegisterFixturePair(ColliderTypes.ActorBody, ColliderTypes.ItemPickupTrigger, PickableCollisionCallback);
        }

        #endregion Public Constructors

        #region Public Properties

        public int Id => (int)(ValueType)FunctioningState.Lying;

        /// <summary>
        /// TODO: Insecure, encapsulate this somehow
        /// </summary>
        public int FsmId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState(Entity entity)
        {
            var pos = entity.Get<PositionComponent>();
            var metadata = entity.Get<ClassComponent>();
            var className = metadata.Name;
            var flavor = metadata.Flavor;
            var stateName = fsmMan.GetStateName(FsmId, Id);

            int stampId;

            if(flavor is null)
                stampId = stampMan.GetByName($"{STAMP_PREFIX}/{className}/{stateName}").Id;
            else
                stampId = stampMan.GetByName($"{STAMP_PREFIX}/{className}/{flavor}/{stateName}").Id;

            entity.PutStamp(stampId, 0, pos.Value);
            entity.SetText(0, $"{className} - Lying");

            var bodyCmp = entity.Get<BodyComponent>();

            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.Clear();
            fixture.GroupIds.Add(ColliderTypes.ItemPickupTrigger);
        }

        public void LeaveState(Entity entity)
        {
            var bodyCmp = entity.Get<BodyComponent>();
            var fixture = bodyCmp.Fixtures.First();
            fixture.GroupIds.RemoveAll(id => id == ColliderTypes.ItemPickupTrigger);
        }

        #endregion Public Methods

        #region Private Methods

        private void PickableCollisionCallback(BodyFixture fixtureA, Entity entityA, BodyFixture fixtureB, Entity entityB, Vector2 projection)
        {
            entityB.SetState(FsmId, (int)FunctioningImpulse.Pick);
        }

        #endregion Private Methods
    }
}