﻿using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Modules.Physics.Helpers;
using OpenBreed.Core.Components;

namespace OpenBreed.Core.Modules.Physics.Systems
{
    public class MovementSystem : WorldSystem, IUpdatableSystem
    {
        private const float FLOOR_FRICTION = 0.2f;

        #region Private Fields

        private readonly List<int> entities = new List<int>();

        #endregion Private Fields

        #region Public Constructors

        public MovementSystem(MovementSystemBuilder builder) : base(builder.core)
        {
            Require<ThrustComponent>();
            Require<PositionComponent>();
            Require<VelocityComponent>();
            Require<BodyComponent>();
        }

        #endregion Public Constructors

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
            var entity = Core.Entities.GetById(id);
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
            dynamicBody.OldPosition = position.Value;
            position.Value = newPos;
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