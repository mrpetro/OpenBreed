using OpenBreed.Physics.Interface;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Generic.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture point shape
    /// </summary>
    public class PointShape : IPointShape
    {
        public PointShape(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Public Properties

        public float X { get; set; }
        public float Y { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Box2 GetAabb()
        {
            return new Box2(X, Y, X, Y);
        }

        public override string ToString()
        {
            return $"Point (X: {X}, Y: {Y})";
        }

        #endregion Public Methods
    }
}