namespace OpenBreed.Rendering.Interface
{
    public interface IFontAtlasBuilder
    {
        #region Public Methods

        IFontAtlasBuilder AddCharacterFromSprite(int ch, string spriteAtlasName, int spriteIndex, float width = 0.0f);

        //IFontAtlasBuilder AddWhiteChar(int ch, float width);

        IFontAtlasBuilder SetHeight(float height);

        IFontAtlasBuilder SetName(string fontName);

        IFont Build();

        #endregion Public Methods
    }
}