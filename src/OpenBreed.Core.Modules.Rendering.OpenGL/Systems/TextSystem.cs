using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Common;

using OpenBreed.Core.Commands;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Builders;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextSystem : WorldSystem, ITextSystem, ICommandExecutor
    {
        #region Private Fields

        private CommandHandler cmdHandler;
        private readonly List<int> entities = new List<int>();
        private readonly List<TextComponent> textComps = new List<TextComponent>();
        private readonly List<Position> positionComps = new List<Position>();

        #endregion Private Fields

        #region Public Constructors

        internal TextSystem(TextSystemBuilder builder) : base(builder.core)
        {
            cmdHandler = new CommandHandler(this);

            Require<TextComponent>();
            Require<Position>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(TextSetCommand.TYPE, cmdHandler);
        }

        /// <summary>
        /// Draw all texts to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawEntityText(viewport, i);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawEntityText(IViewport viewport, int index)
        {
            var text = textComps[index];
            var position = positionComps[index];

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(position.Value.X, position.Value.Y, text.Order);

            GL.Translate(text.Offset.X, text.Offset.Y, 0.0f);

            Core.Rendering.Fonts.GetById(text.FontId).Draw(text.Value);

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case TextSetCommand.TYPE:
                    return HandleTextSetCommand(sender, (TextSetCommand)cmd);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity.Id);
            textComps.Add(entity.Components.OfType<TextComponent>().First());
            positionComps.Add(entity.Components.OfType<Position>().First());
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            var index = entities.IndexOf(entity.Id);

            if (index < 0)
                throw new InvalidOperationException("Entity not found in this system.");

            entities.RemoveAt(index);
            textComps.RemoveAt(index);
            positionComps.RemoveAt(index);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTextSetCommand(object sender, TextSetCommand cmd)
        {
            var entity = Core.Entities.GetById(cmd.EntityId);

            var index = entities.IndexOf(cmd.EntityId);
            if (index < 0)
                return false;

            textComps[index].Value = cmd.Text;

            return true;
        }

        public bool EnqueueMsg(object sender, IEntityCommand msg)
        {
            return false;
        }

        #endregion Private Methods
    }
}