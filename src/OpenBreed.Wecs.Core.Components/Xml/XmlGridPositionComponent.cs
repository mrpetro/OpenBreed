using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Core.Components.Xml
{
    [XmlRoot("GridPosition")]
    public class XmlGridPositionComponent : XmlComponentTemplate, IGridPositionComponentTemplate
    {
        #region Public Properties

        [XmlAttribute("X")]
        public int X { get; set; }

        [XmlAttribute("Y")]
        public int Y { get; set; }

        #endregion Public Properties
    }
}