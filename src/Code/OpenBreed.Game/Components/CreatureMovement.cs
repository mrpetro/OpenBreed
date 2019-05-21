using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using OpenBreed.Core.Systems.Movement;
using OpenBreed.Core.Systems.Movement.Components;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Components
{
    public enum MovementDirection
    {
        Right,
        Up,
        Left,
        Down
    }

    public class CreatureMovement : IMovementComponent
    {
        #region Private Fields

        private float speedPercent = 1.0f;
        private float MAXSPEED = 8.0f;

        private Vector2 thrust;

        private DynamicPosition position;
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
            position = entity.Components.OfType<DynamicPosition>().First();
            direction = entity.Components.OfType<Direction>().First();
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                thrust = Vector2.Zero;
            else
                thrust = direction.Normalized() * Speed;
        }

        public void Update(float dt)
        {
            direction.Current = thrust;

            var newSpeed = position.Velocity;
            newSpeed += thrust;// * dt;

            newSpeed.X = MathHelper.Clamp(newSpeed.X, -MAXSPEED, MAXSPEED);
            newSpeed.Y = MathHelper.Clamp(newSpeed.Y, -MAXSPEED, MAXSPEED);

            position.Current = position.Old;
            position.Current += newSpeed;

            thrust = Vector2.Zero;
        }

        #endregion Public Methods

        #region Private Methods

        private float AngleBetween(Vector2 a, Vector2 b)
        {
            double angleA;
            double angleB;
            if (a.Y >= 0.0f)
                angleA = Math.Atan2(a.Y, a.X);
            else
                angleA = 2 * Math.PI + Math.Atan2(a.Y, a.X);

            if (b.Y >= 0.0f)
                angleB = Math.Atan2(b.Y, b.X);
            else
                angleB = 2 * Math.PI + Math.Atan2(b.Y, b.X);

            return (float)(angleB - angleA);
        }

        #endregion Private Methods
    }
}