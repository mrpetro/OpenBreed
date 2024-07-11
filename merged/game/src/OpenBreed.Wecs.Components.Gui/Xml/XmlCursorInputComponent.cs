using OpenBreed.Wecs.Components.Xml;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Gui.Xml
{
    [XmlRoot("Cursor")]
    public class XmlCursorInputComponent : XmlComponentTemplate, ICursorInputComponentTemplate
    {
        #region Public Properties

        //[XmlArray("Actions")]
        //[XmlArrayItem(ElementName = "Action")]
        //public ICursorAction[] Actions { get; set; }

        #endregion Public Properties
    }
}