using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Extensions
{
    public static class RenderViewExtensions
    {
        #region Public Methods

        public static void MoveBy(this IRenderView renderView, Vector2i offset)
        {
            renderView.View *= Matrix4.CreateTranslation(offset.X, offset.Y, 0.0f);
        }

        public static void MoveTo(this IRenderView renderView, Vector2i position)
        {
            var scale = renderView.View[0, 0];
            renderView.View = Matrix4.CreateScale(scale, scale, 1.0f);
            renderView.View *= Matrix4.CreateTranslation(position.X, position.Y, 0.0f);

        }

        public static float GetScale(this IRenderView renderView) => renderView.View[0, 0];

        public static void SetScale(this IRenderView renderView, float scale)
        {
            renderView.View = Matrix4.CreateScale(scale, scale, 1.0f) * renderView.View.ClearScale();
        }

        public static void ZoomTo(this IRenderView renderView, Vector2i position, float scale)
        {
            var newTransf = renderView.View;
            ;
            var invMatrix = newTransf.Inverted();

            var t1Point = new Vector4(position.X, position.Y, 0.0f, 1.0f) * invMatrix;

            var toScale = scale / newTransf[0, 0];
            newTransf *= Matrix4.CreateScale(toScale, toScale, 1.0f);

            invMatrix = newTransf.Inverted();

            var t2Point = new Vector4(position.X, position.Y, 0.0f, 1.0f) * invMatrix;

            var offsetXDiff = t2Point.X - t1Point.X;
            var offsetYDiff = t2Point.Y - t1Point.Y;

            newTransf = Matrix4.CreateTranslation(offsetXDiff, offsetYDiff, 0.0f) * newTransf;

            renderView.View = newTransf;
        }

        #endregion Public Methods
    }
}