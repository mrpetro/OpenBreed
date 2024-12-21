using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Gui.Abstractions.Helpers
{
    public static class Box2Helper
    {
        #region Public Methods

        public static Box2 NewBox(Vector2 center, Vector2 size) => NewBox(center.X, center.Y, size.X, size.Y);

        public static Box2 NewBox(float centerX, float centerY, float sizeX, float sizeY)
        {
            return new Box2(
                centerX - sizeX / 2.0f,
                centerY - sizeY / 2.0f,
                centerX + sizeX / 2.0f,
                centerY + sizeY / 2.0f);
        }

        #endregion Public Methods
    }
}