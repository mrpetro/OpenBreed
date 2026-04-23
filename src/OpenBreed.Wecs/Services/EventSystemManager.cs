using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Services;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
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
        private readonly Lazy<IWorldMan> lazyWorldMan;
        private Dictionary<Type, Delegate> onEventCallbacks = new Dictionary<Type, Delegate>();


        #endregion Private Fields

        #region Public Constructors

        public EventSystemManager(IEventsMan eventsMan, Lazy<IWorldMan> lazyWorldMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.lazyWorldMan = lazyWorldMan ?? throw new ArgumentNullException(nameof(lazyWorldMan));
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

            if (onEventCallbacks.ContainsKey(eventType))
            {
                return;
            }

            //var updateMethodInfo = genericSystemType.GetMethod("OnEvent", BindingFlags.Instance | BindingFlags.Public);
            var thisType = this.GetType();

            var updateMethodInfo = thisType.GetMethod("UpdateSystems", BindingFlags.Instance | BindingFlags.NonPublic);

            if (updateMethodInfo is null)
            {
                throw new InvalidOperationException("Expected UpdateSystems method.");
            }

            var genericUpdateMethodInfo = updateMethodInfo.MakeGenericMethod(eventType);

            var delegateType = typeof(Action<>).MakeGenericType(eventType);
            var updateSystemsDelegate = genericUpdateMethodInfo.CreateDelegate(delegateType, this);

            //// parameter: (TEvent e)
            //var eventParam = Expression.Parameter(eventType, "e");

            //// call: this.UpdateSystems<TEvent>(e, extra)
            //var call = Expression.Call(
            //    Expression.Constant(this),
            //    genericUpdateMethodInfo,
            //    eventParam,
            //    extraConst
            //);

            //// lambda: (TEvent e) => UpdateSystems<TEvent>(e, extra)
            //var delegateType = typeof(Action<>).MakeGenericType(eventType);

            //var lambda = Expression.Lambda(delegateType, call, eventParam);

            //var updateSystemsDelegate = lambda.Compile();

            eventsMan.Subscribe(eventType, updateSystemsDelegate);

            onEventCallbacks.Add(eventType, updateSystemsDelegate);
        }

        private void UpdateSystems<TEvent>(TEvent e) where TEvent : EventArgs
        {
            lazyWorldMan.Value.Update((world) =>
            {
                var systems = world.GetSystems<IEventSystem<TEvent>>();

                foreach (var system in systems)
                {
                    if (CanInvoke(system, e))
                    {
                        system.OnEvent(world, e);
                    }

                    //var entities = world.GetMatchingEntities(system);

                }    
            });
        }

        public bool CanInvoke<TEvent>(IEventSystem<TEvent> system, TEvent e) where TEvent : EventArgs
        {
            var method = system.GetType().GetMethod(nameof(system.OnEvent), BindingFlags.Instance | BindingFlags.Public, [typeof(IWorld), typeof(TEvent)]);

            if (method is null)
            {
                throw new InvalidOperationException("Method not found");
            }

            var parameter = method.GetParameters().Last();

            var validators = parameter.GetCustomAttributes<OnEventParameterRequireAttribute>();

            foreach (var validator in validators)
            {
                if (!validator.IsValid(lazyWorldMan.Value, e))
                {
                    return false;
                }
            }

            return true;
        }


        #endregion Private Methods
    }
}