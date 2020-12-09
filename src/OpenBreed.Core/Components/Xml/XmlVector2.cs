using System.Xml.Serialization;

namespace OpenBreed.Core.Components.Xml
{
    public class XmlVector2
    {
        #region Public Properties

        [XmlAttribute("X")]
        public float X { get; set; }

        [XmlAttribute("Y")]
        public float Y { get; set; }

        #endregion Public Properties
    }
}