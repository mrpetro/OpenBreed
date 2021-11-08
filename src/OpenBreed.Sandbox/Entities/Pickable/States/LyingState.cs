
//using OpenBreed.Core.Commands;
//using OpenBreed.Core.Common.Systems.Components;
//using OpenBreed.Core.Entities;
//using OpenBreed.Core.Modules.Animation.Systems.Control.Events;
//using OpenBreed.Physics.Generic.Events;
//using OpenBreed.Wecs.Systems.Rendering.Commands;
//using OpenBreed.Core.States;
//using OpenBreed.Sandbox.Entities.Door.States;
//using System;
//using System.Linq;

//namespace OpenBreed.Sandbox.Entities.Pickable.States
//{
//    public class LyingState : IState
//    {
//        #region Private Fields

//        private int stampId;

//        #endregion Private Fields

//        #region Public Constructors

//        public LyingState(string id, int stampId)
//        {
//            Name = id;
//            this.stampId = stampId;
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public Entity Entity { get; private set; }
//        public string Name { get; }

//        #endregion Public Properties

//        #region Public Methods

//        public void EnterState()
//        {
//            // Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
//            var pos = Entity.GetComponent<PositionComponent>();
//            Entity.PostCommand(new PutStampCommand(Entity.World.Id, stampId, 0, pos.Value));
//            Entity.Subscribe<CollisionEventArgs>(OnCollision);
//        }

//        public void Initialize(Entity entity)
//        {
//            Entity = entity;
//        }

//        public void LeaveState()
//        {
//            Entity.Unsubscribe<CollisionEventArgs>(OnCollision);
//        }

//        private void OnCollision(object sender, CollisionEventArgs e)
//        {
//            //Entity.Impulse<FunctioningState, FunctioningImpulse>(FunctioningImpulse.Pick);
//            Entity.PostCommand(new EntitySetStateCommand(Entity.Id, "FunctioningState", "Pick"));
//        }

//        #endregion Public Methods

//        #region Private Methods

//        #endregion Private Methods
//    }
//}