using OpenBreed.Wecs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common
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

        internal OnTriggerComponent(IEnumerable<OnTriggerAction> actions)
        {
            Actions = actions.ToArray();
        }

        #endregion Internal Constructors

        #region Public Properties

        public OnTriggerAction[] Actions { get; }

        #endregion Public Properties
    }

    public sealed class OnTriggerComponentFactory : ComponentFactoryBase<IOnTriggerComponentTemplate>
    {
        #region Public Constructors

        public OnTriggerComponentFactory()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IOnTriggerComponentTemplate template)
        {
            var actions = GetActions(template.Actions);
            return new OnTriggerComponent(actions);
        }

        #endregion Protected Methods

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