﻿using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface.Managers;
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
    public class TextSystem : MatchingSystemBase<TextSystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TextSystem(
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

        public void Render(Worlds.IWorldRenderContext context)
        {
            fontMan.Render(context.View, context.ViewBox, context.Dt, RenderTexts);
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderTexts(OpenBreed.Rendering.Interface.Managers.IRenderView view, Box2 clipBox, float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                RenderText(view, entities[i], clipBox);
        }

        private void RenderText(OpenBreed.Rendering.Interface.Managers.IRenderView view, IEntity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var tcp = entity.Get<TextComponent>();

            fontMan.RenderStart(view, pos.Value);

            try
            {
                for (int i = 0; i < tcp.Parts.Count; i++)
                {
                    var part = tcp.Parts[i];
                    fontMan.RenderPart(view, part.FontId, part.Text, part.Offset, part.Color, part.Order, clipBox);
                }
            }
            finally
            {
                fontMan.RenderEnd(view);
            }
        }

        #endregion Private Methods
    }
}