using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.Interface.Managers
{
    public delegate void FontRenderer(IRenderView view, Box2 clipBox, float dt);

    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFontAtlasBuilder Create();

        void RenderPart(IRenderView view, int fontId, string text, Vector2 origin, Color4 color, float order, Box2 clipBox, bool ignoreScale = false);

        void RenderAppend(IRenderView view, int fontId, string text, Box2 clipBox, Vector2 value, bool ignoreScale = false);

        IFont GetOSFont(string fontName, int fontSize);

        IFont GetGfxFont(string fontName);
        void Render(IRenderView view, Box2 clipBox, float dt, FontRenderer fontRenderer);
        void RenderStart(IRenderView view, Vector2 value);
        void RenderEnd(IRenderView view);

        #endregion Public Methods
    }
}