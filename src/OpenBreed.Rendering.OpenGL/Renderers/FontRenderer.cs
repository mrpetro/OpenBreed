using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions.Renderers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Renderers
{
    internal class FontRenderer : IFontRenderer
    {
        #region Private Fields

        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Public Constructors

        public FontRenderer(IFontMan fontMan)
        {
            this.fontMan = fontMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void RenderPart(IRenderView view, int fontId, string text, Vector2 origin, Color4<Rgba> color, float order, Box2 clipBox, bool ignoreScale = false)
        {
            view.Translate(new Vector3(origin.X, origin.Y, order));
            fontMan.GetById(fontId).Draw(view, text, color, clipBox, ignoreScale);
        }

        public void RenderAppend(IRenderView view, int fontId, string text, Box2 clipBox, Vector2 value, bool ignoreScale = false)
        {
            fontMan.GetById(fontId).Draw(view, text, Color4.White, clipBox, ignoreScale);
        }

        public void Render(IRenderView view, Box2 clipBox, FontRenderCallback fontRenderer)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black[0], Color4.Black[1], Color4.Black[2], Color4.Black[3]);

            fontRenderer.Invoke(view, clipBox);

            GL.Disable(EnableCap.Blend);
        }

        public void RenderStart(IRenderView view, Vector2 pos)
        {
            view.PushMatrix();
            view.Translate(new Vector3(pos.X, pos.Y, 0.0f));

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BlendColor(Color4.Black[0], Color4.Black[1], Color4.Black[2], Color4.Black[3]);
        }

        public void RenderEnd(IRenderView view)
        {
            view.PopMatrix();

            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods
    }
}