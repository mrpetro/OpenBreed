using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Core.Categories;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(TextDataComponent),
        typeof(TextPresentationComponent),
        typeof(PositionComponent))]
    [SystemCategory(CommonCategories.Rendering)]
    public class TextPresenterSystem : IMatchingSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Public Constructors

        public TextPresenterSystem(
            IFontMan fontMan)
        {
            this.fontMan = fontMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Abstractions.Primitives.IWorldRenderContext context)
        {
            context.View.Context.FontRenderer.Render(context.View, context.ViewBox, (view, clipBox) => RenderTexts(context, view, clipBox));
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderTexts(IWorldRenderContext context, OpenBreed.Rendering.Abstractions.IRenderView view, Box2 clipBox)
        {
            var entities = context.World.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                RenderText(view, entity, clipBox);
            }
        }

        private void RenderText(OpenBreed.Rendering.Abstractions.IRenderView view, IEntity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var tp = entity.Get<TextPresentationComponent>();
            var td = entity.Get<TextDataComponent>();

            view.Context.FontRenderer.RenderAppend(view, tp.FontId, td.Data, clipBox, pos.Value);
        }

        #endregion Private Methods
    }
}