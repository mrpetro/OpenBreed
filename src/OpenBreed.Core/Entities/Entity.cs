using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;

using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Events;
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
    internal class Entity : IEntity
    {
        #region Private Fields

        private readonly List<IEntityComponent> components = new List<IEntityComponent>();

        private List<IStateMachine> fsmList;

        #endregion Private Fields

        #region Internal Constructors

        internal Entity(ICore core, List<IEntityComponent> initialComponents)
        {
            fsmList = new List<IStateMachine>();
            FsmList = new ReadOnlyCollection<IStateMachine>(fsmList);

            Core = core ?? throw new ArgumentNullException(nameof(core));

            components = initialComponents ?? new List<IEntityComponent>();
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Internal Constructors

        #region Public Properties

        public ReadOnlyCollection<IEntityComponent> Components { get; }
        public ReadOnlyCollection<IStateMachine> FsmList { get; }

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

        //public void Impulse<TState, TImpulse>(TImpulse impulse) where TState : struct, IConvertible where TImpulse : struct, IConvertible
        //{
        //    var sm = FsmList.OfType<StateMachine<TState, TImpulse>>().FirstOrDefault();
        //    sm.Perform(impulse);
        //}

        public StateMachine<TState, TImpulse> AddFsm<TState, TImpulse>() where TState : struct, IConvertible where TImpulse : struct, IConvertible
        {
            var newFsm = new StateMachine<TState, TImpulse>(this);
            fsmList.Add(newFsm);
            return newFsm;
        }

        public T TryGetComponent<T>()
        {
            return components.OfType<T>().FirstOrDefault();
        }

        public bool Contains<T>()
        {
            return components.OfType<T>().Any();
        }

        public T GetComponent<T>()
        {
            return components.OfType<T>().First();
        }

        public void PostCommand(ICommand command)
        {
            Core.Commands.Post(this, command);
        }

        public void RaiseEvent<T>(T eventArgs) where T : EventArgs
        {
            Core.Events.Raise(this, eventArgs);
        }

        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            Core.Events.Subscribe(this, callback);
        }

        public void Unsubscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            Core.Events.Unsubscribe(this, callback);
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
            return $"Entity:{Id}";
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

            OnLeftWorld(from);
        }

        internal void Initialize(World world)
        {
            //Remember in what world entity is
            World = world;

            foreach (var fsm in fsmList)
                fsm.Initialize();

            OnEnteredWorld(World);
        }

        #endregion Internal Methods

        #region Private Methods

        private void OnLeftWorld(World world)
        {
            RaiseEvent(new EntityLeftWorldEventArgs(this, world));
        }

        private void OnEnteredWorld(World world)
        {
            RaiseEvent(new EntityEnteredWorldEventArgs(this, world));
        }

        #endregion Private Methods
    }
}