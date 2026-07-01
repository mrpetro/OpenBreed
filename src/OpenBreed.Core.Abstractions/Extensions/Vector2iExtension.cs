using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Core.Abstractions.Extensions
{
    public enum CellAnchor
    {
        BottomLeft,
        Center,
        TopRight
    }

    public static class Vector2iExtension
    {
        #region Public Methods

        public static Vector2 ToPosition(this Vector2i index, int cellSize = 16, CellAnchor anchor = CellAnchor.BottomLeft)
        {
            return anchor switch
            {
                CellAnchor.BottomLeft => new Vector2(
                    index.X * cellSize,
                    index.Y * cellSize),

                CellAnchor.Center => new Vector2(
                    (index.X + 0.5f) * cellSize,
                    (index.Y + 0.5f) * cellSize),

                CellAnchor.TopRight => new Vector2(
                    (index.X + 1f) * cellSize,
                    (index.Y + 1f) * cellSize),

                _ => throw new ArgumentOutOfRangeException(nameof(anchor))
            };
        }


        #endregion Public Methods
    }
}