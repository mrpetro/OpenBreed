using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Physics;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class MovementSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 0.2f;

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal MovementSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            RequireEntityWith<ThrustComponent>();
            RequireEntityWith<PositionComponent>();
            RequireEntityWith<VelocityComponent>();
            RequireEntityWith<BodyComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                UpdateEntity(entities[i], dt);
        }

        public void UpdateEntity(Entity entity, float dt)
        {
            var position = entity.Get<PositionComponent>();
            var thrust = entity.Get<ThrustComponent>();
            var velocity = entity.Get<VelocityComponent>();
            var dynamicBody = entity.Get<BodyComponent>();

            //Velocity equation
            var newVel = velocity.Value + thrust.Value * dt;

            //Apply friction force
            newVel += -newVel * FLOOR_FRICTION * dynamicBody.CofFactor;

            //Verlet integration
            var newPos = position.Value + (velocity.Value + newVel) * 0.5f * dt;

            velocity.Value = newVel;
            position.Value = newPos;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods
    }
}