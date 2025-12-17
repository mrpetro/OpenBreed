using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(TileGridComponent))]
    public class TileRenderSystem : IMatchingSystem, IRenderableSystem
    {
        #region Public Constructors

        public TileRenderSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IWorldRenderContext context)
        {
            var entities = context.World.GetMatchingEntities(this);

            foreach (var item in entities)
            {
                RenderEntity(item, context);
            }
        }

        public void RenderEntity(IEntity entity, IWorldRenderContext context)
        {
            var tgc = entity.Get<TileGridComponent>();
            tgc.Grid.Render(context.View, context.ViewBox);
        }

        #endregion Public Methods
    }
}