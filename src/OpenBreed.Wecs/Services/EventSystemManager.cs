using OpenBreed.Common.Interface.Extensions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Extensions;
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
using System.Xml.Linq;

namespace OpenBreed.Wecs.Systems
{
    public class EventSystemManager : IEventSystemManager
    {
        #region Private Fields

        private readonly IEventsMan eventsMan;
        private readonly Lazy<IWorldMan> lazyWorldMan;
        private readonly IEntityMan entityMan;
        private readonly IEntityClassMan entityClassMan;
        private Dictionary<Type, Delegate> onEventCallbacks = new Dictionary<Type, Delegate>();

        #endregion Private Fields

        #region Public Constructors

        public EventSystemManager(
            IEventsMan eventsMan,
            Lazy<IWorldMan> lazyWorldMan,
            IEntityMan entityMan,
            IEntityClassMan entityClassMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
            this.lazyWorldMan = lazyWorldMan ?? throw new ArgumentNullException(nameof(lazyWorldMan));
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.entityClassMan = entityClassMan ?? throw new ArgumentNullException(nameof(entityClassMan));
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

        public bool CanInvoke<TEvent>(IEventSystem<TEvent> system, TEvent e, IWorld world) where TEvent : EventArgs
        {
            var method = system.GetType().GetMethod(nameof(system.OnEvent), BindingFlags.Instance | BindingFlags.Public, [typeof(TEvent), typeof(IWorld)]);

            if (method is null)
            {
                throw new InvalidOperationException("Method not found");
            }


            if (e is WorldEvent worldEvent)
            {
                var eventParameter = method.GetParameters().First();

                var eventFilters = eventParameter.GetCustomAttributes<WorldEventFilterAttribute>();
                if (!EvaluateWorldEventFilters(eventFilters, worldEvent, world))
                {
                    return false;
                }
            }

            if (e is IEntityEvent entityEvent)
            {
                var eventParameter = method.GetParameters().First();

                var eventFilters = eventParameter.GetCustomAttributes<EntityEventFilterAttribute>();
                if (!EvaluateEntityEventFilters(eventFilters, entityEvent, world))
                {
                    return false;
                }
            }

            return true;
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

            var updateMethodInfo = thisType.GetMethod(nameof(UpdateSystems), BindingFlags.Instance | BindingFlags.NonPublic);

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
                    if (CanInvoke(system, e, world))
                    {
                        system.OnEvent(e, world);
                    }

                    //var entities = world.GetMatchingEntities(system);
                }
            });
        }

        private bool EvaluateWorldEventFilters(IEnumerable<WorldEventFilterAttribute> eventFilters, WorldEvent e, IWorld world)
        {
            var eventSourceWorld = lazyWorldMan.Value.GetById(e.WorldId);

            foreach (var eventFilter in eventFilters)
            {
                switch (eventFilter)
                {
                    case SourceWorldWithNameFilterAttribute worldWithNameFilter:
                        if (!eventSourceWorld.Name.StartsWith(worldWithNameFilter.Name))
                        {
                            return false;
                        }

                        break;

                    case TargetWorldAsSourceFilter sameTargetFilter:

                        if (eventSourceWorld.Id != world.Id)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        private bool EvaluateEntityEventFilters(IEnumerable<EntityEventFilterAttribute> eventFilters, IEntityEvent e, IWorld world)
        {
            var eventEntity = entityMan.GetById(e.EntityId);

            foreach (var eventFilter in eventFilters)
            {
                switch (eventFilter)
                {
                    case EntityTriggerActionFilter entityTriggerActionFilter:

                        var actions = eventEntity.GetActionsOnTrigger(entityTriggerActionFilter.TriggerName);

                        if (actions is null || !actions.Contains(entityTriggerActionFilter.ActionName))
                        {
                            return false;
                        }

                        break;

                    case EntityWithTagFilter entityWithTagFilter:

                        if (!string.Equals(entityWithTagFilter.Tag, eventEntity.Tag, StringComparison.Ordinal))
                        {
                            return false;
                        }

                        break;

                    case EntityOfClassFilter entityOfClassFilter:

                        var filterClass = entityClassMan.GetByName(entityOfClassFilter.ClassName);
                        if (!filterClass.IsOrInheritsFrom(eventEntity.ClassId))
                        {
                            return false;
                        }

                        break;

                    default:
                        break;
                }
            }

            return true;
        }

        #endregion Private Methods
    }
}