using OpenBreed.Core.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Components.Physics.Xml
{
    [XmlRoot("Motion")]
    public class XmlMotionComponent : XmlComponentTemplate, IMotionComponentTemplate
    {
    }
}