using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Common.Game.Wecs.Components;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Systems.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems
{
    public class ItemPickupSystem : IEventSystem<ActorCollisionEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Public Constructors

        public ItemPickupSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(ActorCollisionEvent e)
        {
            var actorEntity = entityMan.GetById(e.EntityId);
            var otherEntity = entityMan.GetById(e.OtherEntityId);
        }

        #endregion Public Methods
    }
}