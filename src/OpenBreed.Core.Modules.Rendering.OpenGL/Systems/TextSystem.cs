using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Physics.Builders;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextSystem : WorldSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();

        #endregion Private Fields

        #region Internal Constructors

        internal TextSystem(TextSystemBuilder builder) : base(builder.core)
        {
            Require<TextComponent>();
            Require<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public static void RegisterHandlers(CommandsMan commands)
        {
            commands.Register<TextSetCommand>(HandleTextSetCommand);
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
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
                Core.Rendering.Fonts.GetById(part.FontId).Draw(part.Text);
            }

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private static bool HandleTextSetCommand(ICore core, TextSetCommand cmd)
        {
            var toModify = core.Entities.GetById(cmd.EntityId);
            if (toModify == null)
                return false;

            var text = toModify.Get<TextComponent>();

            if (cmd.PartId < 0 || cmd.PartId >= text.Parts.Count)
            {
                core.Logging.Error($"Unknown text part ID({cmd.PartId}) to modify.");
            }

            text.Parts[cmd.PartId].Text = cmd.Text;

            return true;
        }

        #endregion Private Methods
    }
}