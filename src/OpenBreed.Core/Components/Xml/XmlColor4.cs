using System.Xml.Serialization;

namespace OpenBreed.Core.Components.Xml
{
    public class XmlColor4
    {
        #region Public Properties

        [XmlAttribute("R")]
        public float R { get; set; }

        [XmlAttribute("G")]
        public float G { get; set; }

        [XmlAttribute("B")]
        public float B { get; set; }

        [XmlAttribute("A")]
        public float A { get; set; }

        #endregion Public Properties
    }
}