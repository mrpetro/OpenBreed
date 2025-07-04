using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.Abstractions.Managers
{
    public delegate void FontRenderer(IRenderView view, Box2 clipBox);

    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFontAtlasBuilder Create();

        void RenderPart(IRenderView view, int fontId, string text, Vector2 origin, Color4 color, float order, Box2 clipBox, bool ignoreScale = false);

        void RenderAppend(IRenderView view, int fontId, string text, Box2 clipBox, Vector2 value, bool ignoreScale = false);

        IFont GetOSFont(string fontName, int fontSize);

        IFont GetGfxFont(string fontName);
        void Render(IRenderView view, Box2 clipBox, FontRenderer fontRenderer);
        void RenderStart(IRenderView view, Vector2 value);
        void RenderEnd(IRenderView view);

        /// <summary>
        /// Unload all fonts from render context.
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void UnloadAll(IRenderContext context);

        /// <summary>
        /// Load all fonts to render context.
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void LoadRefresh(IRenderContext context);

        #endregion Public Methods
    }
}