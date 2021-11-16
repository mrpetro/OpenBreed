using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("Camera")]
    public class XmlCameraComponent : XmlComponentTemplate, ICameraComponentTemplate
    {
        #region Public Properties

        public float Width { get; set; }
        public float Height { get; set; }
        public float Brightness { get; set; }

        #endregion Public Properties
    }
}