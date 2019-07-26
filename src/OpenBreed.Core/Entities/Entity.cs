using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    public class Entity : IEntity
    {
        #region Private Fields

        private readonly List<IEntityComponent> components = new List<IEntityComponent>();

        #endregion Private Fields

        #region Public Constructors

        public Entity(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }

        public StateMachine StateMachine { get; private set; }

        public ICore Core { get; }

        public World World { get; private set; }

        public int Id { get; internal set; }

        public object DebugData { get; set; }

        #endregion Public Properties

        #region Public Methods

        public StateMachine AddStateMachine()
        {
            if (StateMachine != null)
                throw new InvalidOperationException("State machine added to entity.");

            StateMachine = new StateMachine(this);
            return StateMachine;
        }

        public void PostMsg(IEntityMsg msg)
        {
            Core.MessageBus.Enqueue(this, msg);
        }

        public void RaiseEvent(IEvent ev)
        {
            Core.EventBus.Enqueue(this, ev);
        }

        public void Subscribe(string eventType, Action<object, IEvent> callback)
        {
            Core.EventBus.Subscribe(eventType, callback);
        }

        public void Unsubscribe(string eventType, Action<object, IEvent> callback)
        {
            Core.EventBus.Unsubscribe(eventType, callback);
        }

        public void Add(IEntityComponent component)
        {
            components.Add(component);
        }

        public virtual void EnterWorld(World world)
        {
            world.AddEntity(this);
        }

        public virtual void LeaveWorld()
        {
            World.RemoveEntity(this);
        }

        public bool Remove(IEntityComponent component)
        {
            return components.Remove(component);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Deinitialize()
        {
            if (StateMachine != null)
                StateMachine.Deinitialize();

            //Forget the world in which entity was
            World = null;
        }

        internal void Initialize(World world)
        {
            //Remember in what world entity is
            World = world;

            if (StateMachine != null)
                StateMachine.Initialize();
        }

        #endregion Internal Methods
    }
}