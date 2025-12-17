namespace OpenBreed.Wecs.Abstractions.Systems
{
    public interface IRenderableSystem : ISystem
    {
        #region Public Methods

        public void Render(IWorldRenderContext context);

        #endregion Public Methods
    }
}