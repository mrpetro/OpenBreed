using OpenBreed.Wecs.Components.Common.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Rendering.Xml
{
    [XmlRoot("Palette")]
    public class XmlPaletteComponent : XmlComponentTemplate, IPaletteComponentTemplate
    {
    }
}