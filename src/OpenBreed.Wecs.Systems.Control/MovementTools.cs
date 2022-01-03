using OpenTK;
using System;

namespace OpenBreed.Wecs.Systems.Control
{
    /// <summary>
    /// Various methods and values related with movement system and components
    /// </summary>
    public static class MovementTools
    {
        #region Private Fields

        private static readonly Vector2[] compass8Ways = {
            new Vector2( 1,  0),
            new Vector2( 1,  1),
            new Vector2( 0,  1),
            new Vector2(-1,  1),
            new Vector2(-1,  0),
            new Vector2(-1, -1),
            new Vector2( 0, -1),
            new Vector2( 1, -1)};

        #endregion Private Fields

        #region Public Methods

        public static Vector2 SnapToCompass8Way(Vector2 direction)
        {
            int octant = (((int)Math.Round(Math.Atan2(direction.Y, direction.X) / (2 * Math.PI / 8))) + 8) % 8;

            return compass8Ways[octant];
        }

        #endregion Public Methods
    }
}