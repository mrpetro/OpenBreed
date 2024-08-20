using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems
{
    public interface IRenderableSystem
    {
        #region Public Methods

        public void Render(IWorldRenderContext context);

        #endregion Public Methods
    }
}