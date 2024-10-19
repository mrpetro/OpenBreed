using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Attributes;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Components.Rendering
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
    [ComponentName("Camera")]
    public class CameraComponent : IEntityComponent
    {
        #region Internal Constructors

        internal CameraComponent(CameraComponentBuilder builder)
        {
            Size = builder.Size;
            Brightness = builder.Brightness;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Width and Height of camera view
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Brightness value of camera
        /// </summary>
        public float Brightness { get; set; }

        #endregion Public Properties
    }

    public sealed class CameraComponentFactory : ComponentFactoryBase<ICameraComponentTemplate>
    {
        #region Private Fields

        private readonly IBuilderFactory builderFactory;

        #endregion Private Fields

        #region Internal Constructors

        public CameraComponentFactory(IBuilderFactory builderFactory)
        {
            this.builderFactory = builderFactory;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(ICameraComponentTemplate template)
        {
            var builder = builderFactory.GetBuilder<CameraComponentBuilder>();
            builder.SetSize(template.Width, template.Height);
            builder.SetBrightness(template.Brightness);
            return builder.Build();
        }

        #endregion Protected Methods
    }

    public class CameraComponentBuilder : IBuilder<CameraComponent>
    {
        #region Internal Fields

        internal Vector2 Size;
        internal float Brightness = 1.0f;

        #endregion Internal Fields

        #region Internal Constructors

        internal CameraComponentBuilder()
        {
        }

        #endregion Internal Constructors

        #region Public Methods

        public CameraComponent Build()
        {
            return new CameraComponent(this);
        }

        public void SetSize(float width, float height)
        {
            Size = new Vector2(width, height);
        }

        public void SetBrightness(float brightness)
        {
            Brightness = brightness;
        }

        #endregion Public Methods
    }
}