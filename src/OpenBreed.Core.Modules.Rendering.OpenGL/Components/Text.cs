using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
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

        #endregion Private Fields

        #region Internal Constructors

        internal Text(int fontId, string value)
        {
            FontId = fontId;
            Value = value;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of text font
        /// </summary>
        public int FontId { get; set; }

        public string Value { get; set; }

        public Type SystemType { get { return typeof(RenderSystem); } }

        /// <summary>
        /// Text position component reference
        /// </summary>
        public Position Position { get { return position; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Draw(IViewport viewport)
        {

        }

        public void Initialize(IEntity entity)
        {
            position = entity.Components.OfType<Position>().FirstOrDefault();
        }

        #endregion Public Methods
    }
}