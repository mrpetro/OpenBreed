using OpenBreed.Wecs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IOnTriggerComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        string ActionName { get; }

        #endregion Public Properties
    }


    [ComponentName("OnTrigger")]
    public class OnTriggerComponent : IEntityComponent
    {
        public string ActionName { get; }

        public OnTriggerComponent(string actionName)
        {
            ActionName = actionName;
        }
    }

    public sealed class OnTriggerComponentFactory : ComponentFactoryBase<IOnTriggerComponentTemplate>
    {
        #region Internal Constructors

        public OnTriggerComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IOnTriggerComponentTemplate template)
        {
            return new OnTriggerComponent(template.ActionName);
        }

        #endregion Protected Methods
    }
}
