using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Windowing.Common;

namespace OpenBreed.Rendering.Interface.Factories
{
    public interface IRenderContextFactory
    {
        #region Public Methods

        void SetupScope(HostCoordinateSystemConverter hostCoordinateSystemConverter, IGraphicsContext graphicsContext);

        IRenderContext CreateContext();

        #endregion Public Methods
    }
}