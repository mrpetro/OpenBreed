using OpenBreed.Wecs.Core.Components.Xml;
using OpenBreed.Wecs.Components.Xml;
using OpenTK;
using OpenTK.Mathematics;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Rendering.Components.Xml
{
    [XmlRoot("Palette")]
    public class XmlPaletteComponent : XmlComponentTemplate, IPaletteComponentTemplate
    {
    }
}