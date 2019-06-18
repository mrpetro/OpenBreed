using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class TextSystem : ITextSystem
    {
        #region Private Fields

        private List<IText> texts;

        #endregion Private Fields

        #region Public Constructors

        public TextSystem(ICore core)
        {
            Core = core;
            texts = new List<IText>();
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Draw all texts to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which sprites will be drawn to</param>
        public void Draw(Viewport viewport)
        {
            float left, bottom, right, top;
            viewport.GetVisibleRectangle(out left, out bottom, out right, out top);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.AlphaTest);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusConstantColor);
            GL.BlendColor(Color4.Black);
            //GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Texture2D);

            for (int i = 0; i < texts.Count; i++)
                DrawText(viewport, texts[i]);

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.AlphaTest);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawText(IViewport viewport, IText text)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(text.Position.Value.X, text.Position.Value.Y, 0.0f);

            Core.Rendering.Fonts.GetById(text.FontId).Draw(text.Value);

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public void AddText(IText text)
        {
            texts.Add(text);
        }

        public void Initialize(World world)
        {
            throw new NotImplementedException();
        }

        public void Deinitialize(World world)
        {
            throw new NotImplementedException();
        }

        public void Update(float dt)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}