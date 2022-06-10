using OpenBreed.Fsm;
using OpenBreed.Fsm.Extensions;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenBreed.Sandbox.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Linq;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly IFsmMan fsmMan;
        private readonly ICollisionMan<Entity> collisionMan;
        private readonly IStampMan stampMan;
        private readonly ItemsMan itemsMan;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(IFsmMan fsmMan, ICollisionMan<Entity> collisionMan, IStampMan stampMan, ItemsMan itemsMan )
        {
            this.fsmMan = fsmMan;
            this.collisionMan = collisionMan;
            this.stampMan = stampMan;
            this.itemsMan = itemsMan;
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
            var pos = entity.Get<PositionComponent>();
            var bodyCmp = entity.Get<BodyComponent>();
            var metadata = entity.Get<MetadataComponent>();
            var level = metadata.Level;
            var className = metadata.Name;
            var flavor = metadata.Flavor;


            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{level}/{className}/{flavor}/{stateName}").Id;

            entity.SetSpriteOff();
            entity.PutStampAtPosition(stampId, 0, pos.Value);
            //entity.SetText(0, "Door - Closed");


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

        private void DoorOpenTriggerCallback(BodyFixture colliderTypeA, Entity actorEntity, BodyFixture colliderTypeB, Entity doorEntity, Vector2 projection)
        {
            var metaData = doorEntity.Get<MetadataComponent>();

            //Check if door requires special key
            if (!string.IsNullOrEmpty(metaData.Option))
            {
                var keycardItemId = itemsMan.GetItemId(metaData.Option);

                if (keycardItemId != -1)
                {
                    var actorInventory = actorEntity.Get<InventoryComponent>();

                    var itemSlot = actorInventory.GetItemSlot(keycardItemId);

                    //No keycard item then door can't be opened
                    if (itemSlot == null)
                        return;
                }
            }

            doorEntity.SetState(FsmId, (int)FunctioningImpulse.Open);
        }

        #endregion Private Methods
    }
}