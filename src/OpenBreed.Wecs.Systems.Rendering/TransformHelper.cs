using OpenBreed.Wecs.Components.Rendering;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public static class TransformHelper
    {        /// <summary>
        #region Public Methods

        /// This will return camera tranformation matrix which includes aspect ratio correction
        /// </summary>
        /// <returns>Camera transformation matrix</returns>
        public static Matrix4 GetCameraTransform(ViewportScalingType viewportScalingType, Vector2 viewportSize, Vector2 cameraPos, Vector2 cameraSize)
        {
            var viewportRatio = viewportSize.X / viewportSize.Y;
            var cameraRatio = cameraSize.X / cameraSize.Y;
            var vcRatio = viewportRatio / cameraRatio;

            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-(int)cameraPos.X, -(int)cameraPos.Y, 0.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / cameraSize.X, 1.0f / cameraSize.Y, 1.0f));

            switch (viewportScalingType)
            {
                case ViewportScalingType.None:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(cameraSize.X / viewportSize.X, cameraSize.Y / viewportSize.Y, 1.0f));
                    break;

                case ViewportScalingType.FitHeightPreserveAspectRatio:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / vcRatio, 1.0f, 1.0f));
                    break;

                case ViewportScalingType.FitWidthPreserveAspectRatio:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f, vcRatio, 1.0f));
                    break;

                case ViewportScalingType.FitBothPreserveAspectRatio:
                    if (vcRatio >= 1)
                        transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / vcRatio, 1.0f, 1.0f));
                    else
                        transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f, vcRatio, 1.0f));
                    break;

                case ViewportScalingType.FitBothIgnoreAspectRatio:
                default:
                    break;
            }

            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f));

            return transform;
        }

        /// <summary>
        /// Local transformation matrix of this viewport
        /// </summary>
        public static Matrix4 GetViewportTransform(Vector2 pos, Vector2 size)
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(size.X, size.Y, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation((int)pos.X, (int)pos.Y, 0.0f));
            return transform;
        }

        public static Box2 GetVisibleRectangle(Vector2 pos, Vector2 size)
        {
            var x = pos.X;
            var y = pos.Y;
            return new Box2(x - size.X / 2.0f, y - size.Y / 2.0f, x + size.X / 2.0f, y + size.Y / 2.0f);
        }

        #endregion Public Methods
    }
}