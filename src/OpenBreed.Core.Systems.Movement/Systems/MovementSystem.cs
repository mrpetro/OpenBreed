using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems.Movement.Systems
{
    public class MovementSystem : WorldSystemEx, IUpdatableSystemEx
    {
        #region Private Fields

        private float MAXSPEED = 8.0f;

        private List<IEntity> entities = new List<IEntity>();
        private List<Thrust> thrustComps = new List<Thrust>();
        private List<Position> positionComps = new List<Position>();
        private List<Direction> directionComps = new List<Direction>();
        private List<Velocity> velocityComps = new List<Velocity>();

        #endregion Private Fields

        #region Public Constructors

        public MovementSystem(ICore core) : base(core)
        {
            Require<Thrust>();
            Require<Position>();
            Require<Direction>();
            Require<Velocity>();
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

            direction.Current = thrust.Value;

            var newSpeed = velocity.Value;
            newSpeed += thrust.Value;// * dt;

            newSpeed.X = MathHelper.Clamp(newSpeed.X, -MAXSPEED, MAXSPEED);
            newSpeed.Y = MathHelper.Clamp(newSpeed.Y, -MAXSPEED, MAXSPEED);

            position.Value += newSpeed;
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
            positionComps.Add(entity.Components.OfType<Position>().First());
            thrustComps.Add(entity.Components.OfType<Thrust>().First());
            directionComps.Add(entity.Components.OfType<Direction>().First());
            velocityComps.Add(entity.Components.OfType<Velocity>().First());
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Public Methods
    }
}