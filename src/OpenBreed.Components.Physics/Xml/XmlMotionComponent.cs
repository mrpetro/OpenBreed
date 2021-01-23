using OpenBreed.Components.Common.Xml;
using OpenBreed.Ecsw.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Components.Physics.Xml
{
    [XmlRoot("Motion")]
    public class XmlMotionComponent : XmlComponentTemplate, IMotionComponentTemplate
    {
    }
}