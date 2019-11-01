using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.States;
using OpenBreed.Game.Entities.Projectile;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Entities.Pickable.States
{
    public class PickingState : IState
    {
        #region Private Fields

        private int stampId;

        #endregion Private Fields

        #region Public Constructors

        public PickingState(string id, int stampId)
        {
            Name = id;
            this.stampId = stampId;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; private set; }
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public void EnterState()
        {
            //Entity.PostMsg(new PlayAnimMsg(Entity, animationId));
            Entity.PostMsg(new TextSetMsg(Entity, String.Join(", ", Entity.CurrentStateNames.ToArray())));
            var pos = Entity.Components.OfType<IPosition>().FirstOrDefault();
            Entity.PostMsg(new PutStampMsg(Entity, stampId, 0, pos.Value));

            Entity.Core.Entities.Destroy(Entity);
        }

        public void Initialize(IEntity entity)
        {
            Entity = entity;
        }

        public void LeaveState()
        {

        }

        public string Process(string actionName, object[] arguments)
        {
            switch (actionName)
            {
                default:
                    break;
            }

            return null;
        }

        #endregion Public Methods
    }
}