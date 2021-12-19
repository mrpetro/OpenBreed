using OpenBreed.Physics.Interface;
using OpenTK;

namespace OpenBreed.Physics.Generic.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture circle shape
    /// </summary>
    public class CircleShape : IShape
    {
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