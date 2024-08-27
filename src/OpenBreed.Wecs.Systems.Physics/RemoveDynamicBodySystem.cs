using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems.Physics.Extensions;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    public class RemoveDynamicBodySystem : EventMatchingSystem<EntityLeftEvent, RemoveDynamicBodySystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Internal Constructors

        internal RemoveDynamicBodySystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IWorldMan worldMan)
            : base(eventsMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Update(EntityLeftEvent e)
        {
            var eventEntity = entityMan.GetById(e.EntityId);

            //Check if cell entity has dynamic body
            if (!eventEntity.Contains<PositionComponent>() ||
                !eventEntity.Contains<BodyComponent>() ||
                !eventEntity.Contains<VelocityComponent>())
            {
                return;
            }

            var eventWorld = worldMan.GetById(e.WorldId);

            foreach (var entity in entities)
            {
                if (entity.WorldId != eventWorld.Id)
                    continue;

                entity.RemoveEntityFromDynamic(eventEntity);
            }
        }

        #endregion Public Methods
    }
}