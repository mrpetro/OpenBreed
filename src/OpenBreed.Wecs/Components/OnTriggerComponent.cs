using OpenBreed.Wecs.Abstractions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace OpenBreed.Wecs.Components
{
    public interface IOnTriggerActionTemplate
    {
        #region Public Properties

        string Trigger { get; set; }
        string Action { get; set; }

        #endregion Public Properties
    }

    public interface IOnTriggerComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        IEnumerable<IOnTriggerActionTemplate> Actions { get; }

        #endregion Public Properties
    }

    public class OnTriggerAction
    {
        #region Internal Constructors

        internal OnTriggerAction(string trigger, string action)
        {
            Trigger = trigger;
            Action = action;
        }

        #endregion Internal Constructors

        #region Public Properties

        public string Trigger { get; }
        public string Action { get; }

        #endregion Public Properties
    }

    [ComponentName("OnTrigger")]
    public class OnTriggerComponent : IEntityComponent
    {
        #region Internal Constructors

        internal OnTriggerComponent()
        {
        }

        internal OnTriggerComponent(IEnumerable<OnTriggerAction> actions)
        {
            Actions = actions.ToArray();
        }

        #endregion Internal Constructors

        #region Public Properties

        public OnTriggerAction[] Actions { get; }

        #endregion Public Properties
    }

}