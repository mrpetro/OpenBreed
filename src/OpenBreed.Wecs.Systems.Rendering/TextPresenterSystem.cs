using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    [RequireEntityWith(
        typeof(TextDataComponent),
        typeof(TextPresentationComponent),
        typeof(PositionComponent))]
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

        //TODO: Remove me
        public bool ContainsEntity(IEntity entity)
        {
            return false;

            //throw new System.NotImplementedException();
        }

        //TODO: Remove me
        public void OnAddEntity(IWorld world, IEntity entity)
        {
            //throw new System.NotImplementedException();
        }

        //TODO: Remove me
        public void OnRemoveEntity(IWorld world, IEntity entity)
        {
            //throw new System.NotImplementedException();
        }

        public void Render(Worlds.IWorldRenderContext context)
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