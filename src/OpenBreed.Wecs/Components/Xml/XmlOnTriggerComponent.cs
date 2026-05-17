using OpenBreed.Wecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components.Xml
{
    public class XmlOnTriggerActionTemplate : IOnTriggerActionTemplate
    {
        #region Public Properties

        [XmlAttribute("Trigger")]
        public string Trigger { get; set; }

        [XmlAttribute("Action")]
        public string Action { get; set; }

        #endregion Public Properties
    }

    [XmlRoot("OnTrigger")]
    public class XmlOnTriggerComponent : XmlComponentTemplate, IOnTriggerComponentTemplate
    {
        #region Public Properties

        [XmlIgnore]
        public IEnumerable<IOnTriggerActionTemplate> Actions => XmlActions.Cast<IOnTriggerActionTemplate>();

        [XmlArray("Actions")]
        [XmlArrayItem(ElementName = "Action")]
        public XmlOnTriggerActionTemplate[] XmlActions { get; set; } = Array.Empty<XmlOnTriggerActionTemplate>();

        #endregion Public Properties

        #region Public Methods

        public override IEntityComponent ToComponent(IServiceProvider serviceProvider)
        {
            var actions = GetActions(Actions);
            return new OnTriggerComponent(actions);
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<OnTriggerAction> GetActions(IEnumerable<IOnTriggerActionTemplate> actionTemplates)
        {
            foreach (var actionTemplate in actionTemplates)
            {
                yield return new OnTriggerAction(actionTemplate.Trigger, actionTemplate.Action);
            }
        }

        #endregion Private Methods
    }
}