
//using OpenBreed.Core.Common.Systems.Components;
//using OpenBreed.Core.Entities;
//using OpenBreed.Core.Modules.Physics.Events;
//using OpenBreed.Systems.Rendering.Commands;
//using OpenBreed.Core.States;
//using OpenBreed.Sandbox.Entities.Projectile;
//using OpenTK;
//using System;
//using System.Linq;

//namespace OpenBreed.Sandbox.Entities.Pickable.States
//{
//    public class PickingState : IState
//    {
//        #region Private Fields

//        private int stampId;

//        #endregion Private Fields

//        #region Public Constructors

//        public PickingState(string id, int stampId)
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
//            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
//            Entity.PostCommand(new TextSetCommand(Entity.Id, 0, String.Join(", ", Entity.CurrentStateNames.ToArray())));
//            var pos = Entity.GetComponent<PositionComponent>();
//            Entity.PostCommand(new PutStampCommand(Entity.World.Id, stampId, 0, pos.Value));

//            Entity.Core.Entities.Destroy(Entity);
//        }

//        public void Initialize(Entity entity)
//        {
//            Entity = entity;
//        }

//        public void LeaveState()
//        {

//        }

//        #endregion Public Methods
//    }
//}