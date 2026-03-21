using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        #region Public Delegates

        public delegate void EventCaller(EventArgs args);

        #endregion Public Delegates

        #region Public Methods

        public void RegisterSystem(IEventSystem system)
        {
            var genericSystemTypes = system.GetType().GetInterfaces().Where(item => item.IsGenericType).Where(item => typeof(IEventSystem).IsAssignableFrom(item));

            foreach (var genericSystemType in genericSystemTypes)
            {
                RegisterGenericSystem(system, genericSystemType);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterGenericSystem(IEventSystem system, Type genericSystemType)
        {
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

            var updateMethodInfo = genericSystemType.GetMethod("OnEvent", BindingFlags.Instance | BindingFlags.Public);

            if (updateMethodInfo is null)
            {
                throw new InvalidOperationException("Expected Update method.");
            }

            var handler = updateMethodInfo.CreateDelegate(system);

            eventsMan.Subscribe(eventType, handler);
        }

        #endregion Private Methods
    }
}