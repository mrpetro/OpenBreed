using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities;
using OpenTK;
using System;
using System.Linq;

namespace OpenBreed.Game.Movement.Components
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

        private int x;
        private int y;

        private Position position;
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
            position = entity.Components.OfType<Position>().First();
            direction = entity.Components.OfType<Direction>().First();
        }

        public void Move(MovementDirection direction)
        {
            switch (direction)
            {
                case MovementDirection.Right:
                    x = 1;
                    break;

                case MovementDirection.Up:
                    y = 1;
                    break;

                case MovementDirection.Left:
                    x = -1;
                    break;

                case MovementDirection.Down:
                    y = -1;
                    break;
            }
        }

        public void Update(float dt)
        {
            var step = new Vector2(dt * x * 64.0f, dt * y * 64.0f);

            direction.X = x;
            direction.Y = y;

            position.X += step.X;
            position.Y += step.Y;

            x = 0;
            y = 0;
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