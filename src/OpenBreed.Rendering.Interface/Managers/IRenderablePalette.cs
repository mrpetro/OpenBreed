using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface IRenderablePalette
    {
        #region Public Methods

        public void SetPalette(IPalette palette);

        public IPalette CurrentPalette { get; }

        #endregion Public Methods
    }
}