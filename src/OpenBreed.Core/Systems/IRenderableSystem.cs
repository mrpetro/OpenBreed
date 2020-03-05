using OpenTK;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// System that state will be rendered to parricular viewport during core render phase
    /// </summary>
    public interface IRenderableSystem : IWorldSystem
    {
        #region Public Methods

        void Render(Box2 viewBox, int depth, float dt);

        #endregion Public Methods
    }
}