using OpenBreed.Wecs.Components.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Scripting.Xml
{
    [XmlRoot("Script")]
    public class XmlScriptComponent : XmlComponentTemplate, IScriptComponentTemplate
    {
        #region Public Properties

        [XmlElement("ScriptId")]
        public string ScriptId { get; set; }

        #endregion Public Properties
    }
}