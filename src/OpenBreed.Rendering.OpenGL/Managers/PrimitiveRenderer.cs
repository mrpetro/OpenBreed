using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class PrimitiveRenderer : IPrimitiveRenderer
    {
        #region Private Fields

        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        #endregion Private Fields

        #region Public Methods

        public void DrawUnitRectangle()
        {
            RenderTools.DrawUnitRectangle();
        }

        public void DrawUnitRectangle(Color4 color)
        {
            GL.Color4(color);
            RenderTools.DrawUnitRectangle();
        }

        public void DrawRectangle(Box2 clipBox)
        {
            RenderTools.DrawRectangle(clipBox);
        }

        public void DrawRectangle(Box2 clipBox, Color4 color)
        {
            GL.Color4(color);
            RenderTools.DrawRectangle(clipBox);
        }

        public void DrawBox(Box2 clipBox)
        {
            RenderTools.DrawBox(clipBox);
        }

        public void DrawUnitBox()
        {
            RenderTools.DrawUnitBox();
        }

        public void DrawUnitBox(Color4 color)
        {
            GL.Color4(color);
            RenderTools.DrawUnitBox();
        }

        public void DrawBrightnessBox(float brightness)
        {
            GL.Enable(EnableCap.Blend);
            if (brightness > 1.0)
            {
                GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.One);
                GL.Color3(brightness - 1, brightness - 1, brightness - 1);
            }
            else
            {
                GL.BlendFunc(BlendingFactor.Zero, BlendingFactor.SrcColor);
                GL.Color3(brightness, brightness, brightness);
            }

            GL.Translate(0, 0, BRIGHTNESS_Z_LEVEL);
            DrawUnitBox();
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods
    }
}