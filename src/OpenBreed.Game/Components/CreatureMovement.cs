using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Movement;
using OpenBreed.Core.Systems.Movement.Components;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public class CreatureMovement : IMovementComponent
    {
        #region Private Fields

        private float speedPercent = 1.0f;
        private float MAXSPEED = 8.0f;

        private Vector2 thrust;

        private Position position;
        private Velocity velocity;
        private Direction direction;

        #endregion Private Fields

        #region Public Constructors

        public CreatureMovement()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public float SpeedPercent
        {
            get
            {
                return speedPercent;
            }

            set
            {
                speedPercent = MathHelper.Clamp(value, 0.0f, 1.0f);
            }
        }

        public float Speed { get { return speedPercent * MAXSPEED; } }

        public Type SystemType { get { return typeof(MovementSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().First();
            velocity = entity.Components.OfType<Velocity>().First();
            direction = entity.Components.OfType<Direction>().First();
        }

        public void Stop()
        {
            thrust = Vector2.Zero;
        }

        public void Move(Vector2 direction)
        {
            thrust = direction.Normalized() * Speed;
        }

        public void Update(float dt)
        {
            direction.Current = thrust;

            var newSpeed = velocity.Value;
            newSpeed += thrust;// * dt;

            newSpeed.X = MathHelper.Clamp(newSpeed.X, -MAXSPEED, MAXSPEED);
            newSpeed.Y = MathHelper.Clamp(newSpeed.Y, -MAXSPEED, MAXSPEED);

            position.Value += newSpeed;
        }

        #endregion Public Methods
    }
}