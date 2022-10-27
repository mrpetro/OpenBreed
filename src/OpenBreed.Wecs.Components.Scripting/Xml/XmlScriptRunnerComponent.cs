using OpenBreed.Wecs.Components.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Scripting.Xml
{
    public class XmlScriptRunTemplate : IScriptRunTemplate
    {
        [XmlElement("ScriptId")]
        public string ScriptId { get; set; }

        [XmlElement("TriggerName")]
        public string TriggerName { get; set; }

        [XmlElement("ScriptFunction")]
        public string ScriptFunction { get; set; }
    }

    [XmlRoot("ScriptRunner")]
    public class XmlScriptRunnerComponent : XmlComponentTemplate, IScriptRunnerComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public IEnumerable<IScriptRunTemplate> Runs => XmlRuns.Cast<IScriptRunTemplate>();

        [XmlArray("Runs")]
        [XmlArrayItem(ElementName = "Run")]
        public XmlScriptRunTemplate[] XmlRuns { get; set; }

        #endregion Public Properties
    }
}