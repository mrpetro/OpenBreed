using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Gui.Xml
{
    [XmlRoot("Cursor")]
    public class XmlCursorInputComponent : XmlComponentTemplate, ICursorInputComponentTemplate
    {
    }
}