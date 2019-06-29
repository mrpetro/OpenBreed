using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
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

        internal Text(int fontId, Vector2 offset, string value)
        {
            FontId = fontId;
            Offset = offset;
            Value = value;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of text font
        /// </summary>
        public int FontId { get; set; }

        /// <summary>
        /// Offset position of text
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Actual text of this component
        /// </summary>
        public string Value { get; set; }

        #endregion Public Properties
    }
}