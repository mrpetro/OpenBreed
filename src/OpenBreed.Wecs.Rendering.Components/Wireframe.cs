using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Components;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Rendering.Components
{
    /// <summary>
    /// Wireframe render component implementation
    /// </summary>
    internal class Wireframe : IEntityComponent
    {
        #region Internal Constructors

        internal Wireframe(float thickness, Color4<Rgba> color)
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
        public Color4<Rgba> Color { get; set; }

        #endregion Public Properties
    }
}