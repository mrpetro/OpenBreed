using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
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
        typeof(TextComponent),
        typeof(PositionComponent))]
    public class TextSystem : IMatchingSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        public TextSystem(
            IEntityMan entityMan,
            IFontMan fontMan,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.fontMan = fontMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

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
            context.View.Context.FontRenderer.Render(context.View, context.ViewBox, (view, viewBox) => RenderTexts(context, view, viewBox));
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderTexts(Worlds.IWorldRenderContext context, OpenBreed.Rendering.Abstractions.IRenderView view, Box2 clipBox)
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
            var tcp = entity.Get<TextComponent>();

            view.Context.FontRenderer.RenderStart(view, pos.Value);

            try
            {
                for (int i = 0; i < tcp.Parts.Count; i++)
                {
                    var part = tcp.Parts[i];
                    view.Context.FontRenderer.RenderPart(view, part.FontId, part.Text, part.Offset, part.Color, part.Order, clipBox);
                }
            }
            finally
            {
                view.Context.FontRenderer.RenderEnd(view);
            }
        }

        #endregion Private Methods
    }
}