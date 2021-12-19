using OpenTK;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface IRenderable
    {
        #region Public Methods

        public void Render(Box2 viewBox, int depth, float dt);

        #endregion Public Methods
    }

    public interface IRenderableBatch
    {
        #region Public Methods

        public void Render(Box2 viewBox, int depth, float dt);
        void Add(IRenderable renderable);

        #endregion Public Methods
    }
}