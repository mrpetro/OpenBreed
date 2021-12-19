using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class RenderableBatch : IRenderableBatch
    {
        #region Private Fields

        private readonly List<IRenderable> renderables = new List<IRenderable>();

        #endregion Private Fields

        #region Public Methods

        public void Add(IRenderable renderable)
        {
            renderables.Add(renderable);
        }

        public void Render(Box2 viewBox, int depth, float dt)
        {
            renderables.ForEach(renderable => renderable.Render(viewBox, depth, dt));
        }

        #endregion Public Methods
    }

    internal class RenderableFactory : IRenderableFactory
    {
        #region Public Methods

        public IRenderableBatch CreateRenderableBatch() => new RenderableBatch();

        #endregion Public Methods
    }
}