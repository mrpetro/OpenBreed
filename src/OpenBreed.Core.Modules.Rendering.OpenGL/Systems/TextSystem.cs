using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextSystem : WorldSystemEx, IRenderableSystem
    {
        #region Private Fields

        private List<IEntity> entities;

        #endregion Private Fields

        #region Public Constructors

        public TextSystem(ICore core) : base(core)
        {
            entities = new List<IEntity>();
            Require<Text>();
            Require<Position>();
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw all texts to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < entities.Count; i++)
                DrawText(viewport, entities[i]);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawText(IViewport viewport, IEntity entity)
        {
            var text = entity.Components.OfType<IText>().First();
            var position = entity.Components.OfType<Position>().First();

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(position.Value.X, position.Value.Y, 0.0f);

            Core.Rendering.Fonts.GetById(text.FontId).Draw(text.Value);

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Public Methods
    }
}