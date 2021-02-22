using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class PrimitiveRenderer : IPrimitiveRenderer
    {
        #region Public Methods

        public void DrawUnitRectangle()
        {
            RenderTools.DrawUnitRectangle();
        }

        public void DrawRectangle(Box2 clipBox)
        {
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

        #endregion Public Methods
    }
}