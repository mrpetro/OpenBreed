using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class RenderableFactory : IRenderableFactory
    {
        #region Private Fields

        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Public Constructors

        public RenderableFactory(IPrimitiveRenderer primitiveRenderer)
        {
            this.primitiveRenderer = primitiveRenderer;
        }

        #endregion Public Constructors

        #region Public Methods

        #endregion Public Methods
    }
}