using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Rendering.Abstractions.Data;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Core.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Rendering.Components.Xml
{
    [XmlRoot("Picture")]
    public class XmlPictureComponent : XmlComponentTemplate, IPictureComponentTemplate
    {
        #region Public Properties

        [XmlElement("ImageName")]
        public string ImageName { get; set; }

        [XmlIgnore]
        public Vector2 Origin
        {
            get => new Vector2(XmlOrigin.X, XmlOrigin.Y);
            set => XmlOrigin = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlIgnore]
        public Color4 Color
        {
            get => new Color4(XmlColor.R, XmlColor.G, XmlColor.B, XmlColor.A);
            set => XmlColor = new XmlColor4() { R = value.R, G = value.G, B = value.B, A = value.A };
        }

        [XmlElement("Origin")]
        public XmlVector2 XmlOrigin { get; set; }

        [XmlElement("Color")]
        public XmlColor4 XmlColor { get; set; }

        public int Order { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var dataLoaderFactory = serviceProvider.GetRequiredService<IDataLoaderFactory>();
            var builderFactory = serviceProvider.GetRequiredService<IBuilderFactory>();

            var pictureDataLoader = dataLoaderFactory.GetLoader<IPictureDataLoader>();

            pictureDataLoader.Load(ImageName);

            var builder = builderFactory.GetBuilder<PictureComponentBuilder>();
            builder.SetImageByName(ImageName);
            builder.SetOrigin(Origin);
            builder.SetOrder(Order);
            builder.SetColor(Color);
            return builder.Build();
        }

        #endregion Public Methods
    }
}