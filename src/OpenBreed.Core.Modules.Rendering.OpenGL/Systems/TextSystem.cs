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
using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextSystem : WorldSystem, ICommandExecutor, ICameraSystem
    {
        #region Private Fields

        private CommandHandler cmdHandler;
        private readonly List<IEntity> entities = new List<IEntity>();

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
        /// Render this system using given viewport component and time step
        /// </summary>
        /// <param name="viewport">Rendered viewport</param>
        /// <param name="dt">Time step</param>
        public void Render(IEntity viewport, float dt)
        {
        }

        public void Render(float left, float bottom, float right, float top, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawText(entities[i]);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        /// <summary>
        /// Draw all texts to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            Render(left, bottom, right, top, dt);
        }

        public void DrawText(IEntity entity)
        {
            var pos = entity.GetComponent<Position>();
            var text = entity.GetComponent<TextComponent>();

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(pos.Value.X, pos.Value.Y, text.Order);

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
            entities.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private bool HandleTextSetCommand(object sender, TextSetCommand cmd)
        {
            var toModify = entities.FirstOrDefault(item => item.Id == cmd.EntityId);
            if (toModify == null)
                return false;

            var text = toModify.GetComponent<TextComponent>();
            text.Value = cmd.Text;

            return true;
        }

        public bool EnqueueMsg(object sender, IEntityCommand msg)
        {
            return false;
        }

        #endregion Private Methods
    }
}