using OpenBreed.Wecs.Components;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Rendering
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