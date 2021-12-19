using OpenTK;
using System;

namespace OpenBreed.Rendering.Interface.Managers
{
    public delegate void FontRenderer(Box2 clipBox, float dt);

    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFontAtlasBuilder Create();

        void Render(int fontId, string text, Vector2 origin, float order, Box2 clipBox);

        void RenderAppend(int fontId, string text, Box2 clipBox, Vector2 value);

        IFont GetOSFont(string fontName, int fontSize);

        IFont GetGfxFont(string fontName);
        void Render(Box2 clipBox, float dt, FontRenderer fontRenderer);

        #endregion Public Methods
    }
}