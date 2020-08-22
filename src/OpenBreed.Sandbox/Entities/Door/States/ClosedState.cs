using OpenBreed.Core;
using OpenBreed.Core.Commands;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.States;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenTK;
using System;

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

            core.GetModule<PhysicsModule>().Collisions.RegisterCollisionPair(ColliderTypes.DoorOpenTrigger, ColliderTypes.ActorBody, DoorOpenTriggerCallback);
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
            var stateName = entity.Core.StateMachines.GetStateName(FsmId, Id);
            var stampId = entity.Core.Rendering.Stamps.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            entity.Core.Commands.Post(new PutStampCommand(entity.World.Id, stampId, 0, pos.Value));

            //STAMP_DOOR_HORIZONTAL_CLOSED = $"{stampPrefix}/{className}/{stateName}";

            entity.Core.Commands.Post(new TextSetCommand(entity.Id, 0, "Door - Closed"));

            var colCmp = entity.Get<CollisionComponent>();

            colCmp.ColliderTypes.Add(ColliderTypes.StaticObstacle);
            colCmp.ColliderTypes.Add(ColliderTypes.DoorOpenTrigger);
        }

        private void DoorOpenTriggerCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            entityA.Core.Commands.Post(new SetStateCommand(entityA.Id, FsmId, (int)FunctioningImpulse.Open));
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