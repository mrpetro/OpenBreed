using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems
{
    public abstract class WorldSystem : IWorldSystem
    {
        #region Private Fields

        private readonly List<IEntity> toAdd = new List<IEntity>();
        private readonly List<IEntity> toRemove = new List<IEntity>();

        private readonly List<Type> requiredComponentTypes = new List<Type>();

        #endregion Private Fields

        #region Protected Constructors

        protected WorldSystem(ICore core)
        {
            Core = core;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Reference to Core
        /// </summary>
        public ICore Core { get; }

        /// <summary>
        /// World which owns this system
        /// </summary>
        public World World { get; private set; }

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

        public bool Matches(IEntity entity)
        {
            foreach (var type in requiredComponentTypes)
            {
                if (!entity.Components.Any(item => type.IsAssignableFrom(item.GetType())))
                    return false;
            }

            return true;
        }

        public void AddEntity(IEntity entity)
        {
            toAdd.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            toRemove.Add(entity);
        }

        public virtual bool ExecuteCommand(ICommand cmd)
        {
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

        #endregion Public Methods

        #region Protected Methods

        protected abstract void OnRemoveEntity(IEntity entity);

        protected abstract void OnAddEntity(IEntity entity);

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