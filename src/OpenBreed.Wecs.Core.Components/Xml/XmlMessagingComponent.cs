using OpenBreed.Wecs.Components.Xml;
using System;
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

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            return new MessagingComponent();
        }

        #endregion Public Methods
    }
}