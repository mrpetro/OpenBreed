using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    public class Entity : IEntity
    {
        #region Private Fields

        private readonly List<IEntityComponent> components = new List<IEntityComponent>();

        private List<StateMachine> fsmList;

        #endregion Private Fields

        #region Public Constructors

        public Entity(ICore core)
        {
            fsmList = new List<StateMachine>();
            FsmList = new ReadOnlyCollection<StateMachine>(fsmList);

            Core = core ?? throw new ArgumentNullException(nameof(core));
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<World> RemovedFromWorld;

        #endregion Public Events

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }
        public ReadOnlyCollection<StateMachine> FsmList { get; }

        public ICore Core { get; }

        /// <summary>
        /// Property for user purpose data
        /// </summary>
        public object Tag { get; set; }

        public World World { get; private set; }

        public int Id { get; internal set; }

        public object DebugData { get; set; }

        public IEnumerable<string> CurrentStateNames => FsmList.Select(item => item.ToString());

        #endregion Public Properties

        #region Public Methods

        public StateMachine AddFSM(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (fsmList.Any(item => item.Name == name))
                throw new InvalidOperationException($"State machine '{name}' already exist.");

            var newFsm = new StateMachine(name, this);
            fsmList.Add(newFsm);
            return newFsm;
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
            Core.EventBus.Subscribe(this, eventType, callback);
        }

        public void Unsubscribe(string eventType, Action<object, IEvent> callback)
        {
            Core.EventBus.Unsubscribe(this, eventType, callback);
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

        public override string ToString()
        {
            return $"Entity({Id})";
        }

        private void OnRemoved(World world)
        {
            RemovedFromWorld?.Invoke(this, world);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Deinitialize()
        {
            foreach (var fsm in fsmList)
                fsm.Deinitialize();

            var from = World;

            //Forget the world in which entity was
            World = null;
            OnRemoved(from);
        }

        internal void Initialize(World world)
        {
            //Remember in what world entity is
            World = world;

            foreach (var fsm in fsmList)
                fsm.Initialize();
        }

        #endregion Internal Methods
    }
}