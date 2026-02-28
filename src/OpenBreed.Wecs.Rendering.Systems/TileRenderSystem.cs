using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Rendering.Systems
{
    [RequireEntityWith(
        typeof(TileGridComponent))]
    [SystemCategory(CommonCategories.Rendering)]
    public class TileRenderSystem : IRenderableSystem
    {
        #region Public Constructors

        public TileRenderSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
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