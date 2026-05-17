using Microsoft.Extensions.DependencyInjection;
using OpenBreed.Common;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Components.Xml;
using OpenBreed.Wecs.Core.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Rendering.Components.Xml
{
    public class XmlTileDataTemplate : ITileDataTemplate
    {
        #region Public Properties

        [XmlElement("AtlasName")]
        public string AtlasName { get; set; }

        [XmlElement("ImageIndex")]
        public int ImageIndex { get; set; }

        [XmlIgnore]
        public Vector2 Position
        {
            get => new Vector2(XmlPosition.X, XmlPosition.Y);
            set => XmlPosition = new XmlVector2() { X = value.X, Y = value.Y };
        }

        [XmlElement("Position")]
        public XmlVector2 XmlPosition { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("TilePutter")]
    public class XmlTilePutterComponent : XmlComponentTemplate, ITilePutterComponentTemplate
    {
        #region Public Properties

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<XmlTileDataTemplate> XmlItems { get; set; }

        [XmlIgnore]
        public IEnumerable<ITileDataTemplate> Items => XmlItems;

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var builderFactory = serviceProvider.GetRequiredService<IBuilderFactory>();

            var tilePutterBuilder = builderFactory.GetBuilder<TilePutterComponentBuilder>();

            var dataBuilder = tilePutterBuilder.CreateData();

            foreach (var item in Items)
            {
                dataBuilder.SetAtlasByName(item.AtlasName);
                dataBuilder.SetImageIndex(item.ImageIndex);
                dataBuilder.SetPosition(item.Position);

                tilePutterBuilder.AddData(dataBuilder.Build());
            }

            return tilePutterBuilder.Build();
        }

        #endregion Public Methods
    }
}