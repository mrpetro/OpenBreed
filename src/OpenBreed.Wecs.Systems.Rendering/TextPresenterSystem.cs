using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TextPresenterSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IFontMan fontMan;

        #endregion Private Fields

        #region Internal Constructors

        internal TextPresenterSystem(
            IWorld world,
            IFontMan fontMan) :
            base(world)
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

        protected override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        protected override void OnAddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(IEntity entity)
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