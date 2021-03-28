using OpenBreed.Core.Managers;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Components.Common
{
    public interface IMessagingComponentTemplate : IComponentTemplate
    {
        #region Public Properties

        int[] Messages { get; }

        #endregion Public Properties
    }

    public class MessagingComponent : IEntityComponent
    {
        #region Public Constructors

        public MessagingComponent()
        {
            Messages = new List<IMsg>();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IMsg> Messages { get; }

        #endregion Public Properties
    }

    public sealed class MessagingComponentFactory : ComponentFactoryBase<IMessagingComponentTemplate>
    {
        #region Internal Constructors

        internal MessagingComponentFactory()
        {
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override IEntityComponent Create(IMessagingComponentTemplate template)
        {
            return new MessagingComponent();
        }

        #endregion Protected Methods
    }
}