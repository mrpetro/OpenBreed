using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
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