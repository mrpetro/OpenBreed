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

        private List<StateMachine> fsmList;

        #endregion Private Fields

        #region Internal Constructors

        internal Entity(ICore core, List<IEntityComponent> initialComponents)
        {
            fsmList = new List<StateMachine>();
            FsmList = new ReadOnlyCollection<StateMachine>(fsmList);

            Core = core ?? throw new ArgumentNullException(nameof(core));

            components = initialComponents ?? new List<IEntityComponent>();
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Internal Constructors

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

        public bool RunScript(string name, params object[] args)
        {
            throw new NotImplementedException();
            //   Core.LuaState["ce"] = this;

            //   Core.LuaState.DoString(@"
            //res1 = ce.Id
            //");

            //   var r = Core.LuaState["res1"];

            //   return false;
        }

        public void PostCommand(ICommand command)
        {
            Core.Commands.Post(this, command);
        }

        public void RaiseEvent(string eventType, EventArgs eventArgs)
        {
            Core.Events.Raise(this, eventType, eventArgs);
        }

        public void Subscribe(string eventType, Action<object, EventArgs> callback)
        {
            Core.Events.Subscribe(this, eventType, callback);
        }

        public void Unsubscribe(string eventType, Action<object, EventArgs> callback)
        {
            Core.Events.Unsubscribe(this, eventType, callback);
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
            RaiseEvent(CoreEventTypes.ENTITY_LEFT_WORLD, new EntityLeftWorldEventArgs(this, world));
        }

        private void OnEnteredWorld(World world)
        {
            RaiseEvent(CoreEventTypes.ENTITY_ENTERED_WORLD, new EntityEnteredWorldEventArgs(this, world));
        }

        #endregion Private Methods
    }
}