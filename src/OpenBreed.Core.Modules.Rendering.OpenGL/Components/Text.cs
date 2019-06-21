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

        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IEntity entity)
        {
        }

        #endregion Public Methods
    }
}