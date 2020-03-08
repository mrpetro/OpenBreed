using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Rendering.Builders;
using OpenTK.Graphics;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    /// <summary>
    /// Viewport component as display for cameras
    /// Related systems:
    /// - ViewportSystem
    /// </summary>
    public class ViewportComponent : IEntityComponent
    {
        #region Public Constructors

        /// <summary>
        /// Constructor for builder
        /// </summary>
        /// <param name="builder"></param>
        internal ViewportComponent(ViewportComponentBuilder builder)
        {
            Width = builder.Width;
            Height = builder.Height;
            CameraEntityId = -1;
            DrawBorder = builder.DrawBorder;
            DrawBackgroud = builder.DrawBackground;
            BackgroundColor = builder.BackgroundColor;
            Clipping = builder.Clipping;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Width of this viewport
        /// </summary>
        public float Width { get; internal set; }

        /// <summary>
        /// Height of this viewport
        /// </summary>
        public float Height { get; internal set; }

        /// <summary>
        ///  Flag to draw border box of viewport
        /// </summary>
        public bool DrawBorder { get; set; }

        /// <summary>
        /// Order of drawing, higher value object is rendered on top of lower value objects
        /// </summary>
        public float Order { get; set; }

        /// <summary>
        /// Flag to clip any graphics that is outside of viewport box
        /// </summary>
        public bool Clipping { get; set; }

        /// <summary>
        /// Viewport background color
        /// </summary>
        public Color4 BackgroundColor { get; set; }

        /// <summary>
        /// Draw viewport background if this flag is true
        /// </summary>
        public bool DrawBackgroud { get; set; }

        /// <summary>
        /// Aspect ratio of this viewport
        /// </summary>
        public float Ratio { get { return Width / Height; } }

        /// <summary>
        /// Camera entity ID which FOV(Field of view) is being rendered to this viewport
        /// </summary>
        public int CameraEntityId { get; set; }

        #endregion Public Properties
    }
}