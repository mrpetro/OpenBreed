using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class RenderablePalette : IRenderablePalette
    {
        #region Public Properties

        public IPalette CurrentPalette { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void SetPalette(IPalette palette)
        {
            CurrentPalette = palette;
        }

        #endregion Public Methods
    }
}