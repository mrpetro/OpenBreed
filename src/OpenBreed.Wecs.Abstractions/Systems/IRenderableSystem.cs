namespace OpenBreed.Wecs.Abstractions.Systems
{
    /// <summary>
    /// System that renders entities.
    /// </summary>
    public interface IRenderableSystem : ISystem
    {
        #region Public Methods

        /// <summary>
        /// Render all entities in this system using given render context.
        /// </summary>
        /// <param name="entities">Entities to render.</param>
        /// <param name="context">World render context.</param>
        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context);

        #endregion Public Methods
    }
}