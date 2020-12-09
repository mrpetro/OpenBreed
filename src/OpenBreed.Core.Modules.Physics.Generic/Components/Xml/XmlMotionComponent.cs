using OpenBreed.Core.Components.Xml;
using OpenBreed.Core.Modules.Physics.Components;
using System.Xml.Serialization;

namespace OpenBreed.Core.Modules.Rendering.Components.Xml
{
    [XmlRoot("Motion")]
    public class XmlMotionComponent : XmlComponentTemplate, IMotionComponentTemplate
    {
    }
}