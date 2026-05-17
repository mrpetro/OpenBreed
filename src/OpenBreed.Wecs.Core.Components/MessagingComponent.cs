using OpenBreed.Core.Abstractions.Managers;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Core.Components
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
}