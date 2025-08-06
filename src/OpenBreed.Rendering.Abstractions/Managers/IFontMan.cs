using OpenTK;
using OpenTK.Mathematics;
using System;

namespace OpenBreed.Rendering.Abstractions.Managers
{
    public delegate void FontRenderCallback(IRenderView view, Box2 clipBox);

    public interface IFontMan
    {
        #region Public Methods

        IFontAtlas GetById(int id);

        IFontAtlasBuilder Create();

        IFontAtlas GetOSFont(string fontName, int fontSize);

        IFontAtlas GetGfxFont(string fontName);

        /// <summary>
        /// Unload all fonts from render context.
        /// </summary>
        /// <param name="renderContext">Render context</param>
        void UnloadAll(IRenderContext context);

        #endregion Public Methods
    }
}