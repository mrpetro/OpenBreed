﻿using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TextPresenterSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TextPresenterSystem(IFontMan fontMan)
        {
            this.fontMan = fontMan;

            RequireEntityWith<TextDataComponent>();
            RequireEntityWith<TextPresentationComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            fontMan.Render(clipBox, dt, RenderTexts);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RenderTexts(Box2 clipBox, float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                RenderText(entities[i], clipBox);
        }

        private void RenderText(Entity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var tp = entity.Get<TextPresentationComponent>();
            var td = entity.Get<TextDataComponent>();

            fontMan.RenderAppend(tp.FontId, td.Data, clipBox, pos.Value);
        }

        #endregion Private Methods
    }
}