using OpenBreed.Core.Common.Systems.Components;
using OpenTK.Graphics;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Wireframe render component implementation
    /// </summary>
    internal class Wireframe : IEntityComponent
    {
        #region Internal Constructors

        internal Wireframe(float thickness, Color4 color)
        {
            Thickness = thickness;
            Color = color;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Thickness of wireframe lines
        /// </summary>
        public float Thickness { get; set; }

        /// <summary>
        /// Color of wireframe lines
        /// </summary>
        public Color4 Color { get; set; }

        #endregion Public Properties
    }
}