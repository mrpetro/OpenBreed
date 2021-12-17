namespace OpenBreed.Rendering.Interface
{
    public interface IFontAtlasBuilder
    {
        #region Public Methods

        IFontAtlasBuilder AddCharacterFromSprite(int ch, string spriteAtlasName, int spriteIndex);

        IFontAtlasBuilder SetName(string fontName);

        IFont Build();

        #endregion Public Methods
    }
}