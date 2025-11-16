using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems
{
    public class EventSystemManager : IEventSystemManager
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public EventSystemManager(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public delegate void EventCaller(EventArgs args);

        public void RegisterSystem(IEventSystem system)
        {
            var genericSystemType = system.GetType().GetInterfaces().Where(item => item.IsGenericType).FirstOrDefault(item => typeof(IEventSystem).IsAssignableFrom(item));

            if (genericSystemType is null)
            {
                throw new InvalidOperationException("Expected event system which implements IEventSystem<TEvent>.");
            }

            var eventType = genericSystemType.GenericTypeArguments.FirstOrDefault();

            if (eventType is null)
            {
                throw new InvalidOperationException("Expected event system with generic type argument.");
            }

            if (!typeof(EventArgs).IsAssignableFrom(eventType))
            {
                throw new InvalidOperationException("Expected event system with generic type argument that inherits EventArgs.");
            }

            var updateMethodInfo = genericSystemType.GetMethod("Update", BindingFlags.Instance | BindingFlags.Public);

            if (updateMethodInfo is null)
            {
                throw new InvalidOperationException("Expected Update method.");
            }

            var handler = updateMethodInfo.CreateDelegate(system);

            eventsMan.Subscribe(eventType, handler);
        }

        #endregion Public Methods
    }
}