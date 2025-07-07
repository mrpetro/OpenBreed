using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.Abstractions.Managers
{
    public delegate void FontRenderCallback(IRenderView view, Box2 clipBox);

    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFontAtlasBuilder Create();

        IFont GetOSFont(string fontName, int fontSize);

        IFont GetGfxFont(string fontName);

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