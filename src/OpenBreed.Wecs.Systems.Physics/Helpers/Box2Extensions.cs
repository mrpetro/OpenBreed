using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Physics.Helpers
{
    /// <summary>
    /// Extension methods for OpenTK.Box2 class
    /// </summary>
    public static class Box2Extensions
    {
        #region Public Methods

        /// <summary>
        /// Check collision between this box and other box
        /// </summary>
        /// <param name="thisBox">This box</param>
        /// <param name="otherBox">Other box</param>
        /// <returns>True if boxes are colliding, false otherwise</returns>
        public static bool CollidesWith(this Box2 thisBox, Box2 otherBox)
        {
            if (thisBox.Min.X > otherBox.Max.X)
                return false;

            if (thisBox.Max.X < otherBox.Min.X)
                return false;

            if (thisBox.Min.Y > otherBox.Max.Y)
                return false;

            if (thisBox.Max.Y < otherBox.Min.Y)
                return false;

            return true;
        }

        /// <summary>
        /// Returns center point of this box
        /// </summary>
        /// <param name="thisBox">This box</param>
        /// <returns>Center point</returns>
        public static Vector2 GetCenter(this Box2 thisBox)
        {
            return new Vector2((thisBox.Min.X + thisBox.Max.X) / 2.0f,
                               (thisBox.Min.Y + thisBox.Max.Y) / 2.0f);
        }

        #endregion Public Methods
    }
}