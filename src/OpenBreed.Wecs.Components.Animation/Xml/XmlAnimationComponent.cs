using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Animation.Xml
{
    [XmlRoot("Animation")]
    public class XmlAnimationComponent : XmlComponentTemplate, IAnimationComponentTemplate
    {
        #region Public Properties

        [XmlElement("Speed")]
        public float Speed { get; set; }

        [XmlElement("Loop")]
        public bool Loop { get; set; }

        [XmlElement("AnimName")]
        public string AnimName { get; set; }

        #endregion Public Properties
    }
}