using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Wecs.Systems
{
    public abstract class SystemBase : ISystem
    {
        #region Private Fields

        private readonly List<Entity> toAdd = new List<Entity>();

        private readonly List<Entity> entities = new List<Entity>();

        private readonly List<Entity> toRemove = new List<Entity>();

        private readonly List<Type> requiredComponentTypes = new List<Type>();

        private readonly List<Type> forbiddenComponentTypes = new List<Type>();

        private readonly Queue<IEntityCommand> commandQueue = new Queue<IEntityCommand>();

        private Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        public IReadOnlyCollection<Entity> Entities { get; }

        #endregion Private Fields

        #region Protected Constructors

        protected SystemBase()
        {
            WorldId = World.NO_WORLD;

            Entities = new ReadOnlyCollection<Entity>(entities);
            RequiredComponentTypes = new ReadOnlyCollection<Type>(requiredComponentTypes);

        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// ID of world which owns this system
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Id of the phase in which system will be updated
        /// </summary>
        public int PhaseId { get; }

        public IReadOnlyCollection<Type> RequiredComponentTypes { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initialize the system when world is created
        /// </summary>
        /// <param name="world">World that this system is initialized on</param>
        public virtual void Initialize(World world)
        {
            if (WorldId != World.NO_WORLD)
                throw new InvalidOperationException("World sytem already initialized.");

            WorldId = world.Id;
        }

        /// <summary>
        /// Deinitialize the system when world is destroyed
        /// </summary>
        public virtual void Deinitialize()
        {
            if (WorldId == World.NO_WORLD)
                throw new InvalidOperationException("World sytem already deinitialized.");

            WorldId = World.NO_WORLD;
        }

        public bool Matches(Entity entity)
        {
            foreach (var type in forbiddenComponentTypes)
            {
                if (entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            foreach (var type in requiredComponentTypes)
            {
                if (!entity.ComponentTypes.Any(item => type.IsAssignableFrom(item)))
                    return false;
            }

            return true;
        }

        public void AddEntity(Entity entity)
        {
            toAdd.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            toRemove.Add(entity);
        }

        public virtual bool EnqueueCommand(IEntityCommand command)
        {
            commandQueue.Enqueue(command);
            return false;
        }

        public void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                    DoRemoveEntity(toRemove[i]);

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                    DoAddEntity(toAdd[i]);

                toAdd.Clear();
            }
        }

        public bool HandleCommand(ICommand cmd)
        {
            return false;
        }

        public bool ContainsEntity(Entity entity)
        {
            return entities.Contains(entity);
        }

        #endregion Public Methods

        #region Protected Methods

        protected void RegisterHandler<TCommand>(Func<TCommand, bool> cmdHandler) where TCommand : IEntityCommand
        {
            handlers.Add(typeof(TCommand), cmdHandler);
        }

        protected virtual bool DenqueueCommand(IEntityCommand entityCommand)
        {
            if (handlers.TryGetValue(entityCommand.GetType(), out Delegate handler))
            {
                handler.DynamicInvoke(entityCommand);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void ExecuteCommands()
        {
            while (commandQueue.Count > 0)
            {
                if (!DenqueueCommand(commandQueue.Dequeue()))
                {
                    //TODO: Log not handled command here
                }
            }
        }

        protected abstract void OnRemoveEntity(Entity entity);

        protected abstract void OnAddEntity(Entity entity);

        protected int RequireEntityWith<TComponent>() where TComponent : IEntityComponent
        {
            var type = typeof(TComponent);

            var typeIndex = requiredComponentTypes.IndexOf(type);

            if (typeIndex >= 0)
                return typeIndex;
            else
            {
                requiredComponentTypes.Add(type);
                return requiredComponentTypes.Count - 1;
            }
        }

        protected int RequireEntityWithout<TComponent>() where TComponent : IEntityComponent
        {
            var type = typeof(TComponent);

            var typeIndex = forbiddenComponentTypes.IndexOf(type);

            if (typeIndex >= 0)
                return typeIndex;
            else
            {
                forbiddenComponentTypes.Add(type);
                return forbiddenComponentTypes.Count - 1;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void DoRemoveEntity(Entity entity)
        {
            entities.Add(entity);

            OnRemoveEntity(entity);
        }

        private void DoAddEntity(Entity entity)
        {
            entities.Remove(entity);

            OnAddEntity(entity);
        }

        #endregion Private Methods
    }
}