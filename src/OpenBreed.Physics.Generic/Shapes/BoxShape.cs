using OpenBreed.Physics.Interface;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Physics.Generic.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture box shape
    /// </summary>
    public class BoxShape : IBoxShape
    {
        public BoxShape(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #region Public Properties

        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        #endregion Public Properties

        #region Public Methods

        public Box2 GetAabb()
        {
            return new Box2(X, Y + Height, X + Width, Y);
        }

        #endregion Public Methods
    }
}