﻿using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TextSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
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
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                RenderText(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
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

        private void RenderText(Entity entity, Box2 clipBox)
        {
            var pos = entity.Get<PositionComponent>();
            var tcp = entity.Get<TextComponent>();

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(pos.Value.X, pos.Value.Y, 0.0f);

            for (int i = 0; i < tcp.Parts.Count; i++)
            {
                var part = tcp.Parts[i];

                fontMan.Render(part.FontId, part.Text, part.Offset, 100, clipBox);

            }

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Private Methods
    }
}