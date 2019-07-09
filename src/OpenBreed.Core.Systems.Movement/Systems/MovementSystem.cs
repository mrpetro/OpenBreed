using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Common.Systems.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Animation.Systems.Movement.Systems
{
    public class MovementSystem : WorldSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<IThrust> thrustComps = new List<IThrust>();
        private readonly List<IPosition> positionComps = new List<IPosition>();
        private readonly List<IDirection> directionComps = new List<IDirection>();
        private readonly List<IVelocity> velocityComps = new List<IVelocity>();
        private float MAXSPEED = 8.0f;

        #endregion Private Fields

        #region Public Constructors

        public MovementSystem(ICore core) : base(core)
        {
            Require<IThrust>();
            Require<IPosition>();
            Require<IDirection>();
            Require<IVelocity>();
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

            direction.Value = thrust.Value;

            var newSpeed = velocity.Value;
            newSpeed += thrust.Value;// * dt;

            newSpeed.X = MathHelper.Clamp(newSpeed.X, -MAXSPEED, MAXSPEED);
            newSpeed.Y = MathHelper.Clamp(newSpeed.Y, -MAXSPEED, MAXSPEED);

            position.Value += newSpeed;
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
        }

        #endregion Protected Methods
    }
}