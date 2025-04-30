using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Windowing.Common;

namespace OpenBreed.Rendering.Abstractions.Factories
{
    public interface IRenderContextFactory
    {
        #region Public Methods

        void SetupScope(HostCoordinateSystemConverter hostCoordinateSystemConverter, IGraphicsContext graphicsContext);

        IRenderContext CreateContext();

        #endregion Public Methods
    }
}