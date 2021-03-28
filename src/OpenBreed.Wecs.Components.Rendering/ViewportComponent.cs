using OpenBreed.Core;
using OpenTK.Graphics;

namespace OpenBreed.Wecs.Components.Rendering
{
    /// <summary>
    /// Types of scaling for camera display in viewports
    /// </summary>
    public enum ViewportScalingType
    {
        None,
        FitWidthPreserveAspectRatio,
        FitHeightPreserveAspectRatio,
        FitBothPreserveAspectRatio,
        FitBothIgnoreAspectRatio
    }

    /// <summary>
    /// Viewport component as display for cameras
    /// Related systems:
    /// - ViewportSystem
    /// </summary>
    public class ViewportComponent : IEntityComponent
    {
        #region Internal Constructors

        /// <summary>
        /// Constructor for builder
        /// </summary>
        /// <param name="builder"></param>
        internal ViewportComponent(ViewportComponentBuilderEx builder)
        {
            Width = builder.Width;
            Height = builder.Height;
            CameraEntityId = -1;
            DrawBorder = builder.DrawBorder;
            DrawBackgroud = builder.DrawBackground;
            BackgroundColor = builder.BackgroundColor;
            Clipping = builder.Clipping;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Width of this viewport
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Height of this viewport
        /// </summary>
        public float Height { get; set; }

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
        /// Type of scaling strategy for camera display
        /// </summary>
        public ViewportScalingType ScalingType { get; set; }

        /// <summary>
        /// Camera entity ID which display is being rendered to this viewport
        /// </summary>
        public int CameraEntityId { get; set; }

        #endregion Public Properties
    }

    public class ViewportComponentBuilderEx
    {
        #region Internal Fields

        internal float Width;

        internal float Height;

        internal int CameraEntityId = -1;

        internal bool DrawBorder = false;

        internal bool DrawBackground = false;

        internal Color4 BackgroundColor;

        internal bool Clipping = false;

        #endregion Internal Fields

        #region Private Constructors

        private ViewportComponentBuilderEx()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        public static ViewportComponentBuilderEx New()
        {
            return new ViewportComponentBuilderEx();
        }

        public ViewportComponent Build()
        {
            return new ViewportComponent(this);
        }

        public void SetSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public void SetBackgroundColor(Color4 value)
        {
            BackgroundColor = value;
        }

        public void SetDrawBorderFlag(bool value)
        {
            DrawBorder = value;
        }

        public void SetDrawBackgroundFlag(bool value)
        {
            DrawBackground = value;
        }

        public void SetClippingFlag(bool value)
        {
            Clipping = value;
        }

        #endregion Public Methods
    }
}