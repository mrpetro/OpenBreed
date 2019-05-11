using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
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
        private float SPEED = 4.0f;
        private float MAXSPEED = 16.0f;


        #region Private Fields

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

        public void Move(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Right:
                    thrust.X += SPEED;
                    break;

                case MovementDirection.Up:
                    thrust.Y += SPEED;
                    break;

                case MovementDirection.Left:
                    thrust.X -= SPEED;
                    break;

                case MovementDirection.Down:
                    thrust.Y -= SPEED;
                    break;
            }
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