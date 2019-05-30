using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK.Graphics.OpenGL;
using System;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    internal class Text : IText
    {
        #region Private Fields

        private Position position;
        private IFont font;

        #endregion Private Fields

        #region Public Constructors

        internal Text(IFont font, string value)
        {
            this.font = font;
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Value { get; set; }
        public Type SystemType { get { return typeof(RenderSystem); } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Draw(IViewport viewport)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();

            GL.Translate(position.Current.X, position.Current.Y, 0.0f);

            font.Draw(Value);

            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().FirstOrDefault();
        }

        #endregion Public Methods
    }
}