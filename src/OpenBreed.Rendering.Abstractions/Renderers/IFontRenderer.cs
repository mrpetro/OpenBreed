using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Abstractions.Renderers
{
    public interface IFontRenderer
    {
        #region Public Methods

        void RenderPart(IRenderView view, int fontId, string text, Vector2 origin, Color4<Rgba> color, float order, Box2 clipBox, bool ignoreScale = false);

        void RenderAppend(IRenderView view, int fontId, string text, Box2 clipBox, Vector2 value, bool ignoreScale = false);

        void Render(IRenderView view, Box2 clipBox, FontRenderCallback fontRenderer);

        void RenderStart(IRenderView view, Vector2 value);

        void RenderEnd(IRenderView view);

        #endregion Public Methods
    }
}