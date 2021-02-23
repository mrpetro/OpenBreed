using OpenBreed.Core.Extensions;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Physics.Events;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Physics
{
    public class DirectionSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private const float FLOOR_FRICTION = 1.0f;

        private readonly List<int> entities = new List<int>();
        private readonly IEntityMan entityMan;

        #endregion Private Fields

        #region Internal Constructors

        internal DirectionSystem(IEntityMan entityMan)
        {
            this.entityMan = entityMan;

            Require<AngularPositionComponent>();
            Require<AngularVelocityComponent>();
            Require<AngularThrustComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public void UpdatePauseImmuneOnly(float dt)
        {
        }

        public void Update(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                UpdateEntity(dt, entities[i]);
        }

        public void UpdateEntity(float dt, int id)
        {
            var entity = entityMan.GetById(id);
            var angularPos = entity.Get<AngularPositionComponent>();
            var angularVel = entity.Get<AngularVelocityComponent>();
            var angularThrust = entity.Get<AngularThrustComponent>();

            //Velocity equation
            //var newVel = angularVel.Value + angularThrust.Value * dt;

            var aPos = angularPos.GetDirection();
            var dPos = angularVel.GetDirection();

            var newPos = aPos.RotateTowards(dPos, (float)Math.PI * 0.125f, 1.0f);
            //newPos = MovementTools.SnapToCompass8Way(newPos);

            if (newPos == aPos)
                return;

            angularPos.SetDirection(newPos);
            entity.RaiseEvent(new DirectionChangedEventArgs(angularPos.GetDirection()));
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity.Id);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
        }

        #endregion Protected Methods
    }
}