using OpenBreed.Core.Components;

namespace OpenBreed.Core.Modules.Rendering.Components
{
    public interface ICameraComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        float Width { get; set; }
        float Height { get; set; }
        float Brightness { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// Camera component as source of display for viewports
    /// Related systems:
    /// - ViewportSystem
    /// </summary>
    public class CameraComponent : IEntityComponent
    {
        #region Internal Constructors

        internal CameraComponent(CameraComponentBuilder builder)
        {
            Width = builder.Width;
            Height = builder.Height;
            Brightness = builder.Brightness;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Width value of camera
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Height value of camera
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Aspect ratio of this camera
        /// </summary>
        public float Ratio { get { return Width / Height; } }

        /// <summary>
        /// Brightness value of camera
        /// </summary>
        public float Brightness { get; set; }

        #endregion Public Properties
    }

    public sealed class CameraComponentFactory : ComponentFactoryBase<ICameraComponentTemplate>
    {
        #region Public Constructors

        public CameraComponentFactory(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ICameraComponentTemplate template)
        {
            var builder = CameraComponentBuilder.New(core);
            builder.SetSize(template.Width, template.Height);
            builder.SetBrightness(template.Brightness);
            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class CameraComponentBuilder
    {
        #region Internal Fields

        internal float Width;
        internal float Height;
        internal float Brightness = 1.0f;

        #endregion Internal Fields

        #region Private Fields

        private ICore core;

        #endregion Private Fields

        #region Private Constructors

        private CameraComponentBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Private Constructors

        #region Public Methods

        public static CameraComponentBuilder New(ICore core)
        {
            return new CameraComponentBuilder(core);
        }

        public CameraComponent Build()
        {
            return new CameraComponent(this);
        }

        public void SetSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public void SetBrightness(float brightness)
        {
            Brightness = brightness;
        }

        #endregion Public Methods
    }
}