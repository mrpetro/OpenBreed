using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Common.Xml
{
    [XmlRoot("Messaging")]
    public class XmlMessagingComponent : XmlComponentTemplate, IMessagingComponentTemplate
    {
        #region Public Properties

        [XmlArray("Messages")]
        [XmlArrayItem(ElementName = "Message")]
        public int[] Messages { get; set; }

        #endregion Public Properties
    }
}