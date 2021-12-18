using OpenTK;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFontAtlasBuilder Create();

        void Render(int fontId, string text, Vector2 origin, float order, Box2 clipBox);

        IFont GetOSFont(string fontName, int fontSize);
        IFont GetGfxFont(string fontName);

        #endregion Public Methods
    }
}