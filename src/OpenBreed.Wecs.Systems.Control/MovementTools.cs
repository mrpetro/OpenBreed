using OpenTK.Mathematics;
using System;

namespace OpenBreed.Wecs.Systems.Control
{
    /// <summary>
    /// Various methods and values related with movement system and components
    /// </summary>
    public static class MovementTools
    {
        #region Private Fields

        private static readonly float[] compass16Degrees = {
            0.0f,
            22.5f,
            45.0f,
            67.5f,
            90.0f,
            112.5f,
            135.0f,
            157.5f,
            180.0f,
            202.5f,
            225.0f,
            247.5f,
            270.0f,
            292.5f,
            315.0f,
            337.5f};

        private static readonly float[] compass8Degrees = {
            0.0f,
            45.0f,
            90.0f,
            135.0f,
            180.0f,
            225.0f,
            270.0f,
            315.0f};

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

        public static float SnapToCompass16Degree(float x, float y)
        {
            int index = (((int)Math.Round(Math.Atan2(y, -x) / (2 * Math.PI / 16))) + 16) % 16;
            return compass16Degrees[index];
        }

        //public static float SnapToCompass16Degree(Vector2 direction) => SnapToCompass16Degree(direction.X, direction.Y);

        public static float SnapToCompass8Degree(float x, float y)
        {
            int index = (((int)Math.Round(Math.Atan2(y, x) / (2 * Math.PI / 8))) + 8) % 8;
            return compass8Degrees[index];
        }

        public static float SnapToCompass8Degree(Vector2 direction) => SnapToCompass8Degree(direction.X, direction.Y);

        public static Vector2 SnapToCompass8Way(float x, float y)
        {
            int index = (((int)Math.Round(Math.Atan2(y, x) / (2 * Math.PI / 8))) + 8) % 8;
            return compass8Ways[index];
        }

        public static Vector2 SnapToCompass8Way(Vector2 direction) => SnapToCompass8Way(direction.X, direction.Y);

        #endregion Public Methods
    }
}