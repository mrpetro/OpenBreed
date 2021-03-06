﻿using OpenBreed.Core.Commands;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems
{
    public abstract class SystemBase : ISystem
    {
        #region Private Fields

        private readonly List<Entity> toAdd = new List<Entity>();
        private readonly List<Entity> toRemove = new List<Entity>();

        private readonly List<Type> requiredComponentTypes = new List<Type>();

        private readonly Queue<IEntityCommand> commandQueue = new Queue<IEntityCommand>();

        private Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// World which owns this system
        /// </summary>
        public World World { get; private set; }

        /// <summary>
        /// Id of the phase in which system will be updated
        /// </summary>
        public int PhaseId { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initialize the system when world is created
        /// </summary>
        /// <param name="world">World that this system is initialized on</param>
        public virtual void Initialize(World world)
        {
            if (World != null)
                throw new InvalidOperationException("World sytem already initialized.");

            World = world;
        }

        /// <summary>
        /// Deinitialize the system when world is destroyed
        /// </summary>
        public virtual void Deinitialize()
        {
            if (World == null)
                throw new InvalidOperationException("World sytem already deinitialized.");

            World = null;
        }

        public bool Matches(Entity entity)
        {
            foreach (var type in requiredComponentTypes)
            {
                if (!entity.Components.Any(item => type.IsAssignableFrom(item.GetType())))
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

        protected void RegisterHandler<TCommand>(Func<TCommand, bool> cmdHandler) where TCommand : IEntityCommand
        {
            handlers.Add(typeof(TCommand), cmdHandler);
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
                    OnRemoveEntity((Entity)toRemove[i]);

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                    OnAddEntity((Entity)toAdd[i]);

                toAdd.Clear();
            }
        }

        public bool HandleCommand(ICommand cmd)
        {
            return false;
        }

        #endregion Public Methods

        #region Protected Methods

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

        protected int Require<C>() where C : IEntityComponent
        {
            var type = typeof(C);

            var typeIndex = requiredComponentTypes.IndexOf(type);

            if (typeIndex >= 0)
                return typeIndex;
            else
            {
                requiredComponentTypes.Add(type);
                return requiredComponentTypes.Count - 1;
            }
        }

        #endregion Protected Methods
    }
}