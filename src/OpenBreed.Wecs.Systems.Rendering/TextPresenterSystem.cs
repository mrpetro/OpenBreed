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
        typeof(TextDataComponent),
        typeof(TextPresentationComponent),
        typeof(PositionComponent))]
    public class TextPresenterSystem : MatchingSystemBase<TextPresenterSystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TextPresenterSystem(
            IFontMan fontMan)
        {
            this.fontMan = fontMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(IRenderContext context)
        {
            fontMan.Render(context.ViewBox, context.Dt, RenderTexts);
        }

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
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

        private void RenderText(IEntity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var tp = entity.Get<TextPresentationComponent>();
            var td = entity.Get<TextDataComponent>();

            fontMan.RenderAppend(tp.FontId, td.Data, clipBox, pos.Value);
        }

        #endregion Private Methods
    }
}