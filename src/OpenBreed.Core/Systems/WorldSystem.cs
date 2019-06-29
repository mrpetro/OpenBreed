using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Systems
{
    public abstract class WorldSystem : IWorldSystem
    {
        #region Protected Fields

        #endregion Protected Fields

        #region Private Fields

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
            if(World == null)
                throw new InvalidOperationException("World sytem already deinitialized.");

            World = null;
        }

        public bool Matches(IEntity entity)
        {
            foreach (var type in requiredComponentTypes)
            {
                if(!entity.Components.Any(item => type.IsAssignableFrom(item.GetType())))
                    return false;
            }

            return true;
        }

        public virtual void AddEntity(IEntity entity)
        {
        }

        public virtual void RemoveEntity(IEntity entity)
        {
        }

        #endregion Public Methods

        #region Protected Methods

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

        public virtual bool HandleMsg(IEntity sender, IEntityMsg message)
        {
            return false;
        }

        //public void PostEvent(ISystemEvent systemEvent)
        //{
        //    World.PostEvent(this, systemEvent);
        //}

        #endregion Protected Methods
    }
}