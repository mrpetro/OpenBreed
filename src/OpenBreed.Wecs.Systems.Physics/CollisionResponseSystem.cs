﻿using OpenBreed.Physics.Interface;
using OpenBreed.Physics.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Commands;
using OpenBreed.Wecs.Systems.Physics.Events;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class CollisionResponseSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly ICollisionMan collisionMan;

        #endregion Private Fields

        #region Internal Constructors

        public CollisionResponseSystem(IEntityMan entityMan, IWorldMan worldMan, ICollisionMan collisionMan)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.collisionMan = collisionMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {

        }

        private IEnumerable<Entity> GetEntitiesWith<TComponent>()
        {
            var world = worldMan.GetById(WorldId);
            return world.Entities.Where(entity => entity.Contains<TComponent>());
        }

        public void Update(float dt)
        {
            var entities = GetEntitiesWith<CollisionComponent>();

            foreach (var entity in entities)
            {
                if (!entity.Contains<VelocityComponent>())
                {
                    entity.Remove<CollisionComponent>();
                    continue;
                }

                var collisionComponent = entity.Get<CollisionComponent>();

                var pd = Vector2.Zero;
                var vd = Vector2.Zero;
                var posComponnent = entity.Get<PositionComponent>();
                var velComponnent = entity.Get<VelocityComponent>();

                if (collisionComponent.Contacts.Count > 4)
                {
                }

                //collisionMan.Resolve(entity, contactEntity, collisionComponent.Contacts);


                foreach (var contact in collisionComponent.Contacts)
                {
                    var contactEntity = entityMan.GetById(contact.EntityId);

                    var projection = contact.Projection - pd;

                    if (projection == Vector2.Zero)
                    {
                        pd = projection;
                        break;
                    }

                    var oldPos = posComponnent.Value;
                    var oldVel = velComponnent.Value;

                    pd = posComponnent.Value - oldPos;
                    vd = velComponnent.Value - oldVel;
                }

                entity.Remove<CollisionComponent>();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods



        #endregion Private Methods
    }
}