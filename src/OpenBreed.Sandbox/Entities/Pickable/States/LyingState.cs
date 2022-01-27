using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Entities.Pickable.States
{
    public class LyingState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;

        private readonly ICollisionMan<Entity> collisionMan;
        private readonly IStampMan stampMan;
        private readonly ItemsMan itemsMan;
        private readonly IEventsManEx triggerMan;

        #endregion Private Fields

        #region Public Constructors

        public LyingState(IFsmMan fsmMan, ICollisionMan<Entity> collisionMan, IStampMan stampMan, ItemsMan itemsMan, IEventsManEx triggerMan)
        {
            this.fsmMan = fsmMan;
            this.collisionMan = collisionMan;
            this.stampMan = stampMan;
            this.itemsMan = itemsMan;
            this.triggerMan = triggerMan;
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
            var metadata = entity.Get<MetadataComponent>();
            var level = metadata.Level;
            var className = metadata.Name;
            var flavor = metadata.Flavor;
            var stateName = fsmMan.GetStateName(FsmId, Id);

            if (flavor != "Trigger")
            {
                int stampId;

                if (flavor is null)
                    stampId = stampMan.GetByName($"{level}/{className}/{stateName}").Id;
                else
                    stampId = stampMan.GetByName($"{level}/{className}/{flavor}/{stateName}").Id;

                entity.PutStamp(stampId, 0, pos.Value);
            }

            if(entity.Contains<TextComponent>())
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
            var metaData = entityB.Get<MetadataComponent>();

            var itemId = itemsMan.GetItemId($"{metaData.Name}{metaData.Option}");

            //Unknown item
            if (itemId == -1)
                return;

            entityB.SetState(FsmId, (int)FunctioningImpulse.Pick);
            entityA.GiveItem(itemId, 1);

            triggerMan.Fire(GameEventTypes.HeroPickedItem, entityA.Id, itemId);
        }

        #endregion Private Methods
    }
}