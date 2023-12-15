using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface IRenderable
    {
        #region Public Methods

        public void Render(Box2 viewBox, int depth, float dt);

        #endregion Public Methods
    }
}