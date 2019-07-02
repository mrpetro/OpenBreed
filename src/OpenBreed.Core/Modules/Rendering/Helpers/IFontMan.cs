namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFont Create(string fontName, int fontSize);

        #endregion Public Methods
    }
}