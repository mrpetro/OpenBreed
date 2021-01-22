using OpenBreed.Components.Common.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Components.Animation.Xml
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

        [XmlElement("Transition")]
        public string Transition { get; set; }

        #endregion Public Properties
    }
}