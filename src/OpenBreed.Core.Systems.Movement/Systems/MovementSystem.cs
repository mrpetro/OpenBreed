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
                UpdateEntity(dt, entities[i]);
        }

        public void UpdateEntity(float dt, IEntity entity)
        {
            var position = entity.Components.OfType<Position>().First();
            var thrust = entity.Components.OfType<Thrust>().First();
            var direction = entity.Components.OfType<Direction>().First();
            var velocity = entity.Components.OfType<Velocity>().First();

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
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Public Methods
    }
}