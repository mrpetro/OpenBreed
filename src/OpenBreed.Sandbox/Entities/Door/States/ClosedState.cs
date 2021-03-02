using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Rendering.Interface;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenTK;
using System;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Physics.Interface;
using OpenBreed.Fsm;
using OpenBreed.Wecs.Entities;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string stampPrefix;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(ICore core)
        {
            stampPrefix = "Tiles/Stamps";

            core.GetManager<ICollisionMan>().RegisterCollisionPair(ColliderTypes.DoorOpenTrigger, ColliderTypes.ActorBody, DoorOpenTriggerCallback);
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
            entity.Core.Commands.Post(new SpriteOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();

            var className = entity.Get<ClassComponent>().Name;
            var stateName = entity.Core.GetManager<IFsmMan>().GetStateName(FsmId, Id);
            var stampId = entity.Core.GetManager<IStampMan>().GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            entity.Core.Commands.Post(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            //STAMP_DOOR_HORIZONTAL_CLOSED = $"{stampPrefix}/{className}/{stateName}";

            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Closed"));

            var colCmp = entity.Get<CollisionComponent>();

            colCmp.ColliderTypes.Add(ColliderTypes.StaticObstacle);
            colCmp.ColliderTypes.Add(ColliderTypes.DoorOpenTrigger);
        }

        private void DoorOpenTriggerCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            entityA.Core.Commands.Post(new SetEntityStateCommand(entityA.Id, FsmId, (int)FunctioningImpulse.Open));
        }

        public void LeaveState(Entity entity)
        {
            var colCmp = entity.Get<CollisionComponent>();
            colCmp.ColliderTypes.Remove(ColliderTypes.DoorOpenTrigger);
        }

        #endregion Public Methods

        #region Private Methods

        //private void OnCollision(object sender, CollisionEventArgs eventArgs)
        //{
        //    var entity = sender as Entity;
        //    entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.Open));
        //}

        #endregion Private Methods
    }
}