using OpenBreed.Core.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Events;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Physics
{
    /// <summary>
    /// System which tries to replicate ABTA actor direction behavior
    /// </summary>
    public class DirectionSystemVanilla : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal DirectionSystemVanilla(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            RequireEntityWith<AngularPositionComponent>();
            RequireEntityWith<AngularVelocityComponent>();
            RequireEntityWith<AngularThrustComponent>();
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
            var angularPos = entity.Get<AngularPositionComponent>();
            var angularVel = entity.Get<AngularVelocityComponent>();

            //Velocity equation
            //var newVel = angularVel.Value + angularThrust.Value * dt;

            var aPos = angularPos.Value;
            var dPos = angularVel.Value;

            var newPos = aPos.RotateTowards(dPos, (float)Math.PI * 0.25f, 1.0f);

            if (newPos == aPos)
                return;

            angularPos.Value = newPos;
            entity.RaiseEvent(new DirectionChangedEventArgs(angularPos.Value));
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