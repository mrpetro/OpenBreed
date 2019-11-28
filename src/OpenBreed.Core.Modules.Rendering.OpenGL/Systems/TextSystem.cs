﻿using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Messages;
using OpenBreed.Core.Modules.Animation.Systems;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextSystem : WorldSystem, ITextSystem, IMsgListener
    {
        #region Private Fields

        private MsgHandler msgHandler;
        private readonly List<int> entities = new List<int>();
        private readonly List<ITextComponent> textComps = new List<ITextComponent>();
        private readonly List<Position> positionComps = new List<Position>();

        #endregion Private Fields

        #region Public Constructors

        public TextSystem(ICore core) : base(core)
        {
            msgHandler = new MsgHandler(this);

            Require<ITextComponent>();
            Require<Position>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.MessageBus.RegisterHandler(TextSetMsg.TYPE, msgHandler);
        }

        /// <summary>
        /// Draw all texts to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            msgHandler.PostEnqueued();

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

        public override bool RecieveMsg(object sender, IMsg message)
        {
            switch (message.Type)
            {
                case TextSetMsg.TYPE:
                    return HandleTextSetMsg(sender, (TextSetMsg)message);

                default:
                    return false;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity.Id);
            textComps.Add(entity.Components.OfType<ITextComponent>().First());
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

        private bool HandleTextSetMsg(object sender, TextSetMsg message)
        {
            var index = entities.IndexOf(message.EntityId);
            if (index < 0)
                return false;

            textComps[index].Value = message.Text;

            return true;
        }

        public bool EnqueueMsg(object sender, IEntityMsg msg)
        {
            return false;
        }

        #endregion Private Methods
    }
}