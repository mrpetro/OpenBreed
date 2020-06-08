﻿using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Components;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Modules.Physics.Helpers;

namespace OpenBreed.Core.Modules.Physics.Systems
{
    public class MovementSystem : WorldSystem, IUpdatableSystem
    {
        private const float FLOOR_FRICTION = 0.2f;

        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<ThrustComponent> thrustComps = new List<ThrustComponent>();
        private readonly List<PositionComponent> positionComps = new List<PositionComponent>();
        private readonly List<VelocityComponent> velocityComps = new List<VelocityComponent>();
        private readonly List<BodyComponent> dynamicBodyComps = new List<BodyComponent>();

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
                UpdateEntity(dt, i);
        }

        public void UpdateEntity(float dt, int index)
        {
            var entity = entities[index];
            var position = positionComps[index];
            var thrust = thrustComps[index];
            var velocity = velocityComps[index];
            var dynamicBody = dynamicBodyComps[index];

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

        protected override void OnAddEntity(IEntity entity)
        {
            entities.Add(entity);
            positionComps.Add(entity.GetComponent<PositionComponent>());
            thrustComps.Add(entity.GetComponent<ThrustComponent>());
            velocityComps.Add(entity.GetComponent<VelocityComponent>());
            dynamicBodyComps.Add(entity.GetComponent<BodyComponent>());
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            positionComps.RemoveAt(index);
            thrustComps.RemoveAt(index);
            velocityComps.RemoveAt(index);
            dynamicBodyComps.RemoveAt(index);
        }

        #endregion Protected Methods
    }
}