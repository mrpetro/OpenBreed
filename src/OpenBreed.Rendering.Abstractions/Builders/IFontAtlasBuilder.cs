namespace OpenBreed.Rendering.Abstractions
{
    public interface IFontAtlasBuilder
    {
        #region Public Methods

        IFontAtlasBuilder SetSpriteAtlas(string spriteAtlasName);

        IFontAtlasBuilder MapCharacterToSpriteId(int ch, int spriteIndex, float width = 0.0f, float xOffset = 0.0f);

        //IFontAtlasBuilder AddWhiteChar(int ch, float width);

        IFontAtlasBuilder SetHeight(float height);

        IFontAtlasBuilder SetName(string fontName);
        IFontAtlasBuilder SetAlias(string alias);

        IFontAtlas Build();

        #endregion Public Methods
    }
}