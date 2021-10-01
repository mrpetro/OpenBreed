using OpenBreed.Core.Commands;
using OpenBreed.Core.Managers;
using OpenBreed.Fsm;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Sandbox.Entities;
using OpenBreed.Sandbox.Entities.Door.States;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Commands;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenTK;
using System;

namespace OpenBreed.Sandbox.Components.States
{
    public class ClosedState : IState<FunctioningState, FunctioningImpulse>
    {
        #region Private Fields

        private readonly string stampPrefix;
        private readonly IFsmMan fsmMan;
        private readonly ICommandsMan commandsMan;
        private readonly ICollisionMan collisionMan;
        private readonly IStampMan stampMan;

        #endregion Private Fields

        #region Public Constructors

        public ClosedState(IFsmMan fsmMan, ICommandsMan commandsMan, ICollisionMan collisionMan, IStampMan stampMan)
        {
            this.fsmMan = fsmMan;
            this.commandsMan = commandsMan;
            this.collisionMan = collisionMan;
            this.stampMan = stampMan;
            stampPrefix = "Tiles/Stamps";
            collisionMan.RegisterCollisionPair(ColliderTypes.DoorOpenTrigger, ColliderTypes.ActorBody, DoorOpenTriggerCallback);
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
            //var messaging = entity.Get<MessagingComponent>();
            //messaging.Messages.Add(new SpriteOffMsg());

            commandsMan.Post(new SpriteOffCommand(entity.Id));

            var pos = entity.Get<PositionComponent>();

            var className = entity.Get<ClassComponent>().Name;
            var stateName = fsmMan.GetStateName(FsmId, Id);
            var stampId = stampMan.GetByName($"{stampPrefix}/{className}/{stateName}").Id;
            commandsMan.Post(new PutStampCommand(entity.Id, stampId, 0, pos.Value));

            //STAMP_DOOR_HORIZONTAL_CLOSED = $"{stampPrefix}/{className}/{stateName}";

            commandsMan.Post(new TextSetCommand(entity.Id, 0, "Door - Closed"));

            var colCmp = entity.Get<ColliderComponent>();

            colCmp.ColliderTypes.Add(ColliderTypes.StaticObstacle);
            colCmp.ColliderTypes.Add(ColliderTypes.DoorOpenTrigger);
        }

        public void LeaveState(Entity entity)
        {
            var colCmp = entity.Get<ColliderComponent>();
            colCmp.ColliderTypes.Remove(ColliderTypes.DoorOpenTrigger);
        }

        #endregion Public Methods

        #region Private Methods

        private void DoorOpenTriggerCallback(int colliderTypeA, Entity entityA, int colliderTypeB, Entity entityB, Vector2 projection)
        {
            commandsMan.Post(new SetEntityStateCommand(entityA.Id, FsmId, (int)FunctioningImpulse.Open));
        }

        #endregion Private Methods

        //private void OnCollision(object sender, CollisionEventArgs eventArgs)
        //{
        //    var entity = sender as Entity;
        //    entity.Core.Commands.Post(new SetStateCommand(entity.Id, FsmId, (int)FunctioningImpulse.Open));
        //}
    }
}