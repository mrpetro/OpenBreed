using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Common.Logging;

namespace OpenBreed.Wecs.Systems.Rendering
{
    public class TextSystem : SystemBase, IRenderableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IFontMan fontMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TextSystem(IEntityMan entityMan, IFontMan fontMan, ILogger logger )
        {
            this.entityMan = entityMan;
            this.fontMan = fontMan;
            this.logger = logger;

            RequireEntityWith<TextComponent>();
            RequireEntityWith<PositionComponent>();

            RegisterHandler<TextSetCommand>(HandleTextSetCommand);
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(Box2 clipBox, int depth, float dt)
        {
            ExecuteCommands();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                RenderText(entities[i], clipBox);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        #endregion Public Methods

        #region Protected Methods

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
                GL.Translate(part.Offset.X, part.Offset.Y, 0.0f);
                fontMan.GetById(part.FontId).Draw(part.Text);
            }

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private bool HandleTextSetCommand(TextSetCommand cmd)
        {
            var toModify = entityMan.GetById(cmd.EntityId);
            if (toModify == null)
                return false;

            var text = toModify.Get<TextComponent>();

            if (cmd.PartId < 0 || cmd.PartId >= text.Parts.Count)
            {
                logger.Error($"Unknown text part ID({cmd.PartId}) to modify.");
            }

            text.Parts[cmd.PartId].Text = cmd.Text;

            return true;
        }

        #endregion Private Methods
    }
}