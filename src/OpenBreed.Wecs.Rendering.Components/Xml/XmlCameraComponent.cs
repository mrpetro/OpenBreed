using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Components.Xml;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Rendering.Components.Xml
{
    [XmlRoot("Camera")]
    public class XmlCameraComponent : XmlComponentTemplate, ICameraComponentTemplate
    {
        #region Public Properties

        public float Width { get; set; }
        public float Height { get; set; }
        public float Brightness { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var builderFactory = serviceProvider.GetRequiredService<IBuilderFactory>();

            var builder = builderFactory.GetBuilder<CameraComponentBuilder>();
            builder.SetSize(Width, Height);
            builder.SetBrightness(Brightness);
            return builder.Build();
        }

        #endregion Public Methods
    }
}