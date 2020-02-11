using OpenTK;

namespace OpenBreed.Core.Modules.Physics.Components.Shapes
{
    /// <summary>
    /// Place holder implementation for fixture box shape
    /// </summary>
    public class BoxShape : IShape
    {
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