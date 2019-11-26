using OpenBreed.Core.Modules.Rendering.Helpers;

namespace OpenBreed.Core.Managers
{
    public interface IFontMan
    {
        #region Public Methods

        IFont GetById(int id);

        IFont Create(string fontName, int fontSize);

        #endregion Public Methods
    }
}