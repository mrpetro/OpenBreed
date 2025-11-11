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
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace OpenBreed.Wecs.Systems.Physics
{
    [RequireEntityWith(
        typeof(CollisionComponent))]
    public class AddDynamicBodySystem : MatchingSystemBase, IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public AddDynamicBodySystem(
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(EntityEnteredEvent e)
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

                entity.AddEntityToDynamics(eventEntity);
            }
        }

        #endregion Public Methods

        //protected override void UpdateEntity(IEntity entity, IWorldContext context)
        //{
        //    var entityIds = entity.Get<BroadphaseStaticPutterComponent>().Ids;
        //    var grid = entity.Get<BroadphaseStaticComponent>().Grid;

        //    //Update all tiles
        //    for (int i = 0; i < entityIds.Count; i++)
        //        entity.AddEntityToStatics(entityMan.GetById(entityIds[i]));

        //    entityIds.Clear();
        //}
    }
}