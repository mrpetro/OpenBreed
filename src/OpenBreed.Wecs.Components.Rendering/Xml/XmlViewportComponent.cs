using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK.Graphics;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("Viewport")]
    public class XmlViewportComponent : XmlComponentTemplate, IViewportComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public Color4 BackgroundColor
        {
            get => new Color4(XmlBackgroundColor.R, XmlBackgroundColor.G, XmlBackgroundColor.B, XmlBackgroundColor.A);
            set => XmlBackgroundColor = new XmlColor4() { R = value.R, G = value.G, B = value.B, A = value.A };
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
    }
}