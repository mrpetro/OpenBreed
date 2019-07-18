using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Modules.Physics.Components;

namespace OpenBreed.Core.Modules.Physics.Systems
{
    public class MovementSystem : WorldSystem, IUpdatableSystem
    {
        private const float FLOOR_FRICTION = 0.2f;

        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<IThrust> thrustComps = new List<IThrust>();
        private readonly List<IPosition> positionComps = new List<IPosition>();
        private readonly List<IDirection> directionComps = new List<IDirection>();
        private readonly List<IVelocity> velocityComps = new List<IVelocity>();
        private readonly List<IDynamicBody> dynamicBodyComps = new List<IDynamicBody>();

        #endregion Private Fields

        #region Public Constructors

        public MovementSystem(ICore core) : base(core)
        {
            Require<IThrust>();
            Require<IPosition>();
            Require<IDirection>();
            Require<IVelocity>();
            Require<IDynamicBody>();
        }

        #endregion Public Constructors

        #region Public Methods

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
            var direction = directionComps[index];
            var velocity = velocityComps[index];
            var dynamicBody = dynamicBodyComps[index];

            direction.Value = thrust.Value;

            //Velocity equation
            var newVel = velocity.Value + thrust.Value * dt;

            //Apply friction force
            newVel += -newVel * FLOOR_FRICTION * dynamicBody.FrictionFactor;

            //Verlet integration
            var newPos = position.Value + (velocity.Value + newVel) * 0.5f * dt;

            velocity.Value = newVel;
            dynamicBody.OldPosition = position.Value;
            position.Value = newPos;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
            positionComps.Add(entity.Components.OfType<IPosition>().First());
            thrustComps.Add(entity.Components.OfType<IThrust>().First());
            directionComps.Add(entity.Components.OfType<IDirection>().First());
            velocityComps.Add(entity.Components.OfType<IVelocity>().First());
            dynamicBodyComps.Add(entity.Components.OfType<IDynamicBody>().First());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            positionComps.RemoveAt(index);
            thrustComps.RemoveAt(index);
            directionComps.RemoveAt(index);
            velocityComps.RemoveAt(index);
            dynamicBodyComps.RemoveAt(index);
        }

        #endregion Protected Methods
    }
}