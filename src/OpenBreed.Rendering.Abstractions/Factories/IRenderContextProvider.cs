using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Windowing.Common;

namespace OpenBreed.Rendering.Abstractions.Factories
{
    public interface IRenderContextProvider
    {
        #region Public Methods

        void SetupScope(HostCoordinateSystemConverter hostCoordinateSystemConverter, IGraphicsContext graphicsContext);

        IRenderContext GetContext();

        #endregion Public Methods
    }
}