using OpenBreed.Physics.Interface;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Generic.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture circle shape
    /// </summary>
    public class CircleShape : ICircleShape
    {
        #region Public Constructors

        public CircleShape(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Center { get; set; }
        public float Radius { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Box2 GetAabb()
        {
            return new Box2(Center.X - Radius / 2,
                            Center.Y + Radius / 2,
                            Center.X + Radius / 2,
                            Center.Y - Radius / 2);
        }

        #endregion Public Methods
    }
}