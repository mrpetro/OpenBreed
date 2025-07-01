using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Windowing.Common;

namespace OpenBreed.Rendering.Abstractions.Factories
{
    public interface IRenderContextProvider
    {
        #region Public Methods

        IRenderContext GetContext(IGraphicsContext graphicsContext, HostCoordinateSystemConverter hostCoordinateSystemConverter);

        #endregion Public Methods
    }
}