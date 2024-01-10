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
        typeof(BroadphaseDynamicComponent))]
    public class UpdateDynamicBodySystem : IUpdatableSystem
    {
        #region Private Fields

        private readonly HashSet<IEntity> entities = new HashSet<IEntity>();

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Internal Constructors

        internal UpdateDynamicBodySystem(
            IEventsMan eventsMan,
            IEntityMan entityMan,
            IWorldMan worldMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
        }

        public int PhaseId => throw new System.NotImplementedException();

        public void AddEntity(IEntity entity) => entities.Add(entity);

        public bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public void RemoveEntity(IEntity entity) => entities.Remove(entity);

        #endregion Internal Constructors

        #region Public Methods

        public void Update(IWorldContext context)
        {
            foreach (var entity in entities)
            {
                if (entity.WorldId != context.World.Id)
                    continue;

                entity.UpdateDynamics(entityMan);
            }
        }

        //protected override void UpdateEntity(IEntity entity, IWorldContext context)
        //{
        //    var entityIds = entity.Get<BroadphaseStaticPutterComponent>().Ids;
        //    var grid = entity.Get<BroadphaseStaticComponent>().Grid;

        //    //Update all tiles
        //    for (int i = 0; i < entityIds.Count; i++)
        //        entity.AddEntityToStatics(entityMan.GetById(entityIds[i]));

        //    entityIds.Clear();
        //}

        #endregion Public Methods
    }
}