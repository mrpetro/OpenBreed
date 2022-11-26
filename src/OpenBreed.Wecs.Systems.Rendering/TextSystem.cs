using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
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
    public class TextSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TextSystem(IEntityMan entityMan, IFontMan fontMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.fontMan = fontMan;
            this.logger = logger;

            RequireEntityWith<TextComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            world.GetModule<IRenderableBatch>().Add(this);
        }

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
            var tcp = entity.Get<TextComponent>();

            fontMan.RenderStart(pos.Value);

            try
            {
                for (int i = 0; i < tcp.Parts.Count; i++)
                {
                    var part = tcp.Parts[i];
                    fontMan.RenderPart(part.FontId, part.Text, part.Offset, part.Color, part.Order, clipBox);
                }
            }
            finally
            {
                fontMan.RenderEnd();
            }
        }

        #endregion Private Methods
    }
}