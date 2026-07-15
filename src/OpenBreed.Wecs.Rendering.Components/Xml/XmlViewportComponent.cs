using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Core.Components.Xml;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Rendering.Components.Xml
{
    [XmlRoot("Viewport")]
    public class XmlViewportComponent : XmlComponentTemplate, IViewportComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public Color4<Rgba> BackgroundColor
        {
            get => new Color4<Rgba>(XmlBackgroundColor.R, XmlBackgroundColor.G, XmlBackgroundColor.B, XmlBackgroundColor.A);
            set => XmlBackgroundColor = new XmlColor4() { R = value.X, G = value.Y, B = value.Z, A = value.W };
        }

        public float Width { get; set; }
        public float Height { get; set; }
        public bool DrawBorder { get; set; }
        public bool Clipping { get; set; }
        public bool DrawBackgroud { get; set; }
        public ViewportScalingType ScalingType { get; set; }
        public float Order { get; set; }

        [XmlElement("BackgroundColor")]
        public XmlColor4 XmlBackgroundColor { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var builderFactory = serviceProvider.GetRequiredService<IBuilderFactory>();

            var builder = builderFactory.GetBuilder<ViewportComponentBuilder>();
            builder.SetBackgroundColor(BackgroundColor);
            builder.SetClippingFlag(Clipping);
            builder.SetDrawBackgroundFlag(DrawBackgroud);
            builder.SetDrawBorderFlag(DrawBorder);
            builder.SetSize(Width, Height);

            return builder.Build();
        }

        #endregion Public Methods
    }
}